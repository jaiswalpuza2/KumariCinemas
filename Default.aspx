<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.Master"
AutoEventWireup="true" CodeBehind="Default.aspx.cs"
Inherits="MovieTicketSystem.Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<style>
   
    .cinema-hero {
        background: linear-gradient(135deg, rgba(79, 70, 229, 0.95), rgba(124, 58, 235, 0.95)), url('Content/cinema_hero_bg.png');
        background-size: cover;
        background-position: center;
        padding: 6rem 4rem;
        border-radius: 30px;
        color: white;
        margin-bottom: 3.5rem;
        position: relative;
        overflow: hidden;
        box-shadow: 0 25px 50px -12px rgba(79, 70, 229, 0.25);
    }

    .hero-tag {
        background: rgba(255, 255, 255, 0.2);
        backdrop-filter: blur(10px);
        color: white;
        padding: 0.6rem 1.4rem;
        border-radius: 50px;
        font-weight: 700;
        font-size: 0.75rem;
        text-transform: uppercase;
        letter-spacing: 1.5px;
        margin-bottom: 2rem;
        display: inline-block;
        border: 1px solid rgba(255, 255, 255, 0.3);
    }

    .hero-title {
        font-size: 4rem;
        font-weight: 800;
        line-height: 1.1;
        margin-bottom: 1.5rem;
        letter-spacing: -2px;
        text-shadow: 0 4px 10px rgba(0,0,0,0.1);
    }

    .hero-subtext {
        font-size: 1.25rem;
        color: #ffffff !important;
        max-width: 600px;
        margin-bottom: 2.5rem;
        line-height: 1.6;
    }

   
    .menu-grid {
        display: grid;
        grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
        gap: 1.5rem;
        margin-bottom: 5rem;
    }

    .menu-item {
        background: white;
        border-radius: 24px;
        padding: 2.5rem 1.5rem;
        text-align: center;
        text-decoration: none !important;
        transition: all 0.4s cubic-bezier(0.4, 0, 0.2, 1);
        border: 1px solid rgba(0, 0, 0, 0.05);
        box-shadow: 0 4px 6px rgba(0, 0, 0, 0.01);
    }

    .menu-item:hover {
        transform: translateY(-10px);
        box-shadow: 0 20px 25px -5px rgba(0, 0, 0, 0.05);
        border-color: var(--accent-primary);
    }

    .menu-icon {
        width: 64px;
        height: 64px;
        background: rgba(79, 70, 229, 0.06);
        color: var(--accent-primary);
        border-radius: 20px;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 1.75rem;
        margin: 0 auto 1.5rem;
        transition: all 0.3s ease;
    }

    .menu-item:hover .menu-icon {
        background: var(--accent-primary);
        color: white;
        transform: rotate(-5deg) scale(1.1);
    }

    .menu-label {
        color: var(--text-primary);
        font-weight: 700;
        font-size: 1.1rem;
        display: block;
    }

 
    .dashboard-panel {
        background: white;
        border-radius: 32px;
        padding: 3.5rem;
        border: 1px solid rgba(0, 0, 0, 0.05);
        box-shadow: 0 10px 30px rgba(0, 0, 0, 0.02);
    }

    .stat-card {
        padding: 1.5rem;
        border-radius: 20px;
        background: #f8fafc;
        border: 1px solid #f1f5f9;
        height: 100%;
        transition: all 0.3s ease;
    }

    .stat-card:hover {
        background: white;
        border-color: var(--accent-primary);
        box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.05);
    }

    .stat-card .label {
        font-size: 0.75rem;
        font-weight: 700;
        text-transform: uppercase;
        letter-spacing: 1px;
        color: var(--text-secondary);
        margin-bottom: 0.5rem;
        display: block;
    }

    .stat-card .value {
        font-size: 2rem;
        font-weight: 800;
        color: var(--text-primary);
    }


    .booking-stats {
        display: flex;
        align-items: flex-end;
        height: 250px;
        gap: 15px;
        margin-top: 2rem;
        padding: 0 2rem;
    }

    .stat-bar-group {
        flex: 1;
        display: flex;
        flex-direction: column;
        align-items: center;
        gap: 12px;
    }

    .stat-bar {
        width: 100%;
        background: linear-gradient(to top, var(--accent-primary), var(--accent-secondary));
        border-radius: 8px 8px 4px 4px;
        transition: height 1.5s cubic-bezier(0.34, 1.56, 0.64, 1);
        min-height: 10px;
        box-shadow: 0 4px 12px rgba(79, 70, 229, 0.15);
    }

    .day-label {
        font-size: 0.75rem;
        font-weight: 700;
        color: var(--text-secondary);
        text-transform: uppercase;
    }
</style>

