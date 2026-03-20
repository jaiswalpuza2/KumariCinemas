using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using System.Web.UI.WebControls;

namespace MovieTicketSystem
{
    public partial class ManageMovies : System.Web.UI.Page
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
                string query = "SELECT * FROM MOVIE ORDER BY MOVIE_ID ASC";
                OracleDataAdapter da = new OracleDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvMovies.DataSource = dt;
                gvMovies.DataBind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            using (OracleConnection con = new OracleConnection(connStr))
            {
                string query;
                if (string.IsNullOrEmpty(hfMovieId.Value))
                {
                    query = "INSERT INTO MOVIE (MOVIE_TITLE, GENRE, LANGUAGE, DURATION) VALUES (:Title, :Genre, :Language, :Duration)";
                }
                else
                {
                    query = "UPDATE MOVIE SET MOVIE_TITLE = :Title, GENRE = :Genre, LANGUAGE = :Language, DURATION = :Duration WHERE MOVIE_ID = :Id";
                }

                using (OracleCommand cmd = new OracleCommand(query, con))
                {
                    cmd.BindByName = true;
                    cmd.Parameters.Add(":Title", txtTitle.Text);
                    cmd.Parameters.Add(":Genre", txtGenre.Text);
                    cmd.Parameters.Add(":Language", txtLanguage.Text);
                    
                    int duration = 0;
                    int.TryParse(txtDuration.Text, out duration);
                    cmd.Parameters.Add(":Duration", duration);
                    
                    if (!string.IsNullOrEmpty(hfMovieId.Value))
                    {
                        cmd.Parameters.Add(":Id", hfMovieId.Value);
                    }

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            ClearForm();
            LoadMovies();
            lblMessage.Text = "Movie saved successfully!";
        }

        protected void gvMovies_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int movieId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditMovie")
            {
                using (OracleConnection con = new OracleConnection(connStr))
                {
                    string query = "SELECT * FROM MOVIE WHERE MOVIE_ID = :Id";
                    using (OracleCommand cmd = new OracleCommand(query, con))
                    {
                        cmd.Parameters.Add(":Id", movieId);
                        con.Open();
                        using (OracleDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                hfMovieId.Value = dr["MOVIE_ID"].ToString();
                                txtTitle.Text = dr["MOVIE_TITLE"].ToString();
                                txtGenre.Text = dr["GENRE"].ToString();
                                txtLanguage.Text = dr["LANGUAGE"].ToString();
                                txtDuration.Text = dr["DURATION"].ToString();
                                btnSave.Text = "Update Movie";
                            }
                        }
                    }
                }
            }
            else if (e.CommandName == "DeleteMovie")
            {
                using (OracleConnection con = new OracleConnection(connStr))
                {
                    string query = "DELETE FROM MOVIE WHERE MOVIE_ID = :Id";
                    using (OracleCommand cmd = new OracleCommand(query, con))
                    {
                        cmd.Parameters.Add(":Id", movieId);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                LoadMovies();
                lblMessage.Text = "Movie deleted!";
            }
        }

        private void ClearForm()
        {
            hfMovieId.Value = "";
            txtTitle.Text = "";
            txtGenre.Text = "";
            txtLanguage.Text = "";
            txtDuration.Text = "";
            btnSave.Text = "Save Movie";
        }
    }
}
