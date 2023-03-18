using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagerApp.Models.Domain
{
    [Table("Tasks")]
    public class Plan
    {
        [Key]
        public Guid Id { get; set; }
        public string Task { get; set; }
        public bool Status { get; set; }

        /* defining foregin key using annotation */
        [Display(Name = "TaskList")]
        public Guid ListId { get; set; }
        [ ForeignKey("ListId") ]
        public virtual TaskList TaskList { get; set; }
    }
}
