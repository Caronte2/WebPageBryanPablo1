using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebPageBryanPablo1.Pages
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string DBpath = Server.MapPath("~/Bbdd/data.db");
            SQLiteConnection conn = new SQLiteConnection("Data Source=" + DBpath + ";Version=3;");
            conn.Open();

            string pin = Request.Form["Pin"];
            string pass = Request.Form["Password"];
            if (!string.IsNullOrEmpty(pin))
            {
                if (itemvalue == "2")
                {

                    string query = "SELECT PIN, Password from patients where PIN = @pin AND Password = @pass";
                    SQLiteCommand comm = new SQLiteCommand(query, conn);
                    comm.Parameters.Add(new SQLiteParameter("@pin", DbType.String) { Value = pin.Trim() });
                    comm.Parameters.Add(new SQLiteParameter("@pass", DbType.String) { Value = pass.Trim() });

                    SQLiteDataReader reader = comm.ExecuteReader();
                    bool res = reader.HasRows;
                    conn.Close();
                    if (res)
                    {
                        Session["Pin"] = pin;
                        Session["itemvalue"] = itemvalue;
                        Response.Redirect("./Users/Patient.aspx");

                    }
                    else
                    {
                        lblError.Text = "The PIN or the password are incorrect. Please, try again.";
                        lblError.Visible = true;
                        return;
                    }
                }
                else if (itemvalue == "1")
                {
                    string query = "SELECT PIN, Password from doctors where PIN = @pin AND Password = @pass";
                    SQLiteCommand comm = new SQLiteCommand(query, conn);
                    comm.Parameters.Add(new SQLiteParameter("@pin", DbType.String) { Value = pin.Trim() });
                    comm.Parameters.Add(new SQLiteParameter("@pass", DbType.String) { Value = pass.Trim() });

                    SQLiteDataReader reader = comm.ExecuteReader();

                    bool res = reader.HasRows;
                    conn.Close();
                    if (res)
                    {
                        Session["Pin"] = pin;
                        Session["itemvalue"] = itemvalue;
                        Response.Redirect("./Users/Doctor.aspx");

                    }
                    else
                    {
                        lblError.Text = "The PIN or the password are incorrect. Please, try again.";
                        lblError.Visible = true;
                        return;
                    }
                }
            }
            else
            {
                lblError.Text = "Please, enter a PIN.";
                lblError.Visible = true;
                return;
            }
        }
    }
}