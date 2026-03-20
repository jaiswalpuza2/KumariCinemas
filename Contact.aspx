<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="MovieTicketSystem.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row mb-5 align-items-center">
        <div class="col-12 text-center py-5">
            <div class="badge bg-primary bg-opacity-10 text-primary p-2 px-3 rounded-pill mb-3">Support Center</div>
            <h1 class="display-4 fw-800 mb-3">How can we help you today?</h1>
            <p class="text-secondary lead mx-auto" style="max-width: 600px;">Have a question about your booking or a screening? Our team is here to ensure your cinematic experience at Kumari Cinema is flawless.</p>
        </div>
    </div>

    <div class="row g-4 mb-5">
        <div class="col-md-4">
            <div class="gridview-container text-center h-100">
                <div class="menu-icon mb-4" style="width: 60px; height: 60px; font-size: 1.5rem; margin: 0 auto; background: rgba(79, 70, 229, 0.1); color: #4f46e5; border-radius: 15px; display: flex; align-items: center; justify-content: center;">
                    <i class="fas fa-phone-alt"></i>
                </div>
                <h5 class="fw-bold mb-2">Call Us</h5>
                <p class="text-secondary small mb-3">Available daily from 10 AM to 10 PM</p>
                <div class="fw-bold text-primary fs-5">+977-1-4XXXXXX</div>
            </div>
        </div>
        <div class="col-md-4">
            <div class="gridview-container text-center h-100">
                <div class="menu-icon mb-4" style="width: 60px; height: 60px; font-size: 1.5rem; margin: 0 auto; background: rgba(124, 58, 235, 0.1); color: #7c3aed; border-radius: 15px; display: flex; align-items: center; justify-content: center;">
                    <i class="fas fa-envelope"></i>
                </div>
                <h5 class="fw-bold mb-2">Email Support</h5>
                <p class="text-secondary small mb-3">We'll respond within 24 hours</p>
                <div class="fw-bold text-primary fs-5">support@kumaricinema.com</div>
            </div>
        </div>
        <div class="col-md-4">
            <div class="gridview-container text-center h-100">
                <div class="menu-icon mb-4" style="width: 60px; height: 60px; font-size: 1.5rem; margin: 0 auto; background: rgba(34, 197, 94, 0.1); color: #22c55e; border-radius: 15px; display: flex; align-items: center; justify-content: center;">
                    <i class="fas fa-map-marker-alt"></i>
                </div>
                <h5 class="fw-bold mb-2">Visit Us</h5>
                <p class="text-secondary small mb-3">Located in the heart of Kathmandu</p>
                <div class="fw-bold text-primary fs-5">Kumari Plaza, Kamalpokhari</div>
            </div>
        </div>
    </div>

    <div class="gridview-container border-primary border-opacity-10 py-5">
        <div class="row align-items-center">
            <div class="col-lg-6 px-lg-5 mb-5 mb-lg-0">
                <h2 class="fw-bold mb-4">Frequently Asked Questions</h2>
                <div class="accordion accordion-flush" id="faqAccordion">
                    <div class="accordion-item bg-transparent border-bottom">
                      <h2 class="accordion-header">
                        <button class="accordion-button collapsed bg-transparent fw-600 py-3" type="button" data-bs-toggle="collapse" data-bs-target="#faq1">
                          How do I cancel my booking?
                        </button>
                      </h2>
                      <div id="faq1" class="accordion-collapse collapse" data-bs-parent="#faqAccordion">
                        <div class="accordion-body text-secondary">
                          Bookings can be cancelled up to 2 hours before the showtime directly through your "My Bookings" portal.
                        </div>
                      </div>
                    </div>
                    <div class="accordion-item bg-transparent border-bottom">
                      <h2 class="accordion-header">
                        <button class="accordion-button collapsed bg-transparent fw-600 py-3" type="button" data-bs-toggle="collapse" data-bs-target="#faq2">
                          Do you offer student discounts?
                        </button>
                      </h2>
                      <div id="faq2" class="accordion-collapse collapse" data-bs-parent="#faqAccordion">
                        <div class="accordion-body text-secondary">
                          Yes! Present a valid student ID at the counter to receive 20% off on weekday screenings.
                        </div>
                      </div>
                    </div>
                </div>
            </div>
            <div class="col-lg-6 px-lg-5">
                <div class="p-4 rounded-4 bg-light border border-white">
                    <h4 class="fw-bold mb-4">Quick Query</h4>
                    <div class="mb-3">
                        <label class="form-label">Name</label>
                        <input type="text" class="form-control" placeholder="Your Name">
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Message</label>
                        <textarea class="form-control" rows="4" placeholder="How can we help?"></textarea>
                    </div>
                    <button type="button" class="btn btn-primary w-100 py-3 mt-2" onclick="alert('Thank you! Your query has been received.')">Send Message</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
