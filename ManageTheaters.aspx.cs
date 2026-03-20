using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using System.Web.UI.WebControls;

namespace MovieTicketSystem
{
    public partial class ManageTheaters : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["CinemaDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "Admin")
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                LoadTheaters();
            }
        }

        private void LoadTheaters()
        {
            using (OracleConnection con = new OracleConnection(connStr))
            {
                string query = "SELECT * FROM THEATERCITYHALL ORDER BY THEATER_ID DESC";
                OracleDataAdapter da = new OracleDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvTheaters.DataSource = dt;
                gvTheaters.DataBind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            using (OracleConnection con = new OracleConnection(connStr))
            {
                string query;
                if (string.IsNullOrEmpty(hfTheaterId.Value))
                {
                    query = "INSERT INTO THEATERCITYHALL (THEATER_NAME, LOCATION, CAPACITY) VALUES (:Name, :Location, :Capacity)";
                }
                else
                {
                    query = "UPDATE THEATERCITYHALL SET THEATER_NAME = :Name, LOCATION = :Location, CAPACITY = :Capacity WHERE THEATER_ID = :Id";
                }

                using (OracleCommand cmd = new OracleCommand(query, con))
                {
                    cmd.Parameters.Add(":Name", txtTheaterName.Text);
                    cmd.Parameters.Add(":Location", txtLocation.Text);
                    cmd.Parameters.Add(":Capacity", Convert.ToInt32(txtCapacity.Text));
                    
                    if (!string.IsNullOrEmpty(hfTheaterId.Value))
                    {
                        cmd.Parameters.Add(":Id", hfTheaterId.Value);
                    }

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            ClearForm();
            LoadTheaters();
            lblMessage.Text = "Theater saved successfully!";
        }

        protected void gvTheaters_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int theaterId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditTheater")
            {
                using (OracleConnection con = new OracleConnection(connStr))
                {
                    string query = "SELECT * FROM THEATERCITYHALL WHERE THEATER_ID = :Id";
                    using (OracleCommand cmd = new OracleCommand(query, con))
                    {
                        cmd.Parameters.Add(":Id", theaterId);
                        con.Open();
                        using (OracleDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                hfTheaterId.Value = dr["THEATER_ID"].ToString();
                                txtTheaterName.Text = dr["THEATER_NAME"].ToString();
                                txtLocation.Text = dr["LOCATION"].ToString();
                                txtCapacity.Text = dr["CAPACITY"].ToString();
                                btnSave.Text = "Update Theater";
                            }
                        }
                    }
                }
            }
            else if (e.CommandName == "DeleteTheater")
            {
                using (OracleConnection con = new OracleConnection(connStr))
                {
                    string query = "DELETE FROM THEATERCITYHALL WHERE THEATER_ID = :Id";
                    using (OracleCommand cmd = new OracleCommand(query, con))
                    {
                        cmd.Parameters.Add(":Id", theaterId);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                LoadTheaters();
                lblMessage.Text = "Theater deleted!";
            }
        }

        private void ClearForm()
        {
            hfTheaterId.Value = "";
            txtTheaterName.Text = "";
            txtLocation.Text = "";
            txtCapacity.Text = "";
            btnSave.Text = "Save Theater";
        }
    }
}
