using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MicrosoftAssignment2.Data;
using MicrosoftAssignment2.Models;
using MicrosoftAssignment2.Models.Services;

namespace MicrosoftAssignment2.Controllers
{
    //--controller for managing parties--\\
    public class PartiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMailDispatcher _emailService;

        //--constructor--\\
        public PartiesController(ApplicationDbContext context, IMailDispatcher emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        //--display the list of parties--\\
        public IActionResult Index()
        {
            //--gets all parties including their invitations--\\
            var parties = _context.Parties.Include(p => p.Invitations).ToList();
            return View(parties);
        }

        //--display details for a single party--\\
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //--gets a party including its invitations--\\
            var party = await _context.Parties.Include(p => p.Invitations).FirstOrDefaultAsync(m => m.Id == id);
            return party == null ? NotFound() : View(party);
        }


        //--return the view for adding a new party--\\
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Action = "Add";
            return View("Edit", new Party());
        }

        //--return the view for editing an existing party--\\
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var party = _context.Parties.Find(id);
            if (party == null)
            {
                return NotFound();
            }
            ViewBag.Action = "Edit";
            return View("Edit", party);
        }

        //--add a new party to the database--\\
        [HttpPost]
        public IActionResult Add(Party party)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Action = "Add";
                return View("Edit", party);
            }

            _context.Parties.Add(party);
            _context.SaveChanges();
            return RedirectToAction("Details", new { id = party.Id });
        }

        //--update an existing party in the database--\\
        [HttpPost]
        public IActionResult Edit(Party party)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Action = "Edit";
                return View(party);
            }

            if (party.Id != 0)
            {
                _context.Parties.Update(party);
                _context.SaveChanges();
                return RedirectToAction("Details", new { id = party.Id });
            }
            return RedirectToAction(nameof(Add));
        }

        //--create an invitation for a party--\\
        [HttpPost]
        public async Task<IActionResult> CreateInvitation(int partyId, string guestName, string guestEmail)
        {
            var party = await _context.Parties.Include(p => p.Invitations).FirstOrDefaultAsync(p => p.Id == partyId);
            if (party == null)
            {
                return NotFound();
            }

            var invitation = new Invitation
            {
                PartyId = partyId,
                GuestName = guestName,
                GuestEmail = guestEmail,
                Status = InvitationStatus.InviteNotSent
            };

            _context.Invitations.Add(invitation);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = partyId });
        }

        //--send out invitations for a party--\\
        [HttpPost]
        public async Task<IActionResult> SendInvitations(int partyId)
        {
            var party = await _context.Parties.Include(p => p.Invitations).SingleOrDefaultAsync(p => p.Id == partyId);
            if (party == null)
            {
                return NotFound();
            }
            var baseUrl = $"{Request.Scheme}://{Request.Host.Value}{Request.PathBase}";
            foreach (var invitation in party.Invitations)
            {
                //--construct the URL for the invitation response page--\\
                string responseUrl = Url.Action("Respond", "Invitations", new { invitationId = invitation.Id }, protocol: null, host: null);

                string fullUrl = $"{baseUrl}{responseUrl}";

                string body = $@"
            <html>
                <body>
                    <p>Hello {invitation.GuestName},</p>
                    <p>You are invited to the '{party.Description}' at {party.Location} on {party.EventDate:d}.</p>
                    <p>We would be thrilled to have you join us! Please <a href='{fullUrl}'>let us know</a> if you can attend as soon as possible.</p>
                    <p>Sincerely,</p>
                    <p>The Party Manager App</p>
                </body>
            </html>";

                //--ppdate the invitation status--\\
                invitation.Status = InvitationStatus.InviteSent;

                //--send the email--\\
                await _emailService.DispatchMailAsync(invitation.GuestEmail, "Invitation to " + party.Description, body);
            }

            await _context.SaveChangesAsync();
            TempData["Message"] = "Invitations sent successfully.";
            return RedirectToAction(nameof(Details), new { id = partyId });
        }
    }
}
