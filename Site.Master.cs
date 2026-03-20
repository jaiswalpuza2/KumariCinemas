using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MovieTicketSystem
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserRole"] != null)
                {
                    string role = Session["UserRole"].ToString();
                    litUserName.Text = Session["UserName"]?.ToString();
                    
                    phUserLinks.Visible = true;
                    phGuestLinks.Visible = false;

                    if (role == "Admin")
                    {
                        phAdminMenu.Visible = true;
                        phReportsMenu.Visible = true;
                        phAdminReportLinks.Visible = true;
                    }
                    else if (role == "Customer")
                    {
                        phReportsMenu.Visible = true;
                        phAdminReportLinks.Visible = false;
                    }
                }
                else
                {
                    phUserLinks.Visible = false;
                    phGuestLinks.Visible = true;
                    phAdminMenu.Visible = false;
                    phReportsMenu.Visible = false;
                }
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Login.aspx");
        }
    }
}
