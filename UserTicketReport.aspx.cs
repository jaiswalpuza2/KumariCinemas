using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using System.Web.UI.WebControls;

namespace MovieTicketSystem
{
    public partial class UserTicketReport : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["CinemaDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserRole"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            string diag = "";
            try {
                diag = EnsureSchema();
            } catch (Exception ex) { 
                diag = "Schema Check Fatal Error: " + ex.Message; 
            }

            if (!IsPostBack)
            {
                if (Session["UserRole"].ToString() == "Admin")
                {
                    phUserSelection.Visible = true;
                    LoadUsers();
                }
                else
                {
                    phUserSelection.Visible = false;
                    int userId = Convert.ToInt32(Session["UserID"]);
                    ShowUserDetails(userId);
                    LoadTicketHistory(userId);
                    pnlReport.Visible = true;
                }
            }
        }

        private string EnsureSchema()
        {
            string log = "";
            using (OracleConnection con = new OracleConnection(connStr))
            {
                con.Open();
                
                // Check if TICKET table exists
                string checkTableQuery = "SELECT COUNT(*) FROM user_tables WHERE table_name = 'TICKET'";
                using (OracleCommand cmd = new OracleCommand(checkTableQuery, con))
                {
                    if (Convert.ToInt32(cmd.ExecuteScalar()) == 0)
                    {
                        return "CRITICAL: TICKET table does not exist.";
                    }
                }

                // Check for USER_ID column
                if (!HasColumn(con, "TICKET", "USER_ID"))
                {
                    log += "USER_ID column missing. Attempting to add... ";
                    ExecuteNonQuery(con, "ALTER TABLE TICKET ADD USER_ID NUMBER REFERENCES USERS(USER_ID) ON DELETE CASCADE", ref log);
                }

                // Check for SHOW_ID column
                if (!HasColumn(con, "TICKET", "SHOW_ID"))
                {
                    log += "SHOW_ID column missing. Attempting to add... ";
                    ExecuteNonQuery(con, "ALTER TABLE TICKET ADD SHOW_ID NUMBER REFERENCES SHOWTIMES(SHOW_ID) ON DELETE CASCADE", ref log);
                }

                // Check for BOOKING_DATE column
                if (!HasColumn(con, "TICKET", "BOOKING_DATE"))
                {
                    log += "BOOKING_DATE column missing. Attempting to add... ";
                    ExecuteNonQuery(con, "ALTER TABLE TICKET ADD BOOKING_DATE DATE DEFAULT SYSDATE", ref log);
                }

                // Check for SEAT_NUMBER column
                if (!HasColumn(con, "TICKET", "SEAT_NUMBER"))
                {
                    log += "SEAT_NUMBER column missing. Attempting to add... ";
                    ExecuteNonQuery(con, "ALTER TABLE TICKET ADD SEAT_NUMBER VARCHAR2(10)", ref log);
                }

                // Check for STATUS column
                if (!HasColumn(con, "TICKET", "STATUS"))
                {
                    log += "STATUS column missing. Attempting to add... ";
                    ExecuteNonQuery(con, "ALTER TABLE TICKET ADD STATUS VARCHAR2(20) DEFAULT 'Paid'", ref log);
                }

                // NEW: Handle phantom PURCHASE_DATE column that's blocking inserts
                if (HasColumn(con, "TICKET", "PURCHASE_DATE"))
                {
                    log += "PURCHASE_DATE phantom column found. Removing to fix ORA-01400... ";
                    ExecuteNonQuery(con, "ALTER TABLE TICKET DROP COLUMN PURCHASE_DATE", ref log);
                }

                // NEW: Handle phantom SEAT_NUM column that's blocking inserts
                if (HasColumn(con, "TICKET", "SEAT_NUM"))
                {
                    log += "SEAT_NUM phantom column found. Removing... ";
                    ExecuteNonQuery(con, "ALTER TABLE TICKET DROP COLUMN SEAT_NUM", ref log);
                }

                // NEW: Handle phantom TICKET_PRICE column that's blocking inserts
                if (HasColumn(con, "TICKET", "TICKET_PRICE"))
                {
                    log += "TICKET_PRICE phantom column found. Removing... ";
                    ExecuteNonQuery(con, "ALTER TABLE TICKET DROP COLUMN TICKET_PRICE", ref log);
                }

                // Verification: List all final columns
                string cols = "";
                using (OracleCommand vCmd = new OracleCommand("SELECT column_name FROM user_tab_cols WHERE table_name = 'TICKET' ORDER BY column_id", con))
                {
                    using (OracleDataReader dr = vCmd.ExecuteReader())
                    {
                        while (dr.Read()) cols += dr[0].ToString() + " ";
                    }
                }
                log += "<br/>Final TICKET columns: <b>" + cols + "</b>";
            }
            return log.Trim();
        }

