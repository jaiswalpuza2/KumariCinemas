using System;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;

namespace MovieTicketSystem
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try { EnsureSchema(); } catch { }
        }

        private void EnsureSchema()
        {
            string connStr = ConfigurationManager.ConnectionStrings["CinemaDB"].ConnectionString;
            using (OracleConnection con = new OracleConnection(connStr))
            {
                con.Open();
                string query = "SELECT COUNT(*) FROM user_tab_cols WHERE table_name = 'USERS' AND column_name = 'USER_ROLE'";
                using (OracleCommand cmd = new OracleCommand(query, con))
                {
                    if (Convert.ToInt32(cmd.ExecuteScalar()) == 0)
                    {
                        using (OracleCommand alterCmd = new OracleCommand("ALTER TABLE USERS ADD USER_ROLE VARCHAR2(20) DEFAULT 'Customer' CHECK (USER_ROLE IN ('Admin', 'Customer'))", con))
                        {
                            alterCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            using (OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["CinemaDB"].ConnectionString))
            {
                // Allow login with either Username or Email
                string query = "SELECT USER_ID, USERNAME, USER_ROLE FROM USERS WHERE (USERNAME = :Identifier OR EMAIL = :Identifier) AND USER_PASSWORD = :Password";

                using (OracleCommand cmd = new OracleCommand(query, con))
                {
                    cmd.BindByName = true;
                    cmd.Parameters.Add(":Identifier", txtEmail.Text.Trim());
                    cmd.Parameters.Add(":Password", txtPassword.Text.Trim());

                    con.Open();
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            // Store user details in session
                            Session["UserID"] = dr["USER_ID"].ToString();
                            Session["UserName"] = dr["USERNAME"].ToString();
                            Session["UserRole"] = dr["USER_ROLE"].ToString();
                            
                            Response.Redirect("Default.aspx");
                        }
                        else
                        {
                            lblMessage.Text = "Invalid Username/Email or Password";
                        }
                    }
                    con.Close();
                }
            }
        }
    }
}
