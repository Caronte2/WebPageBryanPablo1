
using System;
using System.Data;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebPageBryanPablo1.clases;

namespace WebPageBryanPablo1.Pages
{
    public partial class administrator : Page
    {
        private string DatabasePath
        {
            get { return Server.MapPath("~/bbdd/healthcentre.db"); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null ||
                Session["RoleId"] == null ||
                Convert.ToInt32(Session["RoleId"]) != 1)
            {
                Response.Redirect("login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadDoctors();
                LoadDoctorsGrid();
                LoadPatientsGrid();
                LoadStatistics();
            }
        }

        private SQLiteConnection GetConnection()
        {
            SQLiteConnection connection =
                new SQLiteConnection("Data Source=" + DatabasePath + ";Version=3;");

            connection.Open();

            using (SQLiteCommand command =
                new SQLiteCommand("PRAGMA foreign_keys = ON;", connection))
            {
                command.ExecuteNonQuery();
            }

            return connection;
        }

        private void LoadDoctors()
        {
            using (SQLiteConnection connection = GetConnection())
            {
                string query =
                    "SELECT DoctorId, FullName FROM Doctors ORDER BY FullName";

                using (SQLiteDataAdapter adapter =
                    new SQLiteDataAdapter(query, connection))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    ddlDoctor.Items.Clear();
                    ddlDoctor.DataSource = table;
                    ddlDoctor.DataTextField = "FullName";
                    ddlDoctor.DataValueField = "DoctorId";
                    ddlDoctor.DataBind();

                    ddlDoctor.Items.Insert(
                        0, new ListItem("Select a doctor", ""));
                }
            }
        }

        private void LoadDoctorsGrid()
        {
            using (SQLiteConnection connection = GetConnection())
            {
                string query = @"
                    SELECT
                        Doctors.DoctorId,
                        Users.UserId,
                        Users.DNI,
                        Doctors.FullName,
                        Doctors.Mobile
                    FROM Doctors
                    INNER JOIN Users ON Doctors.UserId = Users.UserId
                    ORDER BY Doctors.FullName";

                using (SQLiteDataAdapter adapter =
                    new SQLiteDataAdapter(query, connection))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    gvDoctors.DataSource = table;
                    gvDoctors.DataBind();
                }
            }
        }

