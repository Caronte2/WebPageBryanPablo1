
using System;
using System.Data;
using System.Data.SQLite;
using System.Globalization;

namespace WebPageBryanPablo1.Pages
{
    public partial class patient : System.Web.UI.Page
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
                Convert.ToInt32(Session["RoleId"]) != 3)
            {
                Response.Redirect("~/Pages/login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                using (SQLiteConnection conn =
                    new SQLiteConnection(GetConnectionString()))
                {
                    conn.Open();
                    LoadPatientProfile(conn);
                }
            }
        }

        private void LoadPatientProfile(SQLiteConnection conn)
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            int patientId = 0;

            string query = @"
                SELECT
                    p.PatientId,
                    p.FullName,
                    p.DateOfBirth,
                    p.Address,
                    p.Mobile,
                    d.FullName AS DoctorName,
                    d.Mobile AS DoctorMobile
                FROM Patients p
                LEFT JOIN Doctors d
                    ON p.DoctorId = d.DoctorId
                WHERE p.UserId = @UserId";

            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@UserId", userId);

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        patientId = Convert.ToInt32(reader["PatientId"]);

                        lblFullName.Text = reader["FullName"].ToString();
                        lblDateOfBirth.Text = reader["DateOfBirth"].ToString();
                        lblAddress.Text = reader["Address"].ToString();
                        lblMobile.Text = reader["Mobile"].ToString();

                        lblDoctorName.Text =
                            reader["DoctorName"] == DBNull.Value
                            ? "Not assigned"
                            : reader["DoctorName"].ToString();

                        lblDoctorMobile.Text =
                            reader["DoctorMobile"] == DBNull.Value
                            ? "Not available"
                            : reader["DoctorMobile"].ToString();
                    }
                    else
                    {
                        lblMessage.Text = "Patient information not found.";
                        return;
                    }
                }
            }

            LoadNextAppointment(conn, patientId);
            LoadMedicalRecords(conn, patientId);
        }

        private void LoadNextAppointment(
            SQLiteConnection conn,
            int patientId)
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            string currentTime = DateTime.Now.ToString("HH:mm");

            string query = @"
                SELECT
                    AppointmentDate,
                    AppointmentTime,
                    Reason,
                    Status
                FROM Appointments
                WHERE PatientId = @PatientId
                  AND Status = 'Pending'
                  AND (
                        AppointmentDate > @Today
                        OR (
                            AppointmentDate = @Today
                            AND AppointmentTime >= @CurrentTime
                        )
                  )
                ORDER BY AppointmentDate ASC, AppointmentTime ASC
                LIMIT 1";

            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@PatientId", patientId);
                cmd.Parameters.AddWithValue("@Today", today);
                cmd.Parameters.AddWithValue("@CurrentTime", currentTime);

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        DateTime appointmentDate;

                        string dateValue =
                            reader["AppointmentDate"].ToString();

                        if (DateTime.TryParseExact(
                            dateValue,
                            "yyyy-MM-dd",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out appointmentDate))
                        {
                            lblNextAppointmentDate.Text =
                                appointmentDate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            lblNextAppointmentDate.Text = dateValue;
                        }

                        lblNextAppointmentTime.Text =
                            reader["AppointmentTime"].ToString();

                        lblNextAppointmentReason.Text =
                            reader["Reason"].ToString();

                        lblNextAppointmentStatus.Text =
                            reader["Status"].ToString();

                        lblNextAppointmentMessage.Text =
                            "Your next appointment is scheduled.";
                    }
                    else
                    {
                        lblNextAppointmentDate.Text = "-";
                        lblNextAppointmentTime.Text = "-";
                        lblNextAppointmentReason.Text = "-";
                        lblNextAppointmentStatus.Text = "-";

                        lblNextAppointmentMessage.Text =
                            "You have no upcoming appointments.";
                    }
                }
            }
        }

        private void LoadMedicalRecords(
            SQLiteConnection conn,
            int patientId)
        {
            string query = @"
                SELECT AppointmentDate, Diagnosis, Treatment
                FROM MedicalRecords
                WHERE PatientId = @PatientId
                ORDER BY AppointmentDate DESC";

            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@PatientId", patientId);

                using (SQLiteDataAdapter adapter =
                    new SQLiteDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    gvMedicalRecords.DataSource = dt;
                    gvMedicalRecords.DataBind();
                }
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();

            Response.Redirect("~/Pages/login.aspx");
        }
    }
}
