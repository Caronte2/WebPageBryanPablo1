
using System;
using System.Data;
using System.Data.SQLite;
using System.Globalization;

namespace WebPageBryanPablo1.Pages
{
    public partial class doctor : System.Web.UI.Page
    {
        private string GetConnectionString()
        {
            string path = Server.MapPath("~/bbdd/healthcentre.db");

            return "Data Source=" + path + ";Version=3;";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null ||
                Session["RoleId"] == null ||
                Convert.ToInt32(Session["RoleId"]) != 2)
            {
                Response.Redirect("~/Pages/login.aspx");
                return;
            }

            // Mostrar la pestaña seleccionada en cada carga.
            ShowActiveTab();

            if (!IsPostBack)
            {
                using (SQLiteConnection conn =
                    new SQLiteConnection(GetConnectionString()))
                {
                    conn.Open();

                    int doctorId = GetDoctorId(conn);

                    if (doctorId == 0)
                    {
                        lblMessage.Text =
                            "No doctor profile was found for this account.";
                        return;
                    }

                    LoadPatients(conn, doctorId);
                    LoadAppointmentPatients(conn, doctorId);
                    LoadMedicalRecords(conn, doctorId);
                }
            }
        }

        

        protected void btnRecordsTab_Click(object sender, EventArgs e)
        {
            hfActiveTab.Value = "records";
            ShowActiveTab();
        }

        protected void btnAppointmentsTab_Click(object sender, EventArgs e)
        {
            hfActiveTab.Value = "appointments";
            ShowActiveTab();
        }

        private void ShowActiveTab()
        {
            bool showRecords = hfActiveTab.Value != "appointments";

            pnlRecords.Visible = showRecords;
            pnlAppointments.Visible = !showRecords;

            btnRecordsTab.CssClass = showRecords
                ? "tab-button active"
                : "tab-button";

            btnAppointmentsTab.CssClass = !showRecords
                ? "tab-button active"
                : "tab-button";
        }

        

