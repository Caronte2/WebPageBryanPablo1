
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="WebPageBryanPablo1.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <title>HealthCare Plus</title>

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />

    <link href="css/index.css" rel="stylesheet" />
</head>

<body>

    <form id="form1" runat="server">

        <nav class="navbar navbar-expand-lg navbar-dark main-navbar">
            <div class="container">

                <a class="navbar-brand fw-bold" href="index.aspx">
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
                            <a class="nav-link active" href="index.aspx">Home</a>
                        </li>

                        <li class="nav-item">
                            <a class="nav-link" href="#about">About Us</a>
                        </li>

                        <li class="nav-item">
                            <a class="nav-link" href="#services">Services</a>
                        </li>

                        <li class="nav-item">
                            <a class="nav-link" href="#contact">Contact</a>
                        </li>

                        <li class="nav-item ms-lg-3">
                            <a class="btn btn-light login-button" href="Pages/login.aspx">
                                Login
                            </a>
                        </li>

                    </ul>

                </div>

            </div>
        </nav>

        <section class="hero-section" id="about">

            <div class="container">

                <div class="row align-items-center g-5">

                    <div class="col-lg-6">

                        <span class="badge bg-primary-subtle text-primary mb-3">
                            WELCOME TO HEALTHCARE PLUS
                        </span>

                        <h1 class="display-4 fw-bold main-title">
                            Your Health, Our Priority
                        </h1>

                        <p class="lead text-secondary mt-3">
                            We provide professional medical services
                            and personalised care for you and your family.
                            Our team is committed to improving your health
                            and well-being.
                        </p>

                        <a href="Login.aspx" class="btn btn-primary btn-lg mt-3">
                            Access Patient Portal
                        </a>

                    </div>

                    <div class="col-lg-6">

                        <img src="Images/clinic.jpg"
                            class="img-fluid rounded-4 shadow"
                            alt="Healthcare Centre" />

                    </div>

                </div>

            </div>

        </section>

        <section class="services-section py-5" id="services">

            <div class="container">

                <div class="text-center mb-5">

                    <h2 class="fw-bold main-title">
                        Our Medical Services
                    </h2>

                    <p class="text-secondary">
                        Professional care for a healthier life.
                    </p>

                </div>

                <div class="row g-4">

                    <div class="col-md-6 col-lg-4">

                        <div class="card service-card h-100">

                            <div class="card-body text-center p-4">

                                <h5 class="card-title fw-bold">
                                    General Medicine
                                </h5>

                                <p class="card-text text-secondary">
                                    Medical consultations and general
                                    health examinations.
                                </p>

                            </div>

                        </div>

                    </div>

                    <div class="col-md-6 col-lg-4">

                        <div class="card service-card h-100">

                            <div class="card-body text-center p-4">

                                <h5 class="card-title fw-bold">
                                    Cardiology
                                </h5>

                                <p class="card-text text-secondary">
                                    Specialised care for your heart
                                    and cardiovascular health.
                                </p>

                            </div>

                        </div>

                    </div>

                    <div class="col-md-6 col-lg-4">

                        <div class="card service-card h-100">

                            <div class="card-body text-center p-4">

                                <h5 class="card-title fw-bold">
                                    Family Care
                                </h5>

                                <p class="card-text text-secondary">
                                    Healthcare services for every
                                    member of your family.
                                </p>

                            </div>

                        </div>

                    </div>

                </div>

            </div>

        </section>

        <footer class="main-footer py-4" id="contact">

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
