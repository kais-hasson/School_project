using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.Security;
using System.Web.Configuration;
using System.Xml.Linq;
using System.Text;

namespace School_project.Controllers
{
    public partial class Admin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Login_Button_Click(object sender, EventArgs e)
        {
        
            string dol;
            dol = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\hp\Desktop\New folder\School_project\App_Data\Database1.mdf; Integrated Security = True";
            SqlConnection to_data = new SqlConnection();
            to_data.ConnectionString = dol;
            string sql;
            SqlCommand my_select = new SqlCommand();     
            my_select.Connection = to_data;
            sql = "SELECT * FROM USERS ";
            my_select.CommandText = sql;
            SqlDataReader DReader;
            to_data.Open();
            DReader = my_select.ExecuteReader();
            string h = this.user_name.Text;
            string pass = this.password.Text;
            int f;
            string g;
            string pas;
            while (DReader.Read())
            {
                f = DReader.GetOrdinal("EMAIL");
                g = DReader.GetString(f);
                f = DReader.GetOrdinal("PASSWORD");
                pas = DReader.GetString(f);
                if (h == g && pass == pas)
                {
                    Response.Write("you are not an admin");
                     Response.Redirect("Download_page.aspx");
                }
                else
                    Response.Write("wellcom");
                Response.Redirect("Admin.aspx");
            }
            DReader.Close();
            to_data.Close();
        }
    }
}




        



    
