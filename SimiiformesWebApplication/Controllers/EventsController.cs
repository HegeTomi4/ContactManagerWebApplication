using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SimiiformesWebApplication.Data;
using SimiiformesWebApplication.Models;
using SimiiformesWebApplication.ViewModels;

namespace SimiiformesWebApplication.Controllers
{
    [Authorize(Roles = $"{nameof(Role.Administrator)},{nameof(Role.Manager)}")]
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;            
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Events.Include(e => e.Location);
            return View(await applicationDbContext.ToListAsync());
            //return _context.Events != null ?
            //            View(await _context.Events.ToListAsync()) :
            //              Problem("Entity set 'ApplicationDbContext.Events'  is null.");
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Include(e => e.Location)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        //
        // GET: Events/Create
        public IActionResult Create()
        {
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Id");            
            ViewBag.LocationIdName = _context.Locations!.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = $"{x.PostalCode} {x.City}, {x.Street} {x.HouseNumber}" });
            
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Date,LocationId")] Event @event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Id", @event.LocationId);
            return View(@event); 
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            
            ViewBag.LocationIdName = _context.Locations!.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = $"{x.PostalCode} {x.City}, {x.Street} {x.HouseNumber}" });
            ViewBag.People = _context.Person!.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = $"{x.Name}" });

            var eventViewModel = new EventViewModel
            {
                Id = @event.Id,
                Name = @event.Name,
                Date = @event.Date,
                LocationId = @event.LocationId,
                Guests = @event.Guests?.Select(x => x.Id).ToArray()
            };

            return View(eventViewModel);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Date,LocationId,Guests")] EventViewModel eventViewModel)
        {
            if (id != eventViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var entity = await _context.Events
                        .Include(x => x.Guests)
                        .FirstOrDefaultAsync(x => x.Id == eventViewModel.Id);

                    if (entity == null)
                    {
                        return NotFound();
                    }

                    entity.Name = eventViewModel.Name;
                    entity.Date = eventViewModel.Date;
                    entity.LocationId = eventViewModel.LocationId;
                    entity.Guests = new List<EventPersonConnection>();

                    foreach (var guest in eventViewModel.Guests)
                    {
                        var eventPersonConnection = new EventPersonConnection
                        {
                            EventId = entity.Id,
                            PersonId = guest
                        };

                        entity.Guests.Add(eventPersonConnection);
                    }
                    
                    _context.Update(entity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(eventViewModel.Id))
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
            
            return View(eventViewModel);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Include(e => e.Location)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Events == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Events'  is null.");
            }
            var @event = await _context.Events.FindAsync(id);
            if (@event != null)
            {
                _context.Events.Remove(@event);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
          return (_context.Events?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
