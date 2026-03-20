using System;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace MovieTicketSystem
{
    public partial class Default : System.Web.UI.Page
    {
        public string TotalMovies { get; set; } = "0";
        public string ActiveTheaters { get; set; } = "0";
        public string TotalBookings { get; set; } = "0";
        public string OccupancyRate { get; set; } = "0%";
        public string PopularMovie { get; set; } = "N/A";
        public string TodaySales { get; set; } = "$0";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserRole"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                LoadDashboardStatistics();
            }
        }

        private void LoadDashboardStatistics()
        {
            string connStr = ConfigurationManager.ConnectionStrings["CinemaDB"].ConnectionString;
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                try
                {
                    conn.Open();

                    // Get Total Movies
                    using (OracleCommand cmd = new OracleCommand("SELECT COUNT(*) FROM MOVIE", conn))
                    {
                        TotalMovies = cmd.ExecuteScalar()?.ToString() ?? "0";
                    }

                    // Get Active Theaters
                    using (OracleCommand cmd = new OracleCommand("SELECT COUNT(*) FROM THEATERCITYHALL", conn))
                    {
                        ActiveTheaters = cmd.ExecuteScalar()?.ToString() ?? "0";
                    }

                    // Get Total Bookings
                    using (OracleCommand cmd = new OracleCommand("SELECT COUNT(*) FROM TICKET", conn))
                    {
                        TotalBookings = cmd.ExecuteScalar()?.ToString() ?? "0";
                    }

                    // Calculate Occupancy Rate 
                    using (OracleCommand cmd = new OracleCommand(@"
                        SELECT ROUND(NVL(AVG(occ), 0), 1) 
                        FROM (
                            SELECT (COUNT(t.TICKET_ID) / NULLIF(th.CAPACITY, 0)) * 100 as occ
                            FROM SHOWTIMES s
                            LEFT JOIN TICKET t ON s.SHOW_ID = t.SHOW_ID
                            JOIN THEATERCITYHALL th ON s.THEATER_ID = th.THEATER_ID
                            GROUP BY s.SHOW_ID, th.CAPACITY
                        )", conn))
                    {
                        OccupancyRate = (cmd.ExecuteScalar()?.ToString() ?? "0") + "%";
                    }

                    // Get Most Popular Movie
                    using (OracleCommand cmd = new OracleCommand(@"
                        SELECT m.MOVIE_TITLE 
                        FROM MOVIE m 
                        JOIN SHOWTIMES s ON m.MOVIE_ID = s.MOVIE_ID 
                        JOIN TICKET t ON s.SHOW_ID = t.SHOW_ID 
                        GROUP BY m.MOVIE_TITLE 
                        ORDER BY COUNT(t.TICKET_ID) DESC 
                        FETCH FIRST 1 ROWS ONLY", conn))
                    {
                        PopularMovie = cmd.ExecuteScalar()?.ToString() ?? "None";
                    }

                    // Get Today's Sales
                    using (OracleCommand cmd = new OracleCommand(@"
                        SELECT NVL(SUM(s.PRICE), 0) 
                        FROM TICKET t 
                        JOIN SHOWTIMES s ON t.SHOW_ID = s.SHOW_ID 
                        WHERE t.BOOKING_DATE >= TRUNC(SYSDATE) AND t.STATUS = 'Paid'", conn))
                    {
                        TodaySales = "$" + cmd.ExecuteScalar()?.ToString();
                    }
                }
                catch (Exception)
                {
                    // Fallback for empty database
                    TotalMovies = "0";
                    ActiveTheaters = "0";
                    TotalBookings = "0";
                    OccupancyRate = "0%";
                }
            }
        }
    }
}