        private void LoadPatientsGrid()
        {
            using (SQLiteConnection connection = GetConnection())
            {
                string query = @"
                    SELECT
                        Patients.PatientId,
                        Users.UserId,
                        Users.DNI,
                        Patients.FullName,
                        Patients.Mobile,
                        Patients.DateOfBirth,
                        COALESCE(Doctors.FullName, 'Not assigned') AS AssignedDoctor
                    FROM Patients
                    INNER JOIN Users ON Patients.UserId = Users.UserId
                    LEFT JOIN Doctors ON Patients.DoctorId = Doctors.DoctorId
                    ORDER BY Patients.FullName";

                using (SQLiteDataAdapter adapter =
                    new SQLiteDataAdapter(query, connection))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    gvPatients.DataSource = table;
                    gvPatients.DataBind();
                }
            }
        }

        private void LoadStatistics()
        {
            using (SQLiteConnection connection = GetConnection())
            {
                lblTotalDoctors.Text = GetCount(connection, "Doctors").ToString();
                lblTotalPatients.Text = GetCount(connection, "Patients").ToString();
            }
        }

        private int GetCount(SQLiteConnection connection, string tableName)
        {
            string query = "SELECT COUNT(*) FROM " + tableName;

            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        protected void btnCreateUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(hfEditingUserId.Value))
                {
                    UpdateUser();
                }
                else
                {
                    CreateUser();
                }
            }
            catch (SQLiteException ex)
            {
                if (ex.Message.Contains("UNIQUE constraint failed"))
                {
                    ShowMessage("This DNI is already registered.", true);
                }
                else
                {
                    ShowMessage("Database error: " + ex.Message, true);
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, true);
            }
        }

        private User ValidateForm(bool requirePassword)
        {
            if (string.IsNullOrWhiteSpace(ddlUserType.SelectedValue))
                throw new Exception("Please select a user type.");

            if (string.IsNullOrWhiteSpace(txtDNI.Text))
                throw new Exception("Please enter the DNI.");

            if (string.IsNullOrWhiteSpace(txtFullName.Text))
                throw new Exception("Please enter the full name.");

            if (string.IsNullOrWhiteSpace(txtMobile.Text))
                throw new Exception("Please enter the mobile number.");

            if (requirePassword && string.IsNullOrWhiteSpace(txtNewPassword.Text))
                throw new Exception("Please enter a password.");

            User user = new User();

            user.DNI = txtDNI.Text.Trim();
            user.FullName = txtFullName.Text.Trim();
            user.PhoneNumber = txtMobile.Text.Trim();

            if (!string.IsNullOrWhiteSpace(txtNewPassword.Text))
                user.Password = txtNewPassword.Text;

            if (ddlUserType.SelectedValue == "Patient")
            {
                if (string.IsNullOrWhiteSpace(txtAddress.Text))
                    throw new Exception("Please enter the patient's address.");

                if (string.IsNullOrWhiteSpace(txtDateOfBirth.Text))
                    throw new Exception("Please enter the patient's date of birth.");

                if (string.IsNullOrWhiteSpace(ddlDoctor.SelectedValue))
                    throw new Exception("Please select a doctor.");

                user.Address = txtAddress.Text.Trim();
            }

            return user;
        }

        private void CreateUser()
        {
            User user = ValidateForm(true);

            int roleId = ddlUserType.SelectedValue == "Doctor" ? 2 : 3;
            string passwordHash = HashPassword(user.Password);

            using (SQLiteConnection connection = GetConnection())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                string userQuery = @"
                    INSERT INTO Users (DNI, PasswordHash, RoleId)
                    VALUES (@DNI, @PasswordHash, @RoleId);
                    SELECT last_insert_rowid();";

                int userId;

                using (SQLiteCommand command =
                    new SQLiteCommand(userQuery, connection, transaction))
                {
                    command.Parameters.AddWithValue("@DNI", user.DNI);
                    command.Parameters.AddWithValue("@PasswordHash", passwordHash);
                    command.Parameters.AddWithValue("@RoleId", roleId);

                    userId = Convert.ToInt32(command.ExecuteScalar());
                }

                if (roleId == 2)
                {
                    string doctorQuery = @"
                        INSERT INTO Doctors (UserId, FullName, Mobile)
                        VALUES (@UserId, @FullName, @Mobile)";

                    using (SQLiteCommand command =
                        new SQLiteCommand(doctorQuery, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);
                        command.Parameters.AddWithValue("@FullName", user.FullName);
                        command.Parameters.AddWithValue("@Mobile", user.PhoneNumber);
                        command.ExecuteNonQuery();
                    }
                }
                else
                {
                    string patientQuery = @"
                        INSERT INTO Patients
                        (UserId, FullName, Mobile, Address, DateOfBirth, DoctorId)
                        VALUES
                        (@UserId, @FullName, @Mobile, @Address, @DateOfBirth, @DoctorId)";

                    using (SQLiteCommand command =
                        new SQLiteCommand(patientQuery, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);
                        command.Parameters.AddWithValue("@FullName", user.FullName);
                        command.Parameters.AddWithValue("@Mobile", user.PhoneNumber);
                        command.Parameters.AddWithValue("@Address", user.Address);
                        command.Parameters.AddWithValue("@DateOfBirth", txtDateOfBirth.Text);
                        command.Parameters.AddWithValue(
                            "@DoctorId", Convert.ToInt32(ddlDoctor.SelectedValue));

                        command.ExecuteNonQuery();
                    }
                }

                transaction.Commit();
            }

            FinishOperation();
            ShowMessage("User created successfully.", false);
        }

        private void UpdateUser()
        {
            User user = ValidateForm(false);

            int userId = Convert.ToInt32(hfEditingUserId.Value);
            int entityId = Convert.ToInt32(hfEditingEntityId.Value);
            int roleId = Convert.ToInt32(hfEditingRoleId.Value);

            using (SQLiteConnection connection = GetConnection())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                string userQuery;

                if (string.IsNullOrWhiteSpace(txtNewPassword.Text))
                {
                    userQuery =
                        "UPDATE Users SET DNI = @DNI WHERE UserId = @UserId";
                }
                else
                {
                    userQuery = @"
                        UPDATE Users
                        SET DNI = @DNI, PasswordHash = @PasswordHash
                        WHERE UserId = @UserId";
                }

                using (SQLiteCommand command =
                    new SQLiteCommand(userQuery, connection, transaction))
                {
                    command.Parameters.AddWithValue("@DNI", user.DNI);
                    command.Parameters.AddWithValue("@UserId", userId);

                    if (!string.IsNullOrWhiteSpace(txtNewPassword.Text))
                    {
                        command.Parameters.AddWithValue(
                            "@PasswordHash", HashPassword(user.Password));
                    }

                    command.ExecuteNonQuery();
                }

                if (roleId == 2)
                {
                    string doctorQuery = @"
                        UPDATE Doctors
                        SET FullName = @FullName, Mobile = @Mobile
                        WHERE DoctorId = @DoctorId";

                    using (SQLiteCommand command =
                        new SQLiteCommand(doctorQuery, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@FullName", user.FullName);
                        command.Parameters.AddWithValue("@Mobile", user.PhoneNumber);
                        command.Parameters.AddWithValue("@DoctorId", entityId);
                        command.ExecuteNonQuery();
                    }
                }
                else if (roleId == 3)
                {
                    string patientQuery = @"
                        UPDATE Patients
                        SET FullName = @FullName,
                            Mobile = @Mobile,
                            Address = @Address,
                            DateOfBirth = @DateOfBirth,
                            DoctorId = @DoctorId
                        WHERE PatientId = @PatientId";

                    using (SQLiteCommand command =
                        new SQLiteCommand(patientQuery, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@FullName", user.FullName);
                        command.Parameters.AddWithValue("@Mobile", user.PhoneNumber);
                        command.Parameters.AddWithValue("@Address", user.Address);
                        command.Parameters.AddWithValue("@DateOfBirth", txtDateOfBirth.Text);
                        command.Parameters.AddWithValue(
                            "@DoctorId", Convert.ToInt32(ddlDoctor.SelectedValue));
                        command.Parameters.AddWithValue("@PatientId", entityId);
                        command.ExecuteNonQuery();
                    }
                }

                transaction.Commit();
            }

            FinishOperation();
            ShowMessage("User updated successfully.", false);
        }

        protected void gvDoctors_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "EditDoctor" &&
                e.CommandName != "DeleteDoctor")
                return;

            int rowIndex = Convert.ToInt32(e.CommandArgument);

            int doctorId = Convert.ToInt32(
                gvDoctors.DataKeys[rowIndex].Values["DoctorId"]);

            int userId = Convert.ToInt32(
                gvDoctors.DataKeys[rowIndex].Values["UserId"]);

            try
            {
                if (e.CommandName == "EditDoctor")
                {
                    LoadDoctorForEditing(doctorId, userId);
                }
                else
                {
                    DeleteDoctor(doctorId, userId);
                }
            }
            catch (SQLiteException ex)
            {
                ShowMessage("Database error: " + ex.Message, true);
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, true);
            }
        }

        protected void gvPatients_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "EditPatient" &&
                e.CommandName != "DeletePatient")
                return;

            int rowIndex = Convert.ToInt32(e.CommandArgument);

            int patientId = Convert.ToInt32(
                gvPatients.DataKeys[rowIndex].Values["PatientId"]);

            int userId = Convert.ToInt32(
                gvPatients.DataKeys[rowIndex].Values["UserId"]);

            try
            {
                if (e.CommandName == "EditPatient")
                {
                    LoadPatientForEditing(patientId, userId);
                }
                else
                {
                    DeletePatient(patientId, userId);
                }
            }
            catch (SQLiteException ex)
            {
                ShowMessage("Database error: " + ex.Message, true);
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, true);
            }
        }

        private void LoadDoctorForEditing(int doctorId, int userId)
        {
            using (SQLiteConnection connection = GetConnection())
            {
                string query = @"
                    SELECT Users.DNI, Doctors.FullName, Doctors.Mobile
                    FROM Doctors
                    INNER JOIN Users ON Doctors.UserId = Users.UserId
                    WHERE Doctors.DoctorId = @DoctorId
                    AND Users.UserId = @UserId";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DoctorId", doctorId);
                    command.Parameters.AddWithValue("@UserId", userId);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            return;

                        ddlUserType.SelectedValue = "Doctor";
                        txtDNI.Text = reader["DNI"].ToString();
                        txtFullName.Text = reader["FullName"].ToString();
                        txtMobile.Text = reader["Mobile"].ToString();
                    }
                }
            }

            hfEditingUserId.Value = userId.ToString();
            hfEditingEntityId.Value = doctorId.ToString();
            hfEditingRoleId.Value = "2";

            SetEditMode();
        }

        private void LoadPatientForEditing(int patientId, int userId)
        {
            using (SQLiteConnection connection = GetConnection())
            {
                string query = @"
                    SELECT
                        Users.DNI,
                        Patients.FullName,
                        Patients.Mobile,
                        Patients.Address,
                        Patients.DateOfBirth,
                        Patients.DoctorId
                    FROM Patients
                    INNER JOIN Users ON Patients.UserId = Users.UserId
                    WHERE Patients.PatientId = @PatientId
                    AND Users.UserId = @UserId";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PatientId", patientId);
                    command.Parameters.AddWithValue("@UserId", userId);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            return;

                        string dni = reader["DNI"].ToString();
                        string fullName = reader["FullName"].ToString();
                        string mobile = reader["Mobile"].ToString();
                        string address = reader["Address"].ToString();
                        string dateOfBirth = reader["DateOfBirth"].ToString();
                        string doctorId = reader["DoctorId"] == DBNull.Value
                            ? ""
                            : reader["DoctorId"].ToString();

                        ddlUserType.SelectedValue = "Patient";
                        txtDNI.Text = dni;
                        txtFullName.Text = fullName;
                        txtMobile.Text = mobile;
                        txtAddress.Text = address;
                        txtDateOfBirth.Text = dateOfBirth;

                        LoadDoctors();

                        if (!string.IsNullOrEmpty(doctorId) &&
                            ddlDoctor.Items.FindByValue(doctorId) != null)
                        {
                            ddlDoctor.SelectedValue = doctorId;
                        }
                    }
                }
            }

            hfEditingUserId.Value = userId.ToString();
            hfEditingEntityId.Value = patientId.ToString();
            hfEditingRoleId.Value = "3";

            SetEditMode();
        }

        private void SetEditMode()
        {
            lblFormTitle.Text = "Edit User";
            btnCreateUser.Text = "Update User";
            btnCancelEdit.Visible = true;
            ddlUserType.Enabled = false;

            ClientScript.RegisterStartupScript(
                GetType(), "updateFields", "updateUserFields();", true);
        }

        private void DeleteDoctor(int doctorId, int userId)
        {
            using (SQLiteConnection connection = GetConnection())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                string checkQuery = @"
                    SELECT COUNT(*)
                    FROM MedicalRecords
                    WHERE DoctorId = @DoctorId";

                using (SQLiteCommand command =
                    new SQLiteCommand(checkQuery, connection, transaction))
                {
                    command.Parameters.AddWithValue("@DoctorId", doctorId);

                    int records = Convert.ToInt32(command.ExecuteScalar());

                    if (records > 0)
                    {
                        ShowMessage(
                            "This doctor cannot be deleted because medical records are associated with them.",
                            true);
                        return;
                    }
                }

                string deleteQuery =
                    "DELETE FROM Users WHERE UserId = @UserId";

                using (SQLiteCommand command =
                    new SQLiteCommand(deleteQuery, connection, transaction))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    if (command.ExecuteNonQuery() == 0)
                    {
                        ShowMessage("The doctor could not be found.", true);
                        return;
                    }
                }

                transaction.Commit();
            }

            FinishOperation();
            ShowMessage("Doctor deleted successfully.", false);
        }

        private void DeletePatient(int patientId, int userId)
        {
            using (SQLiteConnection connection = GetConnection())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                string checkQuery = @"
                    SELECT COUNT(*)
                    FROM MedicalRecords
                    WHERE PatientId = @PatientId";

                using (SQLiteCommand command =
                    new SQLiteCommand(checkQuery, connection, transaction))
                {
                    command.Parameters.AddWithValue("@PatientId", patientId);

                    int records = Convert.ToInt32(command.ExecuteScalar());

                    if (records > 0)
                    {
                        ShowMessage(
                            "This patient cannot be deleted because medical records are associated with them.",
                            true);
                        return;
                    }
                }

                string deleteQuery =
                    "DELETE FROM Users WHERE UserId = @UserId";

                using (SQLiteCommand command =
                    new SQLiteCommand(deleteQuery, connection, transaction))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    if (command.ExecuteNonQuery() == 0)
                    {
                        ShowMessage("The patient could not be found.", true);
                        return;
                    }
                }

                transaction.Commit();
            }

            FinishOperation();
            ShowMessage("Patient deleted successfully.", false);
        }

        protected void btnCancelEdit_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void FinishOperation()
        {
            LoadDoctors();
            LoadDoctorsGrid();
            LoadPatientsGrid();
            LoadStatistics();
            ClearForm();
        }

        private void ClearForm()
        {
            hfEditingUserId.Value = "";
            hfEditingEntityId.Value = "";
            hfEditingRoleId.Value = "";

            ddlUserType.Enabled = true;
            ddlUserType.SelectedIndex = 0;

            txtDNI.Text = "";
            txtFullName.Text = "";
            txtNewPassword.Text = "";
            txtMobile.Text = "";
            txtDateOfBirth.Text = "";
            txtAddress.Text = "";

            if (ddlDoctor.Items.Count > 0)
                ddlDoctor.SelectedIndex = 0;

            lblFormTitle.Text = "Create User";
            btnCreateUser.Text = "Create User";
            btnCancelEdit.Visible = false;

            ClientScript.RegisterStartupScript(
                GetType(), "resetFields", "updateUserFields();", true);
        }

        private string HashPassword(string password)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        private void ShowMessage(string message, bool isError)
        {
            lblAdminMessage.Text = message;
            lblAdminMessage.CssClass = isError
                ? "message-label text-danger"
                : "message-label text-success";
        }
    }
}
