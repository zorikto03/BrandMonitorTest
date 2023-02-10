using BrandMonitor.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BrandMonitor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : Controller
    {
        private readonly BMContext _context;
        public TaskController(BMContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            using (var context = new BMContext())
            {
                var task = new Models.Task();
                _context.Tasks.Add(task);
                _context.SaveChanges();
                TaskRunning(task.Guid);

                return Accepted(task.Guid);
            }
        }

        async void TaskRunning(int guid)
        {
            using (var context = new BMContext())
            {
                var task = context.Tasks.FirstOrDefault(x => x.Guid == guid);
                if (task != null)
                {
                    task.SetStatus(Models.Task.running);
                    context.Tasks.Update(task);
                    context.SaveChanges();
                    await System.Threading.Tasks.Task.Delay(120000);

                    task.SetStatus(Models.Task.finished);
                    context.Tasks.Update(task);
                    context.SaveChanges();
                }
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var task = _context.Tasks.FirstOrDefault(x => x.Guid == id);
            if (task != null)
            {
                return Ok(JsonConvert.SerializeObject(task));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

       
        // POST: TaskController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
