﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TOTP_BugTracker.Models
{
    public class Invite
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Invitation Date")]
        public DateTime InviteDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Join Date")]
        public DateTime JoinDate { get; set; }
        public Guid CompanyToken { get; set; }
        public int CompanyId { get; set; }
        public int ProjectId { get; set; }

        [Required]
        public string? InvitorId { get; set; }
        public string? InviteeId { get; set; }

        [Required]
        [DisplayName("Invitee Email")]
        public string? InviteeEmail { get; set; }

        [Required]
        [DisplayName("Invitee First Name")]
        public string? InviteeFirstName { get; set; }

        [Required]
        [DisplayName("Invitee Last Name")]
        public string? InviteeLastName { get; set; }
        public string? Message { get; set; }
        public bool IsValid { get; set; }

        // Navigation Properties

        public virtual Company? Company { get; set; }
        public virtual Project? Project { get; set; }
        public virtual BTUser? Invitor { get; set; }
        public virtual BTUser? Invitee { get; set; }


    }
}
