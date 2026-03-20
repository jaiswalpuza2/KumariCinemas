using System;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;

namespace MovieTicketSystem
{
    public partial class AddMovie : System.Web.UI.Page
    {
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            using (OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["CinemaDB"].ConnectionString))
            {
                string query = "INSERT INTO Movie (Movie_Title, Duration, Language, Genre) VALUES (:Title, :Duration, :Language, :Genre)";

                using (OracleCommand cmd = new OracleCommand(query, con))
                {
                    cmd.Parameters.Add(":Title", txtTitle.Text);
                    cmd.Parameters.Add(":Duration", Convert.ToInt32(txtDuration.Text));
                    cmd.Parameters.Add(":Language", txtLanguage.Text);
                    cmd.Parameters.Add(":Genre", txtGenre.Text);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            lblMessage.Text = "Movie Added Successfully";
        }
    }
}
