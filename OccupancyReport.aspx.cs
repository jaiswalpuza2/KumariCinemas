using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using System.Web.UI.WebControls;

namespace MovieTicketSystem
{
    public partial class OccupancyReport : System.Web.UI.Page
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
                LoadMovies();
            }
        }

        private void LoadMovies()
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

        protected void btnAnalyze_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlMovies.SelectedValue)) return;

            int movieId = Convert.ToInt32(ddlMovies.SelectedValue);
            litMovieTitle.Text = ddlMovies.SelectedItem.Text;
            LoadOccupancyPerformance(movieId);
            pnlReport.Visible = true;
        }

        private void LoadOccupancyPerformance(int movieId)
        {
            using (OracleConnection con = new OracleConnection(connStr))
            {
                // Complex query to get top 3 theater occupancy for a movie
                // Aggregates paid tickets across all showtimes of that movie in each theater
                string query = @"SELECT * FROM (
                                    SELECT th.THEATER_NAME, th.CAPACITY, COUNT(tk.TICKET_ID) as PAID_TICKETS,
                                    CASE 
                                        WHEN th.CAPACITY > 0 THEN (COUNT(tk.TICKET_ID) / (th.CAPACITY * (SELECT COUNT(*) FROM SHOWTIMES s2 WHERE s2.THEATER_ID = th.THEATER_ID AND s2.MOVIE_ID = :MovieId))) * 100 
                                        ELSE 0 
                                    END as OCCUPANCY_PERCENT
                                    FROM THEATERCITYHALL th
                                    JOIN SHOWTIMES s ON th.THEATER_ID = s.THEATER_ID
                                    LEFT JOIN TICKET tk ON s.SHOW_ID = tk.SHOW_ID AND tk.STATUS = 'Paid'
                                    WHERE s.MOVIE_ID = :MovieId
                                    GROUP BY th.THEATER_NAME, th.CAPACITY, th.THEATER_ID
                                    ORDER BY OCCUPANCY_PERCENT DESC
                                 ) WHERE ROWNUM <= 3";
                
                // Note: The calculation above assumes occupancy is (total paid tickets) / (capacity * number of shows)
                
                OracleDataAdapter da = new OracleDataAdapter(query, con);
                da.SelectCommand.Parameters.Add(":MovieId", movieId);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvOccupancy.DataSource = dt;
                gvOccupancy.DataBind();
            }
        }
    }
}
