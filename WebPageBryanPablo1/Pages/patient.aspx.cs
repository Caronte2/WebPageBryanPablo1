using System;
using System.Data;
using System.Data.SQLite;

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
                using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString()))
                {
                    conn.Open();
                    LoadPatientProfile(conn);
                }
            }
        }

        private void LoadPatientProfile(SQLiteConnection conn)
        {
            int userId = Convert.ToInt32(Session["UserId"]);

           
            string queryPatient = @"
                SELECT 
                    p.PatientId, 
                    p.FullName, 
                    p.DateOfBirth, 
                    p.Address, 
                    p.Mobile,
                    d.FullName AS DoctorName,
                    d.Mobile AS DoctorMobile
                FROM Patients p
                LEFT JOIN Doctors d ON p.DoctorId = d.DoctorId
                WHERE p.UserId = @UserId";

            int patientId = 0;

            using (SQLiteCommand cmd = new SQLiteCommand(queryPatient, conn))
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

                        
                        lblDoctorName.Text = reader["DoctorName"] != DBNull.Value ? reader["DoctorName"].ToString() : "No assigned doctor";
                        lblDoctorMobile.Text = reader["DoctorMobile"] != DBNull.Value ? reader["DoctorMobile"].ToString() : "N/A";
                    }
                    else
                    {
                        lblMessage.Text = "No patient profile was found for this account.";
                        return;
                    }
                }
            }

            
            string queryRecords = @"
                SELECT 
                    AppointmentDate,
                    Diagnosis,
                    Treatment
                FROM MedicalRecords
                WHERE PatientId = @PatientId
                ORDER BY AppointmentDate DESC";

            using (SQLiteCommand cmdRec = new SQLiteCommand(queryRecords, conn))
            {
                cmdRec.Parameters.AddWithValue("@PatientId", patientId);

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmdRec))
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