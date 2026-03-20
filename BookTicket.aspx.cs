using System;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;

namespace MovieTicketSystem
{
    public partial class BookTicket : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["CinemaDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                LoadShowtimes();
            }
        }

        private void LoadShowtimes()
        {
            using (OracleConnection con = new OracleConnection(connStr))
            {
                string query = @"SELECT s.SHOW_ID, m.MOVIE_TITLE || ' at ' || t.THEATER_NAME || ' (' || TO_CHAR(s.SHOW_TIME, 'YYYY-MM-DD HH24:MI') || ') - $' || s.PRICE AS DISPLAY_TEXT 
                                 FROM SHOWTIMES s 
                                 JOIN MOVIE m ON s.MOVIE_ID = m.MOVIE_ID 
                                 JOIN THEATERCITYHALL t ON s.THEATER_ID = t.THEATER_ID 
                                 WHERE s.SHOW_TIME > SYSDATE 
                                 ORDER BY s.SHOW_TIME ASC";
                OracleDataAdapter da = new OracleDataAdapter(query, con);
                System.Data.DataTable dt = new System.Data.DataTable();
                da.Fill(dt);
                ddlShowtime.DataSource = dt;
                ddlShowtime.DataTextField = "DISPLAY_TEXT";
                ddlShowtime.DataValueField = "SHOW_ID";
                ddlShowtime.DataBind();
            }
        }

        protected void btnBook_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlShowtime.SelectedValue))
            {
                lblMessage.Text = "Please select a valid showtime.";
                lblMessage.CssClass = "badge bg-danger bg-opacity-10 text-danger p-2 w-100 border border-danger border-opacity-20";
                return;
            }

            using (OracleConnection con = new OracleConnection(connStr))
            {
                string query = "INSERT INTO TICKET (USER_ID, SHOW_ID, SEAT_NUMBER, BOOKING_DATE, STATUS) VALUES (:UserId, :ShowId, :Seat, SYSDATE, 'Paid')";

                using (OracleCommand cmd = new OracleCommand(query, con))
                {
                    cmd.BindByName = true;
                    cmd.Parameters.Add(":UserId", Convert.ToInt32(Session["UserID"])); 
                    cmd.Parameters.Add(":ShowId", Convert.ToInt32(ddlShowtime.SelectedValue));
                    cmd.Parameters.Add(":Seat", txtSeat.Text);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            lblMessage.Text = "Ticket Booked Successfully";
        }
    }
}
