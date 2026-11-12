using System;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI.WebControls;
using WebPageBryanPablo1.clases;

namespace WebPageBryanPablo1.Pages
{
    public partial class administrator : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDoctors();
            }
        }

        private void LoadDoctors()
        {
            string DBpath = Server.MapPath("~/bbdd/healthcentre.db");

            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + DBpath + ";Version=3;"))
            {
                conn.Open();

                string query = "SELECT DoctorId, FullName FROM Doctors ORDER BY FullName";

                using (SQLiteCommand command = new SQLiteCommand(query, conn))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        ddlDoctor.DataSource = reader;
                        ddlDoctor.DataTextField = "FullName";
                        ddlDoctor.DataValueField = "DoctorId";
                        ddlDoctor.DataBind();
                    }
                }
            }

            ddlDoctor.Items.Insert(0, new ListItem("Select a doctor", ""));
        }

        protected void btnCreateUser_Click(object sender, EventArgs e)
        {
            User usuario = new User();

            try
            {
                if (ddlUserType.SelectedValue == "Doctor")
                {
                    usuario.RoleId = 2;
                }
                else if (ddlUserType.SelectedValue == "Patient")
                {
                    usuario.RoleId = 3;
                }
                else
                {
                    lblAdminMessage.Text = "Select a user type";
                    return;
                }

                usuario.FullName = txtFullName.Text.Trim();
                usuario.DNI = txtDNI.Text.Trim();
                usuario.Password = txtNewPassword.Text;
                usuario.PhoneNumber = txtMobile.Text.Trim();

                if (usuario.RoleId == 3)
                {
                    usuario.Address = txtAddress.Text.Trim();

                    if (string.IsNullOrWhiteSpace(txtDateOfBirth.Text))
                    {
                        lblAdminMessage.Text = "Date of birth is required";
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(ddlDoctor.SelectedValue))
                    {
                        lblAdminMessage.Text = "Please select a doctor";
                        return;
                    }
                }
            }
            catch (ArgumentException ex)
            {
                lblAdminMessage.Text = ex.Message;
                return;
            }

            try
            {
                string DBpath = Server.MapPath("~/bbdd/healthcentre.db");

                using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + DBpath + ";Version=3;"))
                {
                    conn.Open();

                    using (SQLiteTransaction transaction = conn.BeginTransaction())
                    {
                        string passwordHash;

                        using (MD5 md5 = MD5.Create())
                        {
                            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(usuario.Password));
                            passwordHash = BitConverter.ToString(data).Replace("-", "");
                        }

                        string query = "INSERT INTO Users (DNI, PasswordHash, RoleId) VALUES (@DNI, @PasswordHash, @RoleId); SELECT last_insert_rowid();";

                        int userId;

                        using (SQLiteCommand command = new SQLiteCommand(query, conn, transaction))
                        {
                            command.Parameters.AddWithValue("@DNI", usuario.DNI);
                            command.Parameters.AddWithValue("@PasswordHash", passwordHash);
                            command.Parameters.AddWithValue("@RoleId", usuario.RoleId);

                            userId = Convert.ToInt32(command.ExecuteScalar());
                        }

                        if (usuario.RoleId == 2)
                        {
                            query = "INSERT INTO Doctors (UserId, FullName, Mobile) VALUES (@UserId, @FullName, @Mobile)";
                        }
                        else
                        {
                            query = "INSERT INTO Patients (UserId, FullName, Mobile, Address, DateOfBirth, DoctorId) VALUES (@UserId, @FullName, @Mobile, @Address, @DateOfBirth, @DoctorId)";
                        }

                        using (SQLiteCommand command = new SQLiteCommand(query, conn, transaction))
                        {
                            command.Parameters.AddWithValue("@UserId", userId);
                            command.Parameters.AddWithValue("@FullName", usuario.FullName);
                            command.Parameters.AddWithValue("@Mobile", usuario.PhoneNumber);

                            if (usuario.RoleId == 3)
                            {
                                command.Parameters.AddWithValue("@Address", usuario.Address);
                                command.Parameters.AddWithValue("@DateOfBirth", txtDateOfBirth.Text);
                                command.Parameters.AddWithValue("@DoctorId", Convert.ToInt32(ddlDoctor.SelectedValue));
                            }

                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                }

                lblAdminMessage.Text = "User successfully created";

                LoadDoctors();

                txtDNI.Text = "";
                txtFullName.Text = "";
                txtNewPassword.Text = "";
                txtMobile.Text = "";
                txtAddress.Text = "";
                txtDateOfBirth.Text = "";
                ddlDoctor.SelectedIndex = 0;
                ddlUserType.SelectedIndex = 0;
            }
            catch (SQLiteException ex)
            {
                if (ex.Message.Contains("UNIQUE constraint failed"))
                {
                    lblAdminMessage.Text = "This DNI is already registered.";
                }
                else
                {
                    lblAdminMessage.Text = "Database error: " + ex.Message;
                }
            }
            catch (Exception ex)
            {
                lblAdminMessage.Text = "Error creating user: " + ex.Message;
            }
        }
    }
}
