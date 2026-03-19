using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using EventManagement.Data;
using EventManagement.Models;

namespace EventManagement.Controllers
{
    // only logged in users can manage events
    [Authorize]
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public EventsController(ApplicationDbContext db)
        {
            _db = db;
        }

        // show all events
        public async Task<IActionResult> Index()
        {
            var events = await _db.Events.ToListAsync();
            return View(events);
        }

        // show add event form
        public IActionResult Create()
        {
            return View();
        }

        // save new event to database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event evt)
        {
            if (ModelState.IsValid)
            {
                // get current logged in user id
                evt.UserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                _db.Events.Add(evt);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(evt);
        }

        // show edit form with existing event data
        public async Task<IActionResult> Edit(int id)
        {
            var evt = await _db.Events.FindAsync(id);
            if (evt == null) return NotFound();
            return View(evt);
        }

        // save edited event
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Event evt)
        {
            if (id != evt.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(evt);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_db.Events.Any(e => e.Id == id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(evt);
        }

        // show delete confirmation page
        public async Task<IActionResult> Delete(int id)
        {
            var evt = await _db.Events.FindAsync(id);
            if (evt == null) return NotFound();
            return View(evt);
        }

        // delete event from database
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var evt = await _db.Events.FindAsync(id);
            if (evt != null)
            {
                _db.Events.Remove(evt);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}