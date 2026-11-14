<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="patient.aspx.cs" Inherits="WebPageBryanPablo1.Pages.patient" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <title>Patient Portal | Health Centre</title>

    
    <link href="../css/doctor.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">

        
        <header class="navbar">
            <div class="brand">
                <i class="bi bi-heart-pulse-fill me-2"></i>
                Health Centre
            </div>

            <div class="nav-right">
                Patient Portal

                <asp:LinkButton ID="btnLogout" runat="server" CssClass="logout-link" OnClick="btnLogout_Click" CausesValidation="false">
                    <i class="bi bi-box-arrow-right me-1"></i>
                    Log out
                </asp:LinkButton>
            </div>
        </header>

        <main class="container">

            
            <section class="page-header">
                <div>
                    <h1>My Medical Dashboard</h1>
                    <p>Review your personal information, assigned physician, and medical history records.</p>
                </div>

                <div class="doctor-label">
                    <span class="status-dot"></span>
                    Patient account
                </div>
            </section>

            
            <asp:Label ID="lblMessage" runat="server" CssClass="message" EnableViewState="false" style="color: #c44747;" />

            
            <section class="panel">
                <div class="panel-header">
                    <div>
                        <h2>Personal Information</h2>
                        <p>Your registered details in the health center database.</p>
                    </div>
                </div>

                <div class="form-grid">
                    <div class="form-group">
                        <label>Full Name</label>
                        <asp:Label ID="lblFullName" runat="server" CssClass="input-control" Text="-" style="display:block; padding-top: 12px; background-color: #fafbfc;"></asp:Label>
                    </div>

                    <div class="form-group">
                        <label>Date of Birth</label>
                        <asp:Label ID="lblDateOfBirth" runat="server" CssClass="input-control" Text="-" style="display:block; padding-top: 12px; background-color: #fafbfc;"></asp:Label>
                    </div>

                    <div class="form-group">
                        <label>Address</label>
                        <asp:Label ID="lblAddress" runat="server" CssClass="input-control" Text="-" style="display:block; padding-top: 12px; background-color: #fafbfc;"></asp:Label>
                    </div>

                    <div class="form-group">
                        <label>Mobile Number</label>
                        <asp:Label ID="lblMobile" runat="server" CssClass="input-control" Text="-" style="display:block; padding-top: 12px; background-color: #fafbfc;"></asp:Label>
                    </div>
                </div>

                
                <div class="panel-header" style="margin-top: 35px; margin-bottom: 20px; border-top: 1px solid #e9eef4; padding-top: 25px;">
                    <div>
                        <h2>Assigned Physician</h2>
                        <p>Your primary health care provider details.</p>
                    </div>
                </div>

                <div class="form-grid">
                    <div class="form-group">
                        <label>Doctor Name</label>
                        <asp:Label ID="lblDoctorName" runat="server" CssClass="input-control" Text="-" style="display:block; padding-top: 12px; background-color: #fafbfc;"></asp:Label>
                    </div>

                    <div class="form-group">
                        <label>Doctor Mobile</label>
                        <asp:Label ID="lblDoctorMobile" runat="server" CssClass="input-control" Text="-" style="display:block; padding-top: 12px; background-color: #fafbfc;"></asp:Label>
                    </div>
                </div>
            </section>

            
            <section class="panel records-panel">
                <div class="panel-header">
                    <div>
                        <h2>Medical Records History</h2>
                        <p>Observations, diagnoses, and treatments registered by your doctor.</p>
                    </div>
                </div>

                <div class="table-wrapper">
                    <asp:GridView ID="gvMedicalRecords" runat="server" 
                        AutoGenerateColumns="false" 
                        CssClass="records-table" 
                        GridLines="None" 
                        EmptyDataText="No medical records found.">
                        <Columns>
                            <asp:BoundField DataField="AppointmentDate" HeaderText="Appointment Date" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="Diagnosis" HeaderText="Diagnosis" />
                            <asp:BoundField DataField="Treatment" HeaderText="Treatment / Observations" />
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