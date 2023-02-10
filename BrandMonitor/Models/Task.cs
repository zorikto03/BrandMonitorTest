using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrandMonitor.Models
{
    public class Task
    {
        public const string created = "created";
        public const string finished = "finished";
        public const string running = "running";

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public Guid Guid { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }

        public Task()
        {
            Status = created;
            Timestamp = DateTime.UtcNow;
        }

        public void SetStatus(string status)
        {
            Status = status;
            Timestamp = DateTime.UtcNow;
        }
    }
}