        private bool HasColumn(OracleConnection con, string tableName, string columnName)
        {
            string query = "SELECT COUNT(*) FROM user_tab_cols WHERE table_name = :tab AND column_name = :col";
            using (OracleCommand cmd = new OracleCommand(query, con))
            {
                cmd.Parameters.Add(":tab", tableName.ToUpper());
                cmd.Parameters.Add(":col", columnName.ToUpper());
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        private void ExecuteNonQuery(OracleConnection con, string sql, ref string log)
        {
            try {
                using (OracleCommand cmd = new OracleCommand(sql, con))
                {
                    cmd.ExecuteNonQuery();
                    log += "Success! ";
                }
            } catch (Exception ex) {
                log += "FAILED: " + ex.Message + " ";
            }
        }

        private void LoadUsers()
        {
            using (OracleConnection con = new OracleConnection(connStr))
            {
                string query = "SELECT USER_ID, USERNAME FROM USERS ORDER BY USERNAME";
                OracleDataAdapter da = new OracleDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                ddlUsers.DataSource = dt;
                ddlUsers.DataTextField = "USERNAME";
                ddlUsers.DataValueField = "USER_ID";
                ddlUsers.DataBind();
                ddlUsers.Items.Insert(0, new ListItem("-- Select User --", ""));
            }
        }

        protected void btnViewReport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlUsers.SelectedValue)) return;

            int userId = Convert.ToInt32(ddlUsers.SelectedValue);
            ShowUserDetails(userId);
            LoadTicketHistory(userId);
            pnlReport.Visible = true;
        }

        private void ShowUserDetails(int userId)
        {
            using (OracleConnection con = new OracleConnection(connStr))
            {
                string query = "SELECT * FROM USERS WHERE USER_ID = :Id";
                using (OracleCommand cmd = new OracleCommand(query, con))
                {
                    cmd.BindByName = true;
                    cmd.Parameters.Add(":Id", userId);
                    con.Open();
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            litUsername.Text = dr["USERNAME"].ToString();
                            litEmail.Text = dr["EMAIL"].ToString();
                            litPhone.Text = dr["PHONE_NUMBER"].ToString();
                        }
                    }
                }
            }
        }

        private void LoadTicketHistory(int userId)
        {
            using (OracleConnection con = new OracleConnection(connStr))
            {
                // Query for tickets in the last 6 months
                string query = @"SELECT tk.TICKET_ID, m.MOVIE_TITLE, th.THEATER_NAME, 
                                 TO_CHAR(s.SHOW_TIME, 'YYYY-MM-DD HH24:MI') as SHOW_TIME, 
                                 tk.SEAT_NUMBER, tk.BOOKING_DATE, tk.STATUS 
                                 FROM TICKET tk
                                 JOIN SHOWTIMES s ON tk.SHOW_ID = s.SHOW_ID
                                 JOIN MOVIE m ON s.MOVIE_ID = m.MOVIE_ID
                                 JOIN THEATERCITYHALL th ON s.THEATER_ID = th.THEATER_ID
                                 WHERE tk.USER_ID = :UserId 
                                 AND tk.BOOKING_DATE >= ADD_MONTHS(SYSDATE, -6)
                                 ORDER BY tk.BOOKING_DATE DESC";
                
                OracleDataAdapter da = new OracleDataAdapter(query, con);
                da.SelectCommand.BindByName = true;
                da.SelectCommand.Parameters.Add(":UserId", userId);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvTickets.DataSource = dt;
                gvTickets.DataBind();
            }
        }
    }
}
