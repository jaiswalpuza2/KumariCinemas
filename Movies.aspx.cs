using System;
using System.Configuration;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Web.UI.WebControls;

namespace MovieTicketSystem
{
    public partial class Movies : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadMovies();
            }
        }

        private void LoadMovies()
        {
            using (OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["CinemaDB"].ConnectionString))
            {
                string query = "SELECT * FROM MOVIE";

                OracleDataAdapter da = new OracleDataAdapter(query, con);
                DataTable dt = new DataTable();

                da.Fill(dt);

                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }
    }
}
