
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="WebPageBryanPablo1.Pages.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <title>Login - HealthCare Plus</title>

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />

    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" rel="stylesheet" />

    <link href="../css/login.css" rel="stylesheet" />

</head>

<body>

    <form id="form1" runat="server">

        <nav class="navbar navbar-expand-lg navbar-dark main-navbar">

            <div class="container">

                <a class="navbar-brand fw-bold" href="../index.aspx">
                    <i class="bi bi-heart-pulse me-2"></i>
                    HealthCare Plus
                </a>

                <button class="navbar-toggler" type="button"
                    data-bs-toggle="collapse"
                    data-bs-target="#mainMenu"
                    aria-controls="mainMenu"
                    aria-expanded="false"
                    aria-label="Toggle navigation">

                    <span class="navbar-toggler-icon"></span>

                </button>

                <div class="collapse navbar-collapse" id="mainMenu">

                    <ul class="navbar-nav ms-auto">

                        <li class="nav-item">
                            <a class="nav-link" href="../index.aspx">Home</a>
                        </li>

                        <li class="nav-item">
                            <a class="nav-link" href="../index.aspx#about">About Us</a>
                        </li>

                        <li class="nav-item">
                            <a class="nav-link" href="../index.aspx#services">Services</a>
                        </li>

                        <li class="nav-item">
                            <a class="nav-link" href="../index.aspx#contact">Contact</a>
                        </li>

                    </ul>

                </div>

            </div>

        </nav>


        <main class="login-section">

            <div class="container">

                <div class="row justify-content-center align-items-center">

                    <div class="col-12 col-sm-10 col-md-7 col-lg-5">

                        <div class="card login-card shadow">

                            <div class="card-body p-4 p-md-5">


                                <div class="text-center mb-4">

                                    <div class="login-icon mb-3">
                                        <i class="bi bi-person-circle"></i>
                                    </div>

                                    <h2 class="fw-bold main-title">
                                        Welcome Back!
                                    </h2>

                                    <p class="text-secondary">
                                        Please sign in to your account
                                    </p>

                                </div>

                    

                                <div class="mb-3">

                                    <asp:Label
                                        ID="lblUserID"
                                        runat="server"
                                        AssociatedControlID="txtUserID"
                                        CssClass="form-label fw-semibold"
                                        Text="User ID">
                                    </asp:Label>

                                    

                                    <asp:TextBox
                                        ID="txtUserID"
                                        runat="server"
                                        CssClass="form-control"
                                        placeholder="Enter your ID number">
                                    </asp:TextBox>

                                </div>

                                <div class="mb-4">

                                    <asp:Label
                                        ID="lblPassword"
                                        runat="server"
                                        AssociatedControlID="txtPassword"
                                        CssClass="form-label fw-semibold"
                                        Text="Password">
                                    </asp:Label>



                                    <asp:TextBox
                                        ID="txtPassword"
                                        runat="server"
                                        TextMode="Password"
                                        CssClass="form-control"
                                        placeholder="Enter your password">
                                    </asp:TextBox>


                                </div>


                                <asp:Label
                                    ID="lblMessage"
                                    runat="server"
                                    CssClass="text-danger d-block mb-3"
                                    EnableViewState="false">
                                </asp:Label>


                                <asp:Button
                                    ID="btnLogin"
                                    runat="server"
                                    Text="Sign In"
                                    CssClass="btn btn-primary btn-lg w-100"
                                    ValidationGroup="Login"
                                    OnClick="btnLogin_Click" />


                                <div class="text-center mt-4">

                                    <p class="text-secondary small mb-2">
                                        Access reserved for authorised users.
                                    </p>

                                    <a href="../index.aspx" class="back-home">
                                        <i class="bi bi-arrow-left me-1"></i>
                                        Back to Home
                                    </a>

                                </div>

                            </div>

                        </div>

                    </div>

                </div>

            </div>

        </main>

        <footer class="main-footer py-4">

            <div class="container text-center">

                <p class="mb-1">
                    HealthCare Plus | info@healthcareplus.com
                </p>

                <p class="mb-0">
                    © 2026 HealthCare Plus. All rights reserved.
                </p>

            </div>

        </footer>

        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>

    </form>

</body>

</html>
