using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Buisness.ViewModels
{
    public class TaskModel: DomainModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }
        public Task_Status TaskStatus { get; set; }
        public UserModel User { get; set; }
        public bool Status { get; set; }
    }
    public enum Task_Status
    {
        All = 0,
        Completed = 1,
        Incomplete = 2
    }
}
