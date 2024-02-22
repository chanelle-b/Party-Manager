using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MicrosoftAssignment2.Data;
using MicrosoftAssignment2.Models;

namespace MicrosoftAssignment2.Controllers
{
    public class InvitationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        //--initializes the context used for database operations--\\
        public InvitationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        //--display a list of all invitations--\\
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Invitations.Include(i => i.Party);
            return View(await applicationDbContext.ToListAsync());
        }

        //--display the response form for a specific invitation--\\
        [HttpGet]
        [Route("Invitations/Respond/{invitationId}")]
        public async Task<IActionResult> Respond(int invitationId)
        {
            //--retrieve the invitation with the related party details--\\
            var invitation = await _context.Invitations
                .Include(i => i.Party)
                .FirstOrDefaultAsync(i => i.Id == invitationId);

            if (invitation == null || invitation.Party == null)
            {
                //--if invitation or party doesn't exist display a not found message--\\
                return NotFound("The invitation or party details are not available.");
            }


            return View("Invitation", invitation);
        }

        //--process the response to an invitation--\\
        [HttpPost]
        public async Task<IActionResult> SubmitResponse(int id, string response)
        {
            var invitation = await _context.Invitations.FindAsync(id);
            if (invitation != null)
            {
                //--update status based on the response--\\
                invitation.Status = response.Equals("yes", StringComparison.OrdinalIgnoreCase)
                                   ? InvitationStatus.RespondedYes
                                   : InvitationStatus.RespondedNo;
                //--save invitation status--\\
                await _context.SaveChangesAsync();

                //--redirect to thank you page --\\
                return RedirectToAction("ThankYou");
            }
            //--not found message if invitation cant be found--\\
            return NotFound();
        }

        //--display thanks for response--\\
        public IActionResult ThankYou()
        {
            return View("ThanksPage");
        }

    }
}
