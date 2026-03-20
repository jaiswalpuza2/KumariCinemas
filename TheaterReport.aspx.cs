using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using System.Web.UI.WebControls;

namespace MovieTicketSystem
{
    public partial class TheaterReport : System.Web.UI.Page
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
                string query = "SELECT THEATER_ID, THEATER_NAME FROM THEATERCITYHALL ORDER BY THEATER_NAME";
                OracleDataAdapter da = new OracleDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                ddlTheaters.DataSource = dt;
                ddlTheaters.DataTextField = "THEATER_NAME";
                ddlTheaters.DataValueField = "THEATER_ID";
                ddlTheaters.DataBind();
                ddlTheaters.Items.Insert(0, new ListItem("-- Select Theater --", ""));
            }
        }

        protected void btnViewReport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlTheaters.SelectedValue)) return;

            int theaterId = Convert.ToInt32(ddlTheaters.SelectedValue);
            ShowTheaterDetails(theaterId);
            LoadSchedule(theaterId);
            pnlReport.Visible = true;
        }

        private void ShowTheaterDetails(int theaterId)
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
                            litTheaterName.Text = dr["THEATER_NAME"].ToString();
                            litLocation.Text = dr["LOCATION"].ToString();
                            litCapacity.Text = dr["CAPACITY"].ToString();
                        }
                    }
                }
            }
        }

        private void LoadSchedule(int theaterId)
        {
            using (OracleConnection con = new OracleConnection(connStr))
            {
                string query = @"SELECT m.MOVIE_TITLE, m.GENRE, m.LANGUAGE, 
                                 TO_CHAR(s.SHOW_TIME, 'YYYY-MM-DD HH24:MI') as SHOW_TIME, s.PRICE 
                                 FROM SHOWTIMES s 
                                 JOIN MOVIE m ON s.MOVIE_ID = m.MOVIE_ID 
                                 WHERE s.THEATER_ID = :Id 
                                 ORDER BY s.SHOW_TIME ASC";
                
                OracleDataAdapter da = new OracleDataAdapter(query, con);
                da.SelectCommand.Parameters.Add(":Id", theaterId);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvSchedule.DataSource = dt;
                gvSchedule.DataBind();
            }
        }
    }
}
