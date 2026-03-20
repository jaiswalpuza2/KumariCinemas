using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using System.Web.UI.WebControls;

namespace MovieTicketSystem
{
    public partial class ManageShowtimes : System.Web.UI.Page
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
                LoadShowtimes();
                LoadMoviesDDL();
                LoadTheatersDDL();
            }
        }

        private void LoadMoviesDDL()
        {
            using (OracleConnection con = new OracleConnection(connStr))
            {
                string query = "SELECT MOVIE_ID, MOVIE_TITLE FROM MOVIE ORDER BY MOVIE_TITLE";
                OracleDataAdapter da = new OracleDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                ddlMovies.DataSource = dt;
                ddlMovies.DataTextField = "MOVIE_TITLE";
                ddlMovies.DataValueField = "MOVIE_ID";
                ddlMovies.DataBind();
                ddlMovies.Items.Insert(0, new ListItem("-- Select Movie --", ""));
            }
        }

        private void LoadTheatersDDL()
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

        private void LoadShowtimes()
        {
            using (OracleConnection con = new OracleConnection(connStr))
            {
                string query = @"SELECT s.SHOW_ID, m.MOVIE_TITLE, t.THEATER_NAME, s.SHOW_TIME, s.PRICE 
                                 FROM SHOWTIMES s 
                                 JOIN MOVIE m ON s.MOVIE_ID = m.MOVIE_ID 
                                 JOIN THEATERCITYHALL t ON s.THEATER_ID = t.THEATER_ID 
                                 ORDER BY s.SHOW_TIME DESC";
                OracleDataAdapter da = new OracleDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvShowtimes.DataSource = dt;
                gvShowtimes.DataBind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            using (OracleConnection con = new OracleConnection(connStr))
            {
                string query;
                if (string.IsNullOrEmpty(hfShowId.Value))
                {
                    query = "INSERT INTO SHOWTIMES (MOVIE_ID, THEATER_ID, SHOW_TIME, PRICE) VALUES (:MovieId, :TheaterId, :ShowTime, :Price)";
                }
                else
                {
                    query = "UPDATE SHOWTIMES SET MOVIE_ID = :MovieId, THEATER_ID = :TheaterId, SHOW_TIME = :ShowTime, PRICE = :Price WHERE SHOW_ID = :Id";
                }

                using (OracleCommand cmd = new OracleCommand(query, con))
                {
                    cmd.BindByName = true;
                    cmd.Parameters.Add(":MovieId", ddlMovies.SelectedValue);
                    cmd.Parameters.Add(":TheaterId", ddlTheaters.SelectedValue);
                    
                    DateTime showTime;
                    if (!DateTime.TryParse(txtShowTime.Text, out showTime)) showTime = DateTime.Now;
                    cmd.Parameters.Add(":ShowTime", OracleDbType.TimeStamp).Value = showTime;
                    
                    decimal price = 0;
                    decimal.TryParse(txtPrice.Text, out price);
                    cmd.Parameters.Add(":Price", price);
                    
                    if (!string.IsNullOrEmpty(hfShowId.Value))
                    {
                        cmd.Parameters.Add(":Id", hfShowId.Value);
                    }

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            ClearForm();
            LoadShowtimes();
            lblMessage.Text = "Showtime saved successfully!";
        }

        protected void gvShowtimes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int showId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditShow")
            {
                using (OracleConnection con = new OracleConnection(connStr))
                {
                    string query = "SELECT * FROM SHOWTIMES WHERE SHOW_ID = :Id";
                    using (OracleCommand cmd = new OracleCommand(query, con))
                    {
                        cmd.Parameters.Add(":Id", showId);
                        con.Open();
                        using (OracleDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                hfShowId.Value = dr["SHOW_ID"].ToString();
                                ddlMovies.SelectedValue = dr["MOVIE_ID"].ToString();
                                ddlTheaters.SelectedValue = dr["THEATER_ID"].ToString();
                                // Format timestamp for datetime-local input
                                DateTime dt = Convert.ToDateTime(dr["SHOW_TIME"]);
                                txtShowTime.Text = dt.ToString("yyyy-MM-ddTHH:mm");
                                txtPrice.Text = dr["PRICE"].ToString();
                                btnSave.Text = "Update Showtime";
                            }
                        }
                    }
                }
            }
            else if (e.CommandName == "DeleteShow")
            {
                using (OracleConnection con = new OracleConnection(connStr))
                {
                    string query = "DELETE FROM SHOWTIMES WHERE SHOW_ID = :Id";
                    using (OracleCommand cmd = new OracleCommand(query, con))
                    {
                        cmd.Parameters.Add(":Id", showId);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                LoadShowtimes();
                lblMessage.Text = "Showtime deleted!";
            }
        }

        private void ClearForm()
        {
            hfShowId.Value = "";
            ddlMovies.SelectedIndex = 0;
            ddlTheaters.SelectedIndex = 0;
            txtShowTime.Text = "";
            txtPrice.Text = "";
            btnSave.Text = "Save Showtime";
        }
    }
}
