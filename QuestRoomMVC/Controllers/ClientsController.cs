using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestRoomMVC.Data;
using QuestRoomMVC.Models;
using QuestRoomMVC.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace QuestRoomMVC.Controllers
{
    public class ClientsController : Controller
    {
        private readonly QuestRoomMVCContext _context;
        private readonly UserManager<User> _userManager;
        public ClientsController(QuestRoomMVCContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Clients
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Clients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string? id, [Bind("Id,UserName,Email,PhoneNumber")] RegisterViewModel user)
        {
            User _user = _context.Users.FirstOrDefault(x => x.Id == id);
            if (_user == null)
            {
                return NotFound();
            }
            try
            {
                _user.UserName = user.UserName;
                _user.Email = user.Email;
                _user.PhoneNumber = user.PhoneNumber;

                _context.Update(_user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();

            }
            return RedirectToAction(nameof(Index));

        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            ViewData["Message"] = "";
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string? id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            Order _order = await _context.Orders.FirstOrDefaultAsync(x => x.UserId == id);
            if (_order != null)
            {
                ViewData["Message"] = "Удалить клиента нет возможности, уже имеются оформленные заказы";
                return View(user);
            }
            else
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
