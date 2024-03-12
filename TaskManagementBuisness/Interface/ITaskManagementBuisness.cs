using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Buisness.ViewModels;

namespace TaskManagement.Buisness.Interface
{
    public interface ITaskManagementBuisness
    {
        Task<List<TaskModel>> GetAllTaskManagement(TaskManagementFilters taskManagementFilters);
        Task<TaskModel> GetTaskByID(TaskModel taskModel);
        Task<bool> SaveOrUpdate(TaskModel taskModel);
        Task<bool> Delete(TaskModel taskModel);

    }
}