<div class="cinema-hero">
    <div class="row align-items-center">
        <div class="col-lg-7">
            <span class="hero-tag"><i class="fas fa-play me-2"></i>Now Showing at Kumari</span>
            <h1 class="hero-title">Your Gateway to the <br/>Ultimate Cinema</h1>
            <p class="hero-subtext">Manage bookings, monitor hall performance, and organize showtimes for the finest movie destination in Nepal.</p>
            <div class="d-flex gap-3">
                <% if (Session["UserRole"]?.ToString() == "Admin") { %>
                    <a href="ManageTickets" class="btn btn-primary btn-lg px-5">Instant Booking</a>
                    <a href="ManageMovies" class="btn btn-outline-light btn-lg px-5 border-2 fw-bold">Our Catalog</a>
                <% } else if (Session["UserRole"]?.ToString() == "Customer") { %>
                    <a href="UserTicketReport" class="btn btn-primary btn-lg px-5">View My Tickets</a>
                <% } %>
            </div>
        </div>
    </div>
</div>

<% if (Session["UserRole"]?.ToString() == "Admin") { %>
<div class="mb-5">
    <h4 class="fw-bold mb-4 d-flex align-items-center"><i class="fas fa-th-large me-2 text-primary"></i> Management Options</h4>
    <div class="menu-grid">
        <a href="ManageMovies" class="menu-item">
            <div class="menu-icon"><i class="fas fa-film"></i></div>
            <span class="menu-label">Movies</span>
        </a>
        <a href="ManageTheaters" class="menu-item">
            <div class="menu-icon"><i class="fas fa-couch"></i></div>
            <span class="menu-label">Theaters</span>
        </a>
        <a href="ManageShowtimes" class="menu-item">
            <div class="menu-icon"><i class="fas fa-clock"></i></div>
            <span class="menu-label">Showtimes</span>
        </a>
        <a href="ManageTickets" class="menu-item">
            <div class="menu-icon"><i class="fas fa-ticket-alt"></i></div>
            <span class="menu-label">Tickets</span>
        </a>
        <a href="UserTicketReport" class="menu-item">
            <div class="menu-icon"><i class="fas fa-chart-line"></i></div>
            <span class="menu-label">Reports</span>
        </a>
    </div>
</div>
<% } else { %>
<div class="mb-5">
    <h4 class="fw-bold mb-4 d-flex align-items-center"><i class="fas fa-user-circle me-2 text-primary"></i> Customer Portal</h4>
    <div class="menu-grid">
        <a href="UserTicketReport" class="menu-item">
            <div class="menu-icon"><i class="fas fa-history"></i></div>
            <span class="menu-label">My Bookings</span>
        </a>
        <a href="Contact" class="menu-item">
            <div class="menu-icon"><i class="fas fa-info-circle"></i></div>
            <span class="menu-label">Support</span>
        </a>
    </div>
</div>
<% } %>

<div class="dashboard-panel mb-5">
    <div class="d-flex justify-content-between align-items-center mb-5">
        <div>
            <h3 class="fw-extrabold mb-1">Live Intelligence</h3>
            <p class="text-secondary mb-0">Real-time performance metrics</p>
        </div>
        <div class="badge bg-success bg-opacity-10 text-success p-2 px-3 rounded-pill border border-success border-opacity-25">
            <i class="fas fa-circle me-1 small"></i> System Active
        </div>
    </div>

    <div class="row g-4 mb-5">
        <div class="col-md-4">
            <div class="stat-card">
                <span class="label">Total Sales</span>
                <div class="value">$<%= TodaySales %></div>
            </div>
        </div>
        <div class="col-md-4">
            <div class="stat-card">
                <span class="label">Active Movies</span>
                <div class="value"><%= TotalMovies %></div>
            </div>
        </div>
        <div class="col-md-4">
            <div class="stat-card">
                <span class="label">Avg Occupancy</span>
                <div class="value"><%= OccupancyRate %></div>
            </div>
        </div>
    </div>

    <h6 class="text-secondary fw-bold text-uppercase mb-4" style="letter-spacing: 1px;">Weekly Booking Trends</h6>
    <div class="booking-stats">
        <div class="stat-bar-group">
            <div class="stat-bar" style="height: 60%;"></div>
            <span class="day-label">Mon</span>
        </div>
        <div class="stat-bar-group">
            <div class="stat-bar" style="height: 45%;"></div>
            <span class="day-label">Tue</span>
        </div>
        <div class="stat-bar-group">
            <div class="stat-bar" style="height: 80%;"></div>
            <span class="day-label">Wed</span>
        </div>
        <div class="stat-bar-group">
            <div class="stat-bar" style="height: 55%;"></div>
            <span class="day-label">Thu</span>
        </div>
        <div class="stat-bar-group">
            <div class="stat-bar" style="height: 95%;"></div>
            <span class="day-label">Fri</span>
        </div>
        <div class="stat-bar-group">
            <div class="stat-bar" style="height: 70%;"></div>
            <span class="day-label">Sat</span>
        </div>
        <div class="stat-bar-group">
            <div class="stat-bar" style="height: 85%;"></div>
            <span class="day-label">Sun</span>
        </div>
    </div>
</div>
</asp:Content>
