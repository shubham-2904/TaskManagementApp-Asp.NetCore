using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagerApp.Models.Domain
{
    [Table("Lists")]
    public class TaskList
    {
        [Key]
        public Guid Id { get; set; }
        public string ListTitle { get; set;}

        public ICollection<Plan> Plan { get; set;}

        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User{ get; set; }

    }
}