        private int GetDoctorId(SQLiteConnection conn)
        {
            string query = @"
                SELECT DoctorId
                FROM Doctors
                WHERE UserId = @UserId";

            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(
                    "@UserId",
                    Convert.ToInt32(Session["UserId"])
                );

                object result = cmd.ExecuteScalar();

                if (result == null || result == DBNull.Value)
                    return 0;

                return Convert.ToInt32(result);
            }
        }

        

        private void LoadPatients(SQLiteConnection conn, int doctorId)
        {
            string query = @"
                SELECT PatientId, FullName
                FROM Patients
                WHERE DoctorId = @DoctorId
                ORDER BY FullName";

            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@DoctorId", doctorId);

                using (SQLiteDataAdapter adapter =
                    new SQLiteDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    ddlPatients.DataSource = dt;
                    ddlPatients.DataTextField = "FullName";
                    ddlPatients.DataValueField = "PatientId";
                    ddlPatients.DataBind();
                }
            }

            ddlPatients.Items.Insert(
                0,
                new System.Web.UI.WebControls.ListItem(
                    "-- Select a patient --", "0")
            );

            bool hasPatients = ddlPatients.Items.Count > 1;

            ddlPatients.Enabled = hasPatients;
            btnSave.Enabled = hasPatients;
        }

        

        private void LoadAppointmentPatients(
            SQLiteConnection conn, int doctorId)
        {
            string query = @"
                SELECT PatientId, FullName
                FROM Patients
                WHERE DoctorId = @DoctorId
                ORDER BY FullName";

            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@DoctorId", doctorId);

                using (SQLiteDataAdapter adapter =
                    new SQLiteDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    ddlAppointmentPatient.DataSource = dt;
                    ddlAppointmentPatient.DataTextField = "FullName";
                    ddlAppointmentPatient.DataValueField = "PatientId";
                    ddlAppointmentPatient.DataBind();
                }
            }

            ddlAppointmentPatient.Items.Insert(
                0,
                new System.Web.UI.WebControls.ListItem(
                    "-- Select a patient --", "0")
            );

            btnCreateAppointment.Enabled =
                ddlAppointmentPatient.Items.Count > 1;
        }

        

        private void LoadMedicalRecords(
            SQLiteConnection conn, int doctorId)
        {
            string query = @"
                SELECT
                    mr.MedicalRecordId,
                    p.FullName AS PatientName,
                    mr.AppointmentDate,
                    mr.Diagnosis,
                    mr.Treatment
                FROM MedicalRecords mr
                INNER JOIN Patients p
                    ON mr.PatientId = p.PatientId
                WHERE mr.DoctorId = @DoctorId
                  AND p.DoctorId = @DoctorId
                ORDER BY mr.AppointmentDate DESC";

            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@DoctorId", doctorId);

                using (SQLiteDataAdapter adapter =
                    new SQLiteDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    gvRecords.DataSource = dt;
                    gvRecords.DataBind();
                }
            }
        }

        

        protected void btnSave_Click(object sender, EventArgs e)
        {
            hfActiveTab.Value = "records";
            ShowActiveTab();

            SaveRecord(false);
        }

        

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            hfActiveTab.Value = "records";
            ShowActiveTab();

            SaveRecord(true);
        }

        private void SaveRecord(bool isUpdate)
        {
            int patientId;
            int recordId = 0;

            if (!int.TryParse(ddlPatients.SelectedValue, out patientId)
                || patientId <= 0)
            {
                lblMessage.Text = "Please select a patient.";
                return;
            }

            DateTime appointmentDate;

            if (!DateTime.TryParseExact(
                txtAppointmentDate.Text,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out appointmentDate))
            {
                lblMessage.Text = "Please enter a valid appointment date.";
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDiagnosis.Text) ||
                string.IsNullOrWhiteSpace(txtTreatment.Text))
            {
                lblMessage.Text =
                    "Diagnosis and treatment are required.";
                return;
            }

            if (isUpdate &&
                (!int.TryParse(hfRecordId.Value, out recordId)
                 || recordId <= 0))
            {
                lblMessage.Text = "Invalid medical record.";
                return;
            }

            using (SQLiteConnection conn =
                new SQLiteConnection(GetConnectionString()))
            {
                conn.Open();

                int doctorId = GetDoctorId(conn);

                if (doctorId == 0)
                    return;

                if (!isUpdate)
                {
                    string query = @"
                        INSERT INTO MedicalRecords
                        (PatientId, DoctorId, AppointmentDate,
                         Diagnosis, Treatment)
                        SELECT
                            PatientId, @DoctorId, @AppointmentDate,
                            @Diagnosis, @Treatment
                        FROM Patients
                        WHERE PatientId = @PatientId
                          AND DoctorId = @DoctorId";

                    using (SQLiteCommand cmd =
                        new SQLiteCommand(query, conn))
                    {
                        AddRecordParameters(
                            cmd, patientId, doctorId, appointmentDate);

                        int rows = cmd.ExecuteNonQuery();

                        lblMessage.Text = rows > 0
                            ? "Medical record created successfully."
                            : "You cannot add a record to this patient.";
                    }
                }
                else
                {
                    string query = @"
                        UPDATE MedicalRecords
                        SET PatientId = @PatientId,
                            AppointmentDate = @AppointmentDate,
                            Diagnosis = @Diagnosis,
                            Treatment = @Treatment
                        WHERE MedicalRecordId = @RecordId
                          AND DoctorId = @DoctorId
                          AND EXISTS (
                              SELECT 1
                              FROM Patients
                              WHERE Patients.PatientId = @PatientId
                                AND Patients.DoctorId = @DoctorId
                          )";

                    using (SQLiteCommand cmd =
                        new SQLiteCommand(query, conn))
                    {
                        AddRecordParameters(
                            cmd, patientId, doctorId, appointmentDate);

                        cmd.Parameters.AddWithValue(
                            "@RecordId", recordId);

                        int rows = cmd.ExecuteNonQuery();

                        lblMessage.Text = rows > 0
                            ? "Medical record updated successfully."
                            : "Record not found or access denied.";
                    }
                }

                ClearForm();

                LoadPatients(conn, doctorId);
                LoadMedicalRecords(conn, doctorId);
            }
        }

        private void AddRecordParameters(
            SQLiteCommand cmd,
            int patientId,
            int doctorId,
            DateTime appointmentDate)
        {
            cmd.Parameters.AddWithValue("@PatientId", patientId);
            cmd.Parameters.AddWithValue("@DoctorId", doctorId);

            cmd.Parameters.AddWithValue(
                "@AppointmentDate",
                appointmentDate.ToString(
                    "yyyy-MM-dd", CultureInfo.InvariantCulture));

            cmd.Parameters.AddWithValue(
                "@Diagnosis", txtDiagnosis.Text.Trim());

            cmd.Parameters.AddWithValue(
                "@Treatment", txtTreatment.Text.Trim());
        }

        

        protected void btnCreateAppointment_Click(
            object sender, EventArgs e)
        {
            hfActiveTab.Value = "appointments";
            ShowActiveTab();

            int patientId;

            if (!int.TryParse(
                ddlAppointmentPatient.SelectedValue,
                out patientId) || patientId <= 0)
            {
                lblMessage.Text = "Please select a patient.";
                return;
            }

            DateTime appointmentDate;

            if (!DateTime.TryParseExact(
                txtNewAppointmentDate.Text,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out appointmentDate))
            {
                lblMessage.Text =
                    "Please enter a valid appointment date.";
                return;
            }

            if (appointmentDate.Date < DateTime.Today)
            {
                lblMessage.Text =
                    "You cannot book an appointment in the past.";
                return;
            }

            TimeSpan appointmentTime;

            if (!TimeSpan.TryParseExact(
                txtAppointmentTime.Text,
                new[] { @"hh\:mm", @"h\:mm", @"hh\:mm\:ss" },
                CultureInfo.InvariantCulture,
                TimeSpanStyles.None,
                out appointmentTime))
            {
                lblMessage.Text =
                    "Please enter a valid appointment time.";
                return;
            }

            if (appointmentTime >= TimeSpan.FromDays(1))
            {
                lblMessage.Text = "Please enter a valid appointment time.";
                return;
            }

            if (string.IsNullOrWhiteSpace(txtAppointmentReason.Text))
            {
                lblMessage.Text =
                    "Please enter the reason for the appointment.";
                return;
            }

            using (SQLiteConnection conn =
                new SQLiteConnection(GetConnectionString()))
            {
                conn.Open();

                int doctorId = GetDoctorId(conn);

                if (doctorId == 0)
                {
                    lblMessage.Text = "Doctor profile not found.";
                    return;
                }

                string query = @"
                    INSERT INTO Appointments
                    (PatientId, DoctorId, AppointmentDate,
                     AppointmentTime, Reason, Status)
                    SELECT
                        PatientId, @DoctorId, @AppointmentDate,
                        @AppointmentTime, @Reason, 'Pending'
                    FROM Patients
                    WHERE PatientId = @PatientId
                      AND DoctorId = @DoctorId";

                using (SQLiteCommand cmd =
                    new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PatientId", patientId);
                    cmd.Parameters.AddWithValue("@DoctorId", doctorId);

                    cmd.Parameters.AddWithValue(
                        "@AppointmentDate",
                        appointmentDate.ToString(
                            "yyyy-MM-dd",
                            CultureInfo.InvariantCulture));

                    cmd.Parameters.AddWithValue(
                        "@AppointmentTime",
                        appointmentTime.ToString(@"hh\:mm"));

                    cmd.Parameters.AddWithValue(
                        "@Reason",
                        txtAppointmentReason.Text.Trim());

                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        lblMessage.Text =
                            "Appointment created successfully.";

                        ddlAppointmentPatient.SelectedIndex = 0;
                        txtNewAppointmentDate.Text = "";
                        txtAppointmentTime.Text = "";
                        txtAppointmentReason.Text = "";
                    }
                    else
                    {
                        lblMessage.Text =
                            "You cannot create an appointment for this patient.";
                    }
                }
            }

            ShowActiveTab();
        }

        

        protected void gvRecords_RowCommand(
            object sender,
            System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            hfActiveTab.Value = "records";
            ShowActiveTab();

            int recordId;

            if (!int.TryParse(
                Convert.ToString(e.CommandArgument), out recordId))
                return;

            using (SQLiteConnection conn =
                new SQLiteConnection(GetConnectionString()))
            {
                conn.Open();

                int doctorId = GetDoctorId(conn);

                if (doctorId == 0)
                    return;

                if (e.CommandName == "EditRecord")
                {
                    string query = @"
                        SELECT
                            mr.MedicalRecordId,
                            mr.PatientId,
                            mr.AppointmentDate,
                            mr.Diagnosis,
                            mr.Treatment
                        FROM MedicalRecords mr
                        INNER JOIN Patients p
                            ON mr.PatientId = p.PatientId
                        WHERE mr.MedicalRecordId = @RecordId
                          AND mr.DoctorId = @DoctorId
                          AND p.DoctorId = @DoctorId";

                    using (SQLiteCommand cmd =
                        new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue(
                            "@RecordId", recordId);

                        cmd.Parameters.AddWithValue(
                            "@DoctorId", doctorId);

                        using (SQLiteDataReader reader =
                            cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                hfRecordId.Value =
                                    reader["MedicalRecordId"].ToString();

                                ddlPatients.SelectedValue =
                                    reader["PatientId"].ToString();

                                txtAppointmentDate.Text =
                                    Convert.ToDateTime(
                                        reader["AppointmentDate"]
                                    ).ToString("yyyy-MM-dd");

                                txtDiagnosis.Text =
                                    reader["Diagnosis"].ToString();

                                txtTreatment.Text =
                                    reader["Treatment"].ToString();

                                btnSave.Visible = false;
                                btnUpdate.Visible = true;
                                btnCancel.Visible = true;

                                lblMessage.Text =
                                    "Editing medical record.";
                            }
                            else
                            {
                                lblMessage.Text =
                                    "Record not found or access denied.";
                            }
                        }
                    }
                }
                else if (e.CommandName == "DeleteRecord")
                {
                    string query = @"
                        DELETE FROM MedicalRecords
                        WHERE MedicalRecordId = @RecordId
                          AND DoctorId = @DoctorId
                          AND EXISTS (
                              SELECT 1
                              FROM Patients
                              WHERE Patients.PatientId =
                                    MedicalRecords.PatientId
                                AND Patients.DoctorId = @DoctorId
                          )";

                    using (SQLiteCommand cmd =
                        new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue(
                            "@RecordId", recordId);

                        cmd.Parameters.AddWithValue(
                            "@DoctorId", doctorId);

                        int rows = cmd.ExecuteNonQuery();

                        lblMessage.Text = rows > 0
                            ? "Medical record deleted successfully."
                            : "Record not found or access denied.";
                    }
                }

                LoadMedicalRecords(conn, doctorId);
            }
        }

        

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            hfActiveTab.Value = "records";
            ShowActiveTab();

            ClearForm();

            lblMessage.Text = "Editing cancelled.";
        }

        private void ClearForm()
        {
            hfRecordId.Value = "";
            ddlPatients.SelectedIndex = 0;
            txtAppointmentDate.Text = "";
            txtDiagnosis.Text = "";
            txtTreatment.Text = "";

            btnSave.Visible = true;
            btnUpdate.Visible = false;
            btnCancel.Visible = false;
        }

        

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();

            Response.Redirect("~/Pages/login.aspx");
        }
    }
}
