using System.ComponentModel.DataAnnotations;

namespace TaskManagerApp.Models
{
    public class AddTaskModels
    {
        public Guid ListId { get; set; }
        public string Task { get; set; }
    }
}
