
<%@ Page Title="Patient" Language="C#" MasterPageFile="" AutoEventWireup="true" CodeBehind="patient.aspx.cs" Inherits="WebPageBryanPablo1.Pages.patient" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Patient - Health Centre</title>
    <link href="../css/doctor.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">

        <nav class="navbar">
            <div class="navbar-brand">
                <i class="bi bi-heart-pulse"></i>
                Health Centre
            </div>

            <asp:Button
                ID="btnLogout"
                runat="server"
                Text="Logout"
                CssClass="btn"
                OnClick="btnLogout_Click" />
        </nav>

        <main class="container">

            <asp:Label
                ID="lblMessage"
                runat="server"
                CssClass="message">
            </asp:Label>

            
            <section class="panel">
                <h2>Personal Information</h2>

                <div class="info-grid">
                    <div>
                        <strong>Full Name</strong>
                        <asp:Label ID="lblFullName" runat="server" />
                    </div>

                    <div>
                        <strong>Date of Birth</strong>
                        <asp:Label ID="lblDateOfBirth" runat="server" />
                    </div>

                    <div>
                        <strong>Address</strong>
                        <asp:Label ID="lblAddress" runat="server" />
                    </div>

                    <div>
                        <strong>Mobile</strong>
                        <asp:Label ID="lblMobile" runat="server" />
                    </div>
                </div>

                <h3>Your Doctor</h3>

                <div class="info-grid">
                    <div>
                        <strong>Doctor Name</strong>
                        <asp:Label ID="lblDoctorName" runat="server" />
                    </div>

                    <div>
                        <strong>Doctor Mobile</strong>
                        <asp:Label ID="lblDoctorMobile" runat="server" />
                    </div>
                </div>
            </section>

            
            <section class="panel">
                <h2>Next Appointment</h2>

                <asp:Label
                    ID="lblNextAppointmentMessage"
                    runat="server"
                    CssClass="message">
                </asp:Label>

                <div class="info-grid">
                    <div>
                        <strong>Date</strong>
                        <asp:Label ID="lblNextAppointmentDate" runat="server" Text="-" />
                    </div>

                    <div>
                        <strong>Time</strong>
                        <asp:Label ID="lblNextAppointmentTime" runat="server" Text="-" />
                    </div>

                    <div>
                        <strong>Reason</strong>
                        <asp:Label ID="lblNextAppointmentReason" runat="server" Text="-" />
                    </div>

                    <div>
                        <strong>Status</strong>
                        <asp:Label ID="lblNextAppointmentStatus" runat="server" Text="-" />
                    </div>
                </div>
            </section>

            
            <section class="panel">
                <h2>Medical Records History</h2>

                <asp:GridView
                    ID="gvMedicalRecords"
                    runat="server"
                    AutoGenerateColumns="False"
                    CssClass="table"
                    EmptyDataText="No medical records found.">

                    <Columns>
                        <asp:BoundField
                            DataField="AppointmentDate"
                            HeaderText="Date" />

                        <asp:BoundField
                            DataField="Diagnosis"
                            HeaderText="Diagnosis" />

                        <asp:BoundField
                            DataField="Treatment"
                            HeaderText="Treatment" />
                    </Columns>
                </asp:GridView>
            </section>

        </main>

    </form>
</body>
</html>
