using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimiiformesWebApplication.Data;
using SimiiformesWebApplication.Models;

namespace SimiiformesWebApplication.Controllers
{
    public class PeopleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PeopleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: People
        public async Task<IActionResult> Index()
        {
            return _context.Person != null ?
                        View(await _context.Person.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Person'  is null.");
        }

        // GET: People/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return _context.Person != null ?
                        View("ShowSearchForm") :
                        Problem("Entity set 'ApplicationDbContext.Person'  is null.");
        }

        // GET: People/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            return _context.Person != null ?
                        View("Index", await _context.Person.Where(j => j.Name.Contains(SearchPhrase)).ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Person'  is null.");
        }

        // GET: People/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Person == null)
            {
                return NotFound();
            }

            var person = await _context.Person
                .Include(p => p.Histories)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        //Delete wrong history
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteHistory(int id)
        {
            var history = await _context.Histories.FindAsync(id);

            if (history == null)
            {
                return NotFound();
            }

            _context.Histories.Remove(history);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = history.PersonId });
        }


        // GET: People/Create

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,PhoneNumber,Gender,Company,Position,Notes,ImagePath,ImageFile")] Person person)
        {
            if (ModelState.IsValid)
            {
                if (person.ImageFile != null && person.ImageFile.Length > 0)
                {
                    // Generate a unique file name for the uploaded image
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(person.ImageFile.FileName);
                    var filePath = Path.Combine("wwwroot", "Sources", "PeopleImages", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await person.ImageFile.CopyToAsync(stream);
                    }
                    person.ImagePath = "/Sources/PeopleImages/" + fileName;
                }
                else
                {
                    person.ImagePath = "/Sources/PeopleImages/Default.jpg";
                }
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }

        // GET: People/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null || _context.Person == null)
            {
                return NotFound();
            }

            var person = await _context.Person.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,PhoneNumber,Gender,Company,Position,Notes,ImageFile")] Person person)
        {
            if (id != person.Id)
            {
                return NotFound();
            }
            var oldImagePath = person.ImagePath;


            if (ModelState.IsValid)
            {
                try
                {
                    if (this._context.Person != null)
                    {
                        //History tracking
                        var oldPerson = await _context.Person.AsNoTracking().SingleOrDefaultAsync(p => p.Id == id);
                        var oldCompany = oldPerson.Company;
                        var oldPosition = oldPerson.Position;

                        // Update the person entity with the new values
                        _context.Update(person);

                        // Set ImagePath before update
                        person.ImagePath = oldPerson.ImagePath;

                        await _context.SaveChangesAsync();

                        // Check if Company or Position have changed and save the old values to the History table
                        if (oldCompany != person.Company || oldPosition != person.Position)
                        {
                            var history = new History
                            {
                                PersonId = person.Id,
                                Company = oldCompany,
                                Position = oldPosition,
                            };

                            _context.Histories.Add(history);
                            await _context.SaveChangesAsync();
                        }

                        // Set oldImagePath
                        oldImagePath = oldPerson.ImagePath;
                    }

                    if (person.ImageFile != null)
                    {
                        // Delete old image                        
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }

                        // Save new image
                        var newImagePath = Path.Combine("wwwroot", "Sources", "PeopleImages", person.ImageFile.FileName);
                        using (var stream = new FileStream(newImagePath, FileMode.Create))
                        {
                            await person.ImageFile.CopyToAsync(stream);
                        }
                        person.ImagePath = "/Sources/PeopleImages/" + person.ImageFile.FileName;
                    }

                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.Id))
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

            return View(person);

        }

        // GET: People/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Person == null)
            {
                return NotFound();
            }

            var person = await _context.Person
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: People/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Person == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Person'  is null.");
            }
            var person = await _context.Person.FindAsync(id);
            if (person != null)
            {
                _context.Person.Remove(person);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(int id)
        {
            return (_context.Person?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
