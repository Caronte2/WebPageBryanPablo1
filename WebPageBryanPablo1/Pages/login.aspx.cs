using System;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;


namespace WebPageBryanPablo1.Pages
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string user, password;

            user = txtUserID.Text.Trim();
            password = txtPassword.Text;

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
            {
                lblMessage.Text = "All fields are required.";
                return;
            }

            try
            {
                string DBpath = Server.MapPath("~/bbdd/healthcentre.db");

                using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + DBpath + ";Version=3;"))
                {
                    conn.Open();

                    string query = "SELECT UserId, PasswordHash, RoleId FROM Users WHERE DNI = @DNI";

                    using (SQLiteCommand command = new SQLiteCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@DNI", user);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int roleId = Convert.ToInt32(reader["RoleId"]);
                                string passwordHash = reader["PasswordHash"].ToString();

                                if (GetMD5(password) == passwordHash)
                                {
                                    Session["UserId"] = reader["UserId"].ToString();
                                    Session["RoleId"] = roleId;

                                    switch (roleId)
                                    {
                                        case 1:
                                            Response.Redirect("administrator.aspx");
                                            break;

                                        case 2:
                                            Response.Redirect("doctor.aspx");
                                            break;

                                        case 3:
                                            Response.Redirect("patient.aspx");
                                            break;

                                        default:
                                            lblMessage.Text = "Invalid user role";
                                            break;
                                    }
                                }
                                else
                                {
                                    lblMessage.Text = "Incorrect DNI or password";
                                }
                            }
                            else
                            {
                                lblMessage.Text = "Incorrect DNI or password";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error al conectar con la base de datos";
            }
        }
        private string GetMD5(string password)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(data).Replace("-", "");
            }
        }
    }
}