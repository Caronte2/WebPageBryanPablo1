<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="administrator.aspx.cs" Inherits="WebPageBryanPablo1.Pages.administrator" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Administrator - HealthCare Plus</title>

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" rel="stylesheet" />
    <link href="../css/administrator.css" rel="stylesheet" />
</head>

<body>
    <form id="form1" runat="server">

        <nav class="navbar navbar-expand-lg navbar-dark main-navbar">
            <div class="container-fluid px-4">
                <a class="navbar-brand" href="../index.aspx">
                    <i class="bi bi-heart-pulse-fill me-2"></i>
                    HealthCare Plus
                </a>

                <button class="navbar-toggler" type="button"
                    data-bs-toggle="collapse"
                    data-bs-target="#adminNavbar"
                    aria-controls="adminNavbar"
                    aria-expanded="false"
                    aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>

                <div class="collapse navbar-collapse" id="adminNavbar">
                    <ul class="navbar-nav ms-auto align-items-lg-center">
                        <li class="nav-item">
                            <span class="nav-link admin-label">
                                <i class="bi bi-person-gear me-1"></i>
                                Administrator
                            </span>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link logout-link" href="login.aspx">
                                <i class="bi bi-box-arrow-right me-1"></i>
                                Log out
                            </a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>

        <main class="admin-container">
            <div class="container-fluid px-4 py-4">

                <div class="welcome-section mb-4">
                    <h1>Administrator Dashboard</h1>
                    <p>Manage doctors, patients and healthcare centre users.</p>
                </div>

                <div class="row g-4 mb-4">

                    <div class="col-md-4">
                        <div class="stat-card">
                            <div class="stat-icon doctors-icon">
                                <i class="bi bi-person-badge"></i>
                            </div>
                            <div>
                                <p>Total Doctors</p>
                                <h3><asp:Label ID="lblTotalDoctors" runat="server" Text="0" /></h3>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-4">
                        <div class="stat-card">
                            <div class="stat-icon patients-icon">
                                <i class="bi bi-people"></i>
                            </div>
                            <div>
                                <p>Total Patients</p>
                                <h3><asp:Label ID="lblTotalPatients" runat="server" Text="0" /></h3>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-4">
                        <div class="stat-card">
                            <div class="stat-icon users-icon">
                                <i class="bi bi-person-check"></i>
                            </div>
                            <div>
                                <p>Total Users</p>
                                <h3><asp:Label ID="lblTotalUsers" runat="server" Text="0" /></h3>
                            </div>
                        </div>
                    </div>

                </div>

                <div class="row g-4">

                    <div class="col-lg-4">
                        <div class="content-card">

                            <div class="card-heading">
                                <div>
                                    <h4><asp:Label ID="lblFormTitle" runat="server" Text="Create User" /></h4>
                                    <p>Register or update a doctor or patient.</p>
                                </div>
                                <i class="bi bi-person-plus heading-icon"></i>
                            </div>

                            <asp:HiddenField ID="hfEditingUserId" runat="server" />
                            <asp:HiddenField ID="hfEditingEntityId" runat="server" />
                            <asp:HiddenField ID="hfEditingRoleId" runat="server" />

                            <div class="mb-3">
                                <label class="form-label" for="<%= ddlUserType.ClientID %>">User Type</label>
                                <asp:DropDownList ID="ddlUserType" runat="server"
                                    CssClass="form-select"
                                    onchange="updateUserFields()">
                                    <asp:ListItem Text="Select user type" Value="" />
                                    <asp:ListItem Text="Doctor" Value="Doctor" />
                                    <asp:ListItem Text="Patient" Value="Patient" />
                                </asp:DropDownList>
                            </div>

                            <div class="mb-3">
                                <label class="form-label" for="<%= txtDNI.ClientID %>">DNI</label>
                                <asp:TextBox ID="txtDNI" runat="server"
                                    CssClass="form-control"
                                    placeholder="Enter DNI" />
                            </div>

                            <div class="mb-3">
                                <label class="form-label" for="<%= txtFullName.ClientID %>">Full Name</label>
                                <asp:TextBox ID="txtFullName" runat="server"
                                    CssClass="form-control"
                                    placeholder="Enter full name" />
                            </div>

                            <div class="mb-3">
                                <label class="form-label" for="<%= txtNewPassword.ClientID %>">Password</label>
                                <asp:TextBox ID="txtNewPassword" runat="server"
                                    TextMode="Password"
                                    CssClass="form-control"
                                    placeholder="Enter password" />
                                <small class="form-text">
                                    When editing, leave empty to keep the current password.
                                </small>
                            </div>

                            <div class="mb-3">
                                <label class="form-label" for="<%= txtMobile.ClientID %>">Mobile</label>
                                <asp:TextBox ID="txtMobile" runat="server"
                                    CssClass="form-control"
                                    placeholder="Enter mobile number" />
                            </div>

                            <div class="mb-3">
                                <label class="form-label" for="<%= txtDateOfBirth.ClientID %>">Date of Birth</label>
                                <asp:TextBox ID="txtDateOfBirth" runat="server"
                                    TextMode="Date"
                                    CssClass="form-control" />
                            </div>

                            <div class="mb-3">
                                <label class="form-label" for="<%= txtAddress.ClientID %>">Address</label>
                                <asp:TextBox ID="txtAddress" runat="server"
                                    CssClass="form-control"
                                    placeholder="Enter address" />
                            </div>

                            <div class="mb-3">
                                <label class="form-label" for="<%= ddlDoctor.ClientID %>">Assigned Doctor</label>
                                <asp:DropDownList ID="ddlDoctor" runat="server" CssClass="form-select">
                                    <asp:ListItem Text="Select a doctor" Value="" />
                                </asp:DropDownList>
                            </div>

                            <asp:Label ID="lblAdminMessage" runat="server"
                                CssClass="message-label"
                                EnableViewState="false" />

                            <asp:Button ID="btnCreateUser" runat="server"
                                Text="Create User"
                                CssClass="btn btn-primary w-100 mt-3"
                                OnClick="btnCreateUser_Click" />

                            <asp:Button ID="btnCancelEdit" runat="server"
                                Text="Cancel Editing"
                                CssClass="btn btn-outline-secondary w-100 mt-2"
                                OnClick="btnCancelEdit_Click"
                                CausesValidation="false"
                                Visible="false" />

                        </div>
                    </div>

                    <div class="col-lg-8">

                        <div class="content-card mb-4">
                            <div class="card-heading">
                                <div>
                                    <h4>Doctors</h4>
                                    <p>Manage registered doctors.</p>
                                </div>
                                <i class="bi bi-person-badge heading-icon"></i>
                            </div>

                            <div class="table-responsive">
                                <asp:GridView ID="gvDoctors" runat="server"
                                    CssClass="table admin-table"
                                    AutoGenerateColumns="False"
                                    GridLines="None"
                                    DataKeyNames="DoctorId,UserId"
                                    OnRowCommand="gvDoctors_RowCommand"
                                    EmptyDataText="No doctors registered yet."
                                    UseAccessibleHeader="true">

                                    <Columns>
                                        <asp:BoundField DataField="DoctorId" HeaderText="ID" />
                                        <asp:BoundField DataField="DNI" HeaderText="DNI" />
                                        <asp:BoundField DataField="FullName" HeaderText="Full Name" />
                                        <asp:BoundField DataField="Mobile" HeaderText="Mobile" />

                                        <asp:TemplateField HeaderText="Actions">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEditDoctor" runat="server"
                                                    CssClass="btn btn-sm btn-edit"
                                                    CommandName="EditDoctor"
                                                    CommandArgument="<%# Container.DataItemIndex %>"
                                                    CausesValidation="false">
                                                    <i class="bi bi-pencil-square"></i> Edit
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>

                        <div class="content-card">
                            <div class="card-heading">
                                <div>
                                    <h4>Patients</h4>
                                    <p>Manage registered patients.</p>
                                </div>
                                <i class="bi bi-people heading-icon"></i>
                            </div>

                            <div class="table-responsive">
                                <asp:GridView ID="gvPatients" runat="server"
                                    CssClass="table admin-table"
                                    AutoGenerateColumns="False"
                                    GridLines="None"
                                    DataKeyNames="PatientId,UserId"
                                    OnRowCommand="gvPatients_RowCommand"
                                    EmptyDataText="No patients registered yet."
                                    UseAccessibleHeader="true">

                                    <Columns>
                                        <asp:BoundField DataField="PatientId" HeaderText="ID" />
                                        <asp:BoundField DataField="DNI" HeaderText="DNI" />
                                        <asp:BoundField DataField="FullName" HeaderText="Full Name" />
                                        <asp:BoundField DataField="Mobile" HeaderText="Mobile" />
                                        <asp:BoundField DataField="DateOfBirth" HeaderText="Date of Birth" />
                                        <asp:BoundField DataField="AssignedDoctor" HeaderText="Assigned Doctor" />

                                        <asp:TemplateField HeaderText="Actions">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEditPatient" runat="server"
                                                    CssClass="btn btn-sm btn-edit"
                                                    CommandName="EditPatient"
                                                    CommandArgument="<%# Container.DataItemIndex %>"
                                                    CausesValidation="false">
                                                    <i class="bi bi-pencil-square"></i> Edit
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>

                    </div>
                </div>

            </div>
        </main>

        <footer class="main-footer">
            <p>© 2026 HealthCare Plus. All rights reserved.</p>
        </footer>

        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>

        <script>
            function updateUserFields() {
                var userType = document.getElementById('<%= ddlUserType.ClientID %>').value;

                var dateOfBirth = document.getElementById('<%= txtDateOfBirth.ClientID %>');
                var address = document.getElementById('<%= txtAddress.ClientID %>');
    var assignedDoctor = document.getElementById('<%= ddlDoctor.ClientID %>');

    var isPatient = userType === "Patient";

    dateOfBirth.disabled = !isPatient;
    address.disabled = !isPatient;
    assignedDoctor.disabled = !isPatient;

    if (!isPatient) {
        dateOfBirth.value = "";
        address.value = "";
        assignedDoctor.selectedIndex = 0;
    }
}

document.addEventListener("DOMContentLoaded", updateUserFields);
</script>

    </form>
</body>
</html>
