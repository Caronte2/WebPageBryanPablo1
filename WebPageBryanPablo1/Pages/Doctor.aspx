
<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="Doctor.aspx.cs"
    Inherits="WebPageBryanPablo1.Pages.doctor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <title>Doctor Dashboard | Health Centre</title>

    <link href="../css/doctor.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" rel="stylesheet" />
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
                <i class="bi bi-person-gear me-1"></i>
                Doctor Portal

                <a class="logout-link" href="login.aspx">
                    <i class="bi bi-box-arrow-right me-1"></i>
                    Log out
                </a>
            </div>
        </header>

        <main class="container">

            
            <section class="page-header">
                <div>
                    <h1>Doctor Dashboard</h1>
                    <p>Manage medical records and patient appointments.</p>
                </div>

                <div class="doctor-label">
                    <span class="status-dot"></span>
                    Doctor account
                </div>
            </section>

            
            <asp:Label ID="lblMessage" runat="server"
                CssClass="message" />

            
            <asp:HiddenField
                ID="hfActiveTab"
                runat="server"
                Value="records" />

            
            <div class="tabs-container">

                <asp:Button
                    ID="btnRecordsTab"
                    runat="server"
                    Text="Patients Records"
                    CssClass="tab-button active"
                    OnClick="btnRecordsTab_Click"
                    CausesValidation="false" />

                <asp:Button
                    ID="btnAppointmentsTab"
                    runat="server"
                    Text="Create Appointment"
                    CssClass="tab-button"
                    OnClick="btnAppointmentsTab_Click"
                    CausesValidation="false" />

            </div>


            

            <asp:Panel ID="pnlRecords" runat="server">

                
                <section class="panel">

                    <div class="panel-header">
                        <div>
                            <h2>Medical Record Management</h2>
                            <p>Create or update a patient's medical record.</p>
                        </div>
                    </div>

                    <asp:HiddenField ID="hfRecordId" runat="server" />

                    <div class="form-grid">

                        <div class="form-group">
                            <label>Patient</label>

                            <asp:DropDownList
                                ID="ddlPatients"
                                runat="server"
                                CssClass="input-control">
                            </asp:DropDownList>
                        </div>

                        <div class="form-group">
                            <label>Appointment Date</label>

                            <asp:TextBox
                                ID="txtAppointmentDate"
                                runat="server"
                                TextMode="Date"
                                CssClass="input-control">
                            </asp:TextBox>
                        </div>

                        <div class="form-group full-width">
                            <label>Diagnosis</label>

                            <asp:TextBox
                                ID="txtDiagnosis"
                                runat="server"
                                TextMode="MultiLine"
                                Rows="3"
                                CssClass="input-control"
                                placeholder="Enter the patient's diagnosis">
                            </asp:TextBox>
                        </div>

                        <div class="form-group full-width">
                            <label>Treatment</label>

                            <asp:TextBox
                                ID="txtTreatment"
                                runat="server"
                                TextMode="MultiLine"
                                Rows="3"
                                CssClass="input-control"
                                placeholder="Enter the prescribed treatment">
                            </asp:TextBox>
                        </div>

                    </div>

                    <div class="form-actions">

                        <asp:Button
                            ID="btnSave"
                            runat="server"
                            Text="Save Record"
                            CssClass="btn btn-primary"
                            OnClick="btnSave_Click" />

                        <asp:Button
                            ID="btnUpdate"
                            runat="server"
                            Text="Update Record"
                            CssClass="btn btn-primary"
                            OnClick="btnUpdate_Click"
                            Visible="false" />

                        <asp:Button
                            ID="btnCancel"
                            runat="server"
                            Text="Cancel"
                            CssClass="btn btn-secondary"
                            OnClick="btnCancel_Click"
                            Visible="false"
                            CausesValidation="false" />

                    </div>

                </section>


                
                <section class="panel records-panel">

                    <div class="panel-header">
                        <div>
                            <h2>My Patients' Medical Records</h2>
                            <p>Only records belonging to your assigned patients are displayed.</p>
                        </div>
                    </div>

                    <div class="table-wrapper">

                        <asp:GridView
                            ID="gvRecords"
                            runat="server"
                            AutoGenerateColumns="false"
                            CssClass="records-table"
                            GridLines="None"
                            EmptyDataText="No medical records found."
                            OnRowCommand="gvRecords_RowCommand">

                            <Columns>

                                <asp:BoundField
                                    DataField="PatientName"
                                    HeaderText="Patient" />

                                <asp:BoundField
                                    DataField="AppointmentDate"
                                    HeaderText="Appointment Date"
                                    DataFormatString="{0:dd/MM/yyyy}" />

                                <asp:BoundField
                                    DataField="Diagnosis"
                                    HeaderText="Diagnosis" />

                                <asp:BoundField
                                    DataField="Treatment"
                                    HeaderText="Treatment" />

                                <asp:TemplateField HeaderText="Actions">

                                    <ItemTemplate>

                                        <asp:LinkButton
                                            ID="btnEditRecord"
                                            runat="server"
                                            Text="Edit"
                                            CssClass="action-edit"
                                            CommandName="EditRecord"
                                            CommandArgument='<%# Eval("MedicalRecordId") %>'
                                            CausesValidation="false" />

                                        <asp:LinkButton
                                            ID="btnDeleteRecord"
                                            runat="server"
                                            Text="Delete"
                                            CssClass="action-delete"
                                            CommandName="DeleteRecord"
                                            CommandArgument='<%# Eval("MedicalRecordId") %>'
                                            CausesValidation="false"
                                            OnClientClick="return confirm('Are you sure you want to delete this medical record?');" />

                                    </ItemTemplate>

                                </asp:TemplateField>

                            </Columns>

                        </asp:GridView>

                    </div>

                </section>

            </asp:Panel>


            

            <asp:Panel
                ID="pnlAppointments"
                runat="server"
                Visible="false">

                <section class="panel">

                    <div class="panel-header">
                        <div>
                            <h2>Create New Appointment</h2>
                            <p>Schedule a new appointment for one of your patients.</p>
                        </div>
                    </div>

                    <div class="form-grid">

                        
                        <div class="form-group">
                            <label>Patient</label>

                            <asp:DropDownList
                                ID="ddlAppointmentPatient"
                                runat="server"
                                CssClass="input-control">
                            </asp:DropDownList>
                        </div>

                        
                        <div class="form-group">
                            <label>Appointment Date</label>

                            <asp:TextBox
                                ID="txtNewAppointmentDate"
                                runat="server"
                                TextMode="Date"
                                CssClass="input-control">
                            </asp:TextBox>
                        </div>

                       
                        <div class="form-group">
                            <label>Appointment Time</label>

                            <asp:TextBox
                                ID="txtAppointmentTime"
                                runat="server"
                                TextMode="Time"
                                CssClass="input-control">
                            </asp:TextBox>
                        </div>

                        
                        <div class="form-group full-width">
                            <label>Reason for Appointment</label>

                            <asp:TextBox
                                ID="txtAppointmentReason"
                                runat="server"
                                TextMode="MultiLine"
                                Rows="3"
                                CssClass="input-control"
                                placeholder="Enter the reason for the appointment">
                            </asp:TextBox>
                        </div>

                    </div>

                    
                    <div class="form-actions">

                        <asp:Button
                            ID="btnCreateAppointment"
                            runat="server"
                            Text="Create Appointment"
                            CssClass="btn btn-primary"
                            OnClick="btnCreateAppointment_Click" />

                    </div>

                </section>

            </asp:Panel>

        </main>

        
        <footer class="footer">
            <p>© Health Centre - Medical Management System</p>
        </footer>

    </form>
</body>
</html>
