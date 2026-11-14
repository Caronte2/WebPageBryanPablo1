
<%@ Page Title="Patient" Language="C#" MasterPageFile="" AutoEventWireup="true" CodeBehind="patient.aspx.cs" Inherits="WebPageBryanPablo1.Pages.patient" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Patient - Health Centre</title>
    <link href="../css/patient.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">

        <header class="navbar">
    <div class="brand">
        <a class="navbar-brand" href="../index.aspx">
            <i class="bi bi-heart-pulse-fill me-2"></i>
            Health Centre
        </a>
    </div>

    <div class="nav-right">
        <i class="bi bi-person-fill me-1"></i>
        Patient

        <asp:LinkButton
            ID="btnLogout"
            runat="server"
            CssClass="logout-link"
            OnClick="btnLogout_Click">
            <i class="bi bi-box-arrow-right me-1"></i>
            Log out
        </asp:LinkButton>
    </div>
</header>

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
    <div class="panel-header">
        <h2>Medical Records History</h2>
        <p>View your previous medical consultations and treatments.</p>
    </div>

    <div class="table-wrapper">
        <asp:GridView
            ID="gvMedicalRecords"
            runat="server"
            AutoGenerateColumns="False"
            CssClass="records-table"
            GridLines="None"
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
    </div>
</section>

        </main>
        <footer class="footer">
            <p>© Health Centre - Medical Management System</p>
        </footer>
    </form>
</body>
</html>
