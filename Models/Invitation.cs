using System.ComponentModel.DataAnnotations;

namespace MicrosoftAssignment2.Models
{
    public class Invitation
    {
        //--invitation objects--\\
        public int Id { get; set; }

        [Required]
        public string GuestName { get; set; }

        [Required]
        [EmailAddress]
        public string GuestEmail { get; set; }

        [Required]
        public InvitationStatus Status { get; set; } = InvitationStatus.InviteNotSent;

        public int PartyId { get; set; }
        public Party Party { get; set; }
    }
    //--invitation status enum--\\
    public enum InvitationStatus
    {
        [Display(Name = "Invite not sent")]
        InviteNotSent,

        [Display(Name = "Invite sent")]
        InviteSent,

        [Display(Name = "Responded Yes")]
        RespondedYes,

        [Display(Name = "Responded No")]
        RespondedNo
    }
}