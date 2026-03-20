using System;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;

namespace MovieTicketSystem
{
    public partial class Register : System.Web.UI.Page
    {
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            try { EnsureSchema(); } catch { }
            using (OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["CinemaDB"].ConnectionString))
            {
                con.Open();
                
                // Check if user already exists
                string checkQuery = "SELECT COUNT(*) FROM USERS WHERE USERNAME = :Username OR EMAIL = :Email";
                using (OracleCommand checkCmd = new OracleCommand(checkQuery, con))
                {
                    checkCmd.Parameters.Add(":Username", txtUsername.Text.Trim());
                    checkCmd.Parameters.Add(":Email", txtEmail.Text.Trim());
                    
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (count > 0)
                    {
                        lblMessage.Text = "Username or Email already exists!";
                        lblMessage.CssClass = "badge bg-danger bg-opacity-10 text-danger p-2 border border-danger border-opacity-25 w-100";
                        return;
                    }
                }

                string query = "INSERT INTO USERS (USERNAME, EMAIL, USER_PASSWORD, PHONE_NUMBER, ADDRESS, USER_ROLE) VALUES (:Username, :Email, :Password, :Phone, :Address, 'Customer')";

                using (OracleCommand cmd = new OracleCommand(query, con))
                {
                    cmd.Parameters.Add(":Username", txtUsername.Text.Trim());
                    cmd.Parameters.Add(":Email", txtEmail.Text.Trim());
                    cmd.Parameters.Add(":Password", txtPassword.Text.Trim());
                    cmd.Parameters.Add(":Phone", txtPhone.Text.Trim());
                    cmd.Parameters.Add(":Address", txtAddress.Text.Trim());

                    cmd.ExecuteNonQuery();
                }
            }

            lblMessage.Text = "Registration Successful! You can now login.";
            lblMessage.CssClass = "badge bg-success bg-opacity-10 text-success p-2 border border-success border-opacity-25 w-100";
            
            // Clear fields
            txtUsername.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            txtAddress.Text = "";
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
    }
}
