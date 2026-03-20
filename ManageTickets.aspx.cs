using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using System.Web.UI.WebControls;

namespace MovieTicketSystem
{
    public partial class ManageTickets : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["CinemaDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Admin role check
            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "Admin")
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
                LoadUsersDDL();
                LoadShowtimesDDL();
                LoadTickets();
                if (!string.IsNullOrEmpty(diag)) {
                    lblMessage.Text = (string.IsNullOrEmpty(lblMessage.Text) ? "" : lblMessage.Text + "<br/>") + diag;
                }
            }
        }

        private string EnsureSchema()
        {
            string log = "";
            using (OracleConnection con = new OracleConnection(connStr))
            {
                con.Open();
                
                // Check if TICKET table exists list its columns
                string checkTableQuery = "SELECT COUNT(*) FROM user_tables WHERE table_name = 'TICKET'";
                using (OracleCommand cmd = new OracleCommand(checkTableQuery, con))
                {
                    if (Convert.ToInt32(cmd.ExecuteScalar()) == 0)
                    {
                        return "CRITICAL: TICKET table does not exist. Please run Setup.aspx.";
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

        private void LoadUsersDDL()
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

        private void LoadShowtimesDDL()
        {
            using (OracleConnection con = new OracleConnection(connStr))
            {
                string query = @"SELECT s.SHOW_ID, m.MOVIE_TITLE || ' (' || TO_CHAR(s.SHOW_TIME, 'YYYY-MM-DD HH24:MI') || ')' as ShowLabel 
                                 FROM SHOWTIMES s 
                                 JOIN MOVIE m ON s.MOVIE_ID = m.MOVIE_ID 
                                 ORDER BY s.SHOW_TIME DESC";
                OracleDataAdapter da = new OracleDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                ddlShowtimes.DataSource = dt;
                ddlShowtimes.DataTextField = "ShowLabel";
                ddlShowtimes.DataValueField = "SHOW_ID";
                ddlShowtimes.DataBind();
                ddlShowtimes.Items.Insert(0, new ListItem("-- Select Showtime --", ""));
            }
        }

        private void LoadTickets()
        {
            using (OracleConnection con = new OracleConnection(connStr))
            {
                string query = @"SELECT tk.TICKET_ID, u.USERNAME, m.MOVIE_TITLE, TO_CHAR(s.SHOW_TIME, 'YYYY-MM-DD HH24:MI') as SHOW_TIME, tk.SEAT_NUMBER, tk.STATUS 
                                 FROM TICKET tk
                                 JOIN USERS u ON tk.USER_ID = u.USER_ID
                                 JOIN SHOWTIMES s ON tk.SHOW_ID = s.SHOW_ID
                                 JOIN MOVIE m ON s.MOVIE_ID = m.MOVIE_ID
                                 ORDER BY tk.BOOKING_DATE DESC";
                try
                {
                    OracleDataAdapter da = new OracleDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gvTickets.DataSource = dt;
                    gvTickets.DataBind();
                }
                catch (OracleException ex)
                {
                    lblMessage.Text = "Database Error: " + ex.Message;
                    lblMessage.CssClass = "text-danger font-weight-bold";
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            using (OracleConnection con = new OracleConnection(connStr))
            {
                string query;
                if (string.IsNullOrEmpty(hfTicketId.Value))
                {
                    query = "INSERT INTO TICKET (USER_ID, SHOW_ID, SEAT_NUMBER, STATUS) VALUES (:UserId, :ShowId, :Seat, :Status)";
                }
                else
                {
                    query = "UPDATE TICKET SET USER_ID = :UserId, SHOW_ID = :ShowId, SEAT_NUMBER = :Seat, STATUS = :Status WHERE TICKET_ID = :Id";
                }

                using (OracleCommand cmd = new OracleCommand(query, con))
                {
                    cmd.Parameters.Add(":UserId", ddlUsers.SelectedValue);
                    cmd.Parameters.Add(":ShowId", ddlShowtimes.SelectedValue);
                    cmd.Parameters.Add(":Seat", txtSeat.Text);
                    cmd.Parameters.Add(":Status", ddlStatus.SelectedValue);
                    
                    if (!string.IsNullOrEmpty(hfTicketId.Value))
                    {
                        cmd.Parameters.Add(":Id", hfTicketId.Value);
                    }

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            ClearForm();
            LoadTickets();
            lblMessage.Text = "Ticket saved successfully!";
        }

        protected void gvTickets_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int ticketId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditTicket")
            {
                using (OracleConnection con = new OracleConnection(connStr))
                {
                    string query = "SELECT * FROM TICKET WHERE TICKET_ID = :Id";
                    using (OracleCommand cmd = new OracleCommand(query, con))
                    {
                        cmd.Parameters.Add(":Id", ticketId);
                        con.Open();
                        using (OracleDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                hfTicketId.Value = dr["TICKET_ID"].ToString();
                                ddlUsers.SelectedValue = dr["USER_ID"].ToString();
                                ddlShowtimes.SelectedValue = dr["SHOW_ID"].ToString();
                                txtSeat.Text = dr["SEAT_NUMBER"].ToString();
                                ddlStatus.SelectedValue = dr["STATUS"].ToString();
                                btnSave.Text = "Update Ticket";
                            }
                        }
                    }
                }
            }
            else if (e.CommandName == "DeleteTicket")
            {
                using (OracleConnection con = new OracleConnection(connStr))
                {
                    string query = "DELETE FROM TICKET WHERE TICKET_ID = :Id";
                    using (OracleCommand cmd = new OracleCommand(query, con))
                    {
                        cmd.Parameters.Add(":Id", ticketId);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                LoadTickets();
                lblMessage.Text = "Ticket deleted!";
            }
        }

        private void ClearForm()
        {
            hfTicketId.Value = "";
            ddlUsers.SelectedIndex = 0;
            ddlShowtimes.SelectedIndex = 0;
            txtSeat.Text = "";
            ddlStatus.SelectedIndex = 0;
            btnSave.Text = "Save Ticket";
        }
    }
}
