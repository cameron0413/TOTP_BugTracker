using Microsoft.AspNetCore.Mvc.Rendering;

namespace TOTP_BugTracker.Models.ViewModels

{
    public class AssignTicketDevViewModel
    {
        public Ticket? Ticket { get; set; }
        public SelectList? DevList { get; set; }
        public string? DevId { get; set; }

    }
}
