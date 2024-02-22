using System.ComponentModel.DataAnnotations;

namespace MicrosoftAssignment2.Models
{
    public class Party
    {
        //--invitation objects--\\
        public int Id { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        public string Location { get; set; }

        //--collection of invitations--\\
        public ICollection<Invitation>? Invitations { get; set; }
    }
}