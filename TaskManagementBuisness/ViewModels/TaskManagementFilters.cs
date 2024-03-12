using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Buisness.ViewModels
{
    public class TaskManagementFilters
    {
        public Task_Status Status { get; set; }
        public UserModel User { get; set; }
    }
}
