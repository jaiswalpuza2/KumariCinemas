using System;
using System.IO;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;

namespace MovieTicketSystem
{
    public partial class Setup : System.Web.UI.Page
    {
        protected void btnSetup_Click(object sender, EventArgs e)
        {
            string connStr = ConfigurationManager.ConnectionStrings["CinemaDB"].ConnectionString;
            string scriptPath = Server.MapPath("~/schema.sql");
            
            if (!File.Exists(scriptPath))
            {
                litStatus.Text = "<div class='alert alert-danger'>Error: schema.sql file not found.</div>";
                return;
            }

            string script = File.ReadAllText(scriptPath);
            // Split script into individual commands by either / or ;
            // Split script into individual commands by semicolon
            // This assumes standard SQL (no PL/SQL blocks that use semicolons internally)
            // or that PL/SQL blocks are separated by /
            string[] rawChunks = script.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            System.Collections.Generic.List<string> allCommands = new System.Collections.Generic.List<string>();

            foreach (string chunk in rawChunks)
            {
                if (chunk.ToUpper().Contains("BEGIN"))
                {
                    allCommands.Add(chunk.Trim());
                }
                else
                {
                    string[] subCmds = chunk.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var s in subCmds)
                    {
                        if (!string.IsNullOrWhiteSpace(s)) allCommands.Add(s.Trim());
                    }
                }
            }

            using (OracleConnection con = new OracleConnection(connStr))
            {
                try
                {
                    con.Open();
                    int successCount = 0;
                    int failCount = 0;
                    string errors = "";

                    foreach (string cmdText in allCommands)
                    {
                        // Clean the command (remove comments for execution)
                        string cleanedCmd = "";
                        using (StringReader reader = new StringReader(cmdText))
                        {
                            string line;
                            while ((line = reader.ReadLine()) != null)
                            {
                                string l = line.Trim();
                                if (string.IsNullOrWhiteSpace(l)) continue;
                                
                                // Remove inline comments
                                int commentIdx = l.IndexOf("--");
                                if (commentIdx >= 0)
                                {
                                    l = l.Substring(0, commentIdx).Trim();
                                }

                                if (!string.IsNullOrWhiteSpace(l))
                                {
                                    cleanedCmd += l + " ";
                                }
                            }
                        }
                        
                        cleanedCmd = cleanedCmd.Trim();
                        if (string.IsNullOrWhiteSpace(cleanedCmd) || cleanedCmd.ToUpper() == "COMMIT")
                            continue;

                        try
                        {
                            using (OracleCommand cmd = new OracleCommand(cleanedCmd, con))
                            {
                                cmd.ExecuteNonQuery();
                                successCount++;
                            }
                        }
                        catch (OracleException ex)
                        {
                            // Ignore "table/view does not exist" or "sequence does not exist" during DROP
                            if (cleanedCmd.ToUpper().StartsWith("DROP") && (ex.Number == 942 || ex.Number == 2289))
                            {
                                successCount++; // Count as success since the goal (no table) is met
                                continue;
                            }
                            
                            failCount++;
                            errors += "<li><b>Error " + ex.Number + "</b>: " + ex.Message + "<br/><code>" + (cleanedCmd.Length > 100 ? cleanedCmd.Substring(0, 100) + "..." : cleanedCmd) + "</code></li>";
                        }
                        catch (Exception ex)
                        {
                            failCount++;
                            errors += "<li><b>General Error</b>: " + ex.Message + "</li>";
                        }
                    }

                    if (failCount == 0)
                    {
                        // Verification Step: List columns of TICKET to be absolutely sure
                        string colsStr = "";
                        try {
                            using (OracleCommand vCmd = new OracleCommand("SELECT column_name FROM user_tab_cols WHERE table_name = 'TICKET' ORDER BY column_id", con))
                            {
                                using (OracleDataReader dr = vCmd.ExecuteReader())
                                {
                                    while (dr.Read()) colsStr += dr[0].ToString() + " ";
                                }
                            }
                        } catch { colsStr = "Could not verify columns."; }

                        litStatus.Text = "<div class='alert alert-success'><h4>Success!</h4>All " + successCount + " commands processed successfully.<br/>Verified TICKET columns: <b>" + colsStr + "</b><br/><br/><a href='Default.aspx' class='btn btn-success'>Go to Dashboard</a></div>";
                    }
                    else
                    {
                        litStatus.Text = "<div class='alert alert-warning'><h4>Completed with " + failCount + " error(s)</h4><ul>" + errors + "</ul><br/><a href='Default.aspx' class='btn btn-primary'>Go to Dashboard</a></div>";
                    }
                }
                catch (Exception ex)
                {
                    litStatus.Text = "<div class='alert alert-danger'>Connection Error: " + ex.Message + "</div>";
                }
            }
        }
    }
}
