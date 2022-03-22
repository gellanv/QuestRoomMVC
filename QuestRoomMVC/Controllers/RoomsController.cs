using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestRoomMVC.Data;
using QuestRoomMVC.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QuestRoomMVC.Controllers
{
    public class RoomsController : Controller
    {
        public static string CountSort = "";
        public static string DifSort = "";

        public static string LastSort = "";

        private readonly QuestRoomMVCContext _context;
        public readonly IWebHostEnvironment _env;
        public RoomsController(QuestRoomMVCContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Rooms
        [Authorize(Roles = "admin, client")]
        public async Task<IActionResult> Index(int? countpl = -1, int? diflevel = -1, int? fearlevel = -1, string throwoff = null, string sortOrder = null, int page = 1)
        {

            if (countpl != -1 && diflevel != -1 && fearlevel != -1 && throwoff == null)
            {
                IQueryable<Room> rooms = _context.Rooms.Where(x => x.CountPlayers == countpl && ((int)x.DifficultLevel) == diflevel && ((int)x.FearLevel) == fearlevel);
                IndexViewModel viewModel = await PaginationMethod(rooms, page);
                return View(viewModel);

            }
            else if (sortOrder != null)
            {
                if (sortOrder == "count")
                {
                    CountSort = CountSort == "count" ? "countDes" : "count";
                    LastSort = CountSort;
                }
                else if (sortOrder == "diff")
                {
                    DifSort = DifSort == "diff" ? "diffDes" : "diff";
                    LastSort = DifSort;
                }
                IQueryable<Room> rooms = from s in _context.Rooms select s;

                switch (LastSort)
                {
                    case "count":
                        rooms = _context.Rooms.OrderByDescending(s => s.CountPlayers);
                        break;
                    case "countDes":
                        rooms = _context.Rooms.OrderBy(s => s.CountPlayers);
                        break;
                    case "diff":
                        rooms = _context.Rooms.OrderByDescending(s => s.DifficultLevel);
                        break;
                    case "diffDes":
                        rooms = _context.Rooms.OrderBy(s => s.DifficultLevel);
                        break;
                }
                IndexViewModel viewModel = await PaginationMethod(rooms, page);
                return View(viewModel);
            }
            else
            {
                IQueryable<Room> rooms = from s in _context.Rooms select s;
                IndexViewModel viewModel = await PaginationMethod(rooms, page);
                return View(viewModel);
            }
        }

        public async Task<IndexViewModel> PaginationMethod(IQueryable<Room> rooms, int page)
        {
            int pageSize = 4;
            var count = await rooms.CountAsync();
            var items = await rooms.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                Rooms = items
            };
            return viewModel;
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms.FirstOrDefaultAsync(m => m.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // GET: Rooms/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rooms/Create
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,CountPlayers,DifficultLevel,FearLevel")] Room room, IFormFile fileimg)
        {
            if (fileimg != null)
            {
                try
                {
                    var rootPath = _env.WebRootPath + "/";
                    string filePath = "img/" + fileimg.FileName;
                    using (var fileStream = new FileStream(rootPath + filePath, FileMode.Create))
                    {
                        await fileimg.CopyToAsync(fileStream);
                        room.Image = filePath;
                    }
                }
                catch
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            else
            {
                room.Image = "";
            }

            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Rooms/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }

        // POST: Rooms/Edit/5
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Image,Name,Description,CountPlayers,DifficultLevel,FearLevel")] Room room, IFormFile fileimg)
        {
            if (fileimg != null)
            {
                try
                {
                    var rootPath = _env.WebRootPath + '/';
                    string filePath = "img/" + fileimg.FileName;
                    using (var fileStream = new FileStream(rootPath + filePath, FileMode.Create))
                    {
                        await fileimg.CopyToAsync(fileStream);
                        room.Image = filePath;
                    }
                }
                catch
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            _context.Attach(room).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomExists(room.Id))
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

        // GET: Rooms/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms.FirstOrDefaultAsync(m => m.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Rooms/Delete/5
        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomExists(int id)
        {
            return _context.Rooms.Any(e => e.Id == id);
        }
    }
}
