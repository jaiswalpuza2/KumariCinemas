using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using System.Web.UI.WebControls;

namespace MovieTicketSystem
{
    public partial class ManageUsers : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["CinemaDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "Admin")
            {
                Response.Redirect("Login.aspx");
            }

            try { EnsureSchema(); } catch { }

            if (!IsPostBack)
            {
                LoadUsers();
            }
        }

        private void EnsureSchema()
        {
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

        private void LoadUsers()
        {
            using (OracleConnection con = new OracleConnection(connStr))
            {
                string query = "SELECT * FROM USERS ORDER BY USER_ID DESC";
                OracleDataAdapter da = new OracleDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvUsers.DataSource = dt;
                gvUsers.DataBind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            using (OracleConnection con = new OracleConnection(connStr))
            {
                string query;
                if (string.IsNullOrEmpty(hfUserId.Value))
                {
                   query = "INSERT INTO USERS (USERNAME, USER_PASSWORD, EMAIL, PHONE_NUMBER, ADDRESS, USER_ROLE) VALUES (:Username, :Password, :Email, :Phone, :Address, :Role)";
                }
                else
                {
                    query = "UPDATE USERS SET USERNAME = :Username, USER_PASSWORD = :Password, EMAIL = :Email, PHONE_NUMBER = :Phone, ADDRESS = :Address, USER_ROLE = :Role WHERE USER_ID = :Id";
                }

                using (OracleCommand cmd = new OracleCommand(query, con))
                {
                    cmd.BindByName = true;
                    cmd.Parameters.Add(":Username", txtUsername.Text);
                    cmd.Parameters.Add(":Email", txtEmail.Text);
                    cmd.Parameters.Add(":Password", txtPassword.Text);
                    cmd.Parameters.Add(":Phone", txtPhone.Text);
                    cmd.Parameters.Add(":Address", txtAddress.Text);
                    cmd.Parameters.Add(":Role", ddlRole.SelectedValue);
                    
                    if (!string.IsNullOrEmpty(hfUserId.Value))
                    {
                        cmd.Parameters.Add(":Id", hfUserId.Value);
                    }

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            ClearForm();
            LoadUsers();
            lblMessage.Text = "User saved successfully!";
        }

        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int userId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditUser")
            {
                using (OracleConnection con = new OracleConnection(connStr))
                {
                    string query = "SELECT * FROM USERS WHERE USER_ID = :Id";
                    using (OracleCommand cmd = new OracleCommand(query, con))
                    {
                        cmd.Parameters.Add(":Id", userId);
                        con.Open();
                        using (OracleDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                hfUserId.Value = dr["USER_ID"].ToString();
                                txtUsername.Text = dr["USERNAME"].ToString();
                                txtEmail.Text = dr["EMAIL"].ToString();
                                txtPassword.Text = dr["USER_PASSWORD"].ToString(); // Fix field name if needed
                                txtPhone.Text = dr["PHONE_NUMBER"].ToString();
                                txtAddress.Text = dr["ADDRESS"].ToString();
                                ddlRole.SelectedValue = dr["USER_ROLE"].ToString();
                                btnSave.Text = "Update User";
                            }
                        }
                    }
                }
            }
            else if (e.CommandName == "DeleteUser")
            {
                using (OracleConnection con = new OracleConnection(connStr))
                {
                    string query = "DELETE FROM USERS WHERE USER_ID = :Id";
                    using (OracleCommand cmd = new OracleCommand(query, con))
                    {
                        cmd.Parameters.Add(":Id", userId);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                LoadUsers();
                lblMessage.Text = "User deleted!";
            }
        }

        private void ClearForm()
        {
            hfUserId.Value = "";
            txtUsername.Text = "";
            txtEmail.Text = "";
            txtPassword.Text = "";
            txtPhone.Text = "";
            txtAddress.Text = "";
            btnSave.Text = "Save User";
        }
    }
}
