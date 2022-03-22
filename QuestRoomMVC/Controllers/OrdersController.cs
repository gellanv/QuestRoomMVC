using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuestRoomMVC.Data;
using QuestRoomMVC.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestRoomMVC.Controllers
{
    public class OrdersController : Controller
    {
        private readonly QuestRoomMVCContext _context;
        private readonly UserManager<User> _userManager;
        public static List<string> timeList = new List<string> { "12:00", "13:00", "14:00", "15:00", "16:00", "17:00", "18:00", "19:00", "20:00", "21:00", "22:00" };
        public OrdersController(QuestRoomMVCContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Orders
        [Authorize(Roles = "admin, client")]
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("admin"))
            {
                var questRoomMVCContext = _context.Orders.Include(o => o._user).Include(o => o.Room);
                return View(await questRoomMVCContext.ToListAsync());
            }
            else
            {
                var _user = await _userManager.FindByNameAsync(User.Identity.Name);
                var questRoomMVCContext = _context.Orders.Include(o => o._user).Where(x => x.UserId == _user.Id).Include(o => o.Room);
                return View(await questRoomMVCContext.ToListAsync());
            }
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o._user)
                .Include(o => o.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create(int roomid)
        {
            Room room = _context.Rooms.FirstOrDefault(x => x.Id == roomid);
            ViewBag.ImgRoom = room.Image;
            ViewBag.NameRoom = room.Name;
            ViewBag.RoomId = roomid;
            ViewBag.timeList = new SelectList(timeList);
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DateOrder,TimeOrder")] Order order, int roomId)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            order.UserId = user.Id;
            order.RoomId = roomId;
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", order.UserId);
            ViewData["RoomId"] = new SelectList(_context.Set<Room>(), "Id", "Name", order.RoomId);
            ViewBag.timeList = new SelectList(timeList);
            return View(order);
        }

        // POST: Orders/Edit/5       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateOrder,TimeOrder,UserId,RoomId")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (order.UserId == null)
                    {
                        Order _order = _context.Orders.FirstOrDefault(x => x.Id == order.Id);
                        _order.DateOrder = order.DateOrder;
                        _order.TimeOrder = order.TimeOrder;
                        _context.Update(_order);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        _context.Update(order);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name", order.UserId);
            ViewData["RoomId"] = new SelectList(_context.Set<Room>(), "Id", "Image", order.RoomId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o._user)
                .Include(o => o.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
