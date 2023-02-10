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
        public TaskController()
        {
        }

        [HttpPost]
        public IActionResult Get()
        {
            using var context = new BMContext();
            var task = new Models.Task();
            context.Tasks.Add(task);
            context.SaveChanges();
            TaskRunning(task.Guid);

            return Accepted(task.Guid);
        }

        async void TaskRunning(Guid guid)
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
        public IActionResult Get(string id)
        {
            if (Guid.TryParse(id, out var taskGuid))
            {
                using var context = new BMContext();
                var task = context.Tasks.FirstOrDefault(x => x.Guid == taskGuid);
                if (task != null)
                {
                    return Ok(JsonConvert.SerializeObject(task));
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
