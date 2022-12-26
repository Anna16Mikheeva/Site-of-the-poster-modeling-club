using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using static WebApplication1.Models.Events;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using WebApplication1.ViewModel;
using WebApplication1.Data.Migrations;

namespace WebApplication1.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext Context;
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private static string _imageCopy;
        private static string _imageEvent;
        private static string _nameEvent;
        private static string _descriptionEvent;
        private static string _dateEvent;
        private static string _signUpEvent;
        public EventsController(ILogger<EventsController> logger, ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            Context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            var items = Context.Events.ToList();
            return View(items);
        }


        // GET: Events/Details/5
        public async Task<IActionResult> But(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var events = await Context.Events.FindAsync(id);
            _imageEvent = events.ImageEvent;
            _nameEvent = events.NameEvent;
            _descriptionEvent = events.DescriptionEvent;
            _dateEvent = events.DateEvent;
            _signUpEvent = events.SignUpEvent;
            But(events);
            if (events == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> But(Events events)
        {
            events.SignUpEvent += User.Identity.Name + " \n";
            Context.Update(events);
            await Context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventsViewModel vm)/*Create([Bind("Id,ImageEvent,DescriptionEvent")] Events events)*/
        {
            //if (ModelState.IsValid)
            //{
            //    _context.Add(events);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(events);
            string stringFileName = UploadFile(vm);
            var events = new Events
            {
                ImageEvent = stringFileName,
                NameEvent = vm.NameEvent,
                DescriptionEvent = vm.DescriptionEvent,
                DateEvent = vm.DateEvent,
                SignUpEvent = vm.SignUpEvent
            };
            //_imageCopy = events.ImageEvent;
            Context.Events.Add(events);
            await Context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //!
        private string UploadFile(EventsViewModel vm)
        {
            string fileName = null;
            if (vm.ImageEvent != null)
            {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "Img");
                fileName = Guid.NewGuid().ToString() + "-" + vm.ImageEvent.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    vm.ImageEvent.CopyTo(fileStream);
                }
            }
            return fileName;
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var events = await Context.Events.FindAsync(id);
            _imageCopy = events.ImageEvent;
            if (events == null)
            {
                return NotFound();
            }
            return View(events);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ImageEvent,NameEvent,DescriptionEvent,DateEvent")] Events events, EventsViewModel vm)
        {
            
            if (id != events.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(events.ImageEvent == null && _imageCopy != null)
                    {
                        events.ImageEvent = _imageCopy;
                    }
                    else
                    {
                        string stringFileName = UploadFile(vm);
                        events.ImageEvent = stringFileName;
                    }
                    Context.Update(events);
                    await Context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventsExists(events.Id))
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
            return View(events);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var events = await Context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (events == null)
            {
                return NotFound();
            }

            return View(events);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var events = await Context.Events.FindAsync(id);
            Context.Events.Remove(events);
            await Context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventsExists(int id)
        {
            return Context.Events.Any(e => e.Id == id);
        }
    }
}
