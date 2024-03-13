using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_Management_System.Models;
using TaskManagement.Buisness.Filters;
using TaskManagement.Buisness.Interface;
using TaskManagement.Buisness.ViewModels;

namespace Task_Management_System.Controllers
{
    [AuthorizationFilterAttribute]
    public class TaskManagementController : Controller
    {
        private readonly ITaskManagementBuisness taskManagementBuisness;
        public TaskManagementController(ITaskManagementBuisness taskManagementBuisness)
        {
            this.taskManagementBuisness = taskManagementBuisness;
        }
        public async Task<ActionResult> Index(TaskManagementFilters taskManagementFilters)
        {
            var result = await taskManagementBuisness.GetAllTaskManagement(taskManagementFilters);

            // Get enum values
            var taskStatusValues = Enum.GetValues(typeof(Task_Status))
                                       .Cast<Task_Status>()
                                       .Select(s => new SelectListItem
                                       {
                                           Value = ((int)s).ToString(),
                                           Text = s.ToString()
                                       });

            ViewBag.TaskStatus = taskStatusValues;


            return View(result);
        }
        public ActionResult GetAllTask(TaskManagementFilters taskManagementFilters)
        {
            var tasks = taskManagementBuisness.GetAllTaskManagement(taskManagementFilters);
            return PartialView("_TaskTablePartial", tasks.Result);
        }

        public IActionResult Create()
        {
            return PartialView();
        }
        public async Task<ActionResult> Edit(TaskModel taskModel)
        {
            var taskStatusValues = await taskManagementBuisness.GetTaskByID(taskModel);
            return PartialView("Create", taskStatusValues);
        }


        [HttpPost]
        public async Task<IActionResult> Save(TaskModel taskModel)
        {
            try
            {
                // Perform data validation within the try block
                if (taskModel.Title == null)
                {
                    throw new Exception("Please enter Title");
                }

                string taskDate = taskModel.DueDate.ToString();
                if (taskDate == "01-01-0001 00:00:00")
                {
                    throw new Exception("Please select Due Date"); // Or a custom validation exception
                }

                // Save task using taskManagementBuisness
                var status = await this.taskManagementBuisness.SaveOrUpdate(taskModel);

                return Json(new { IsValid = true, message = "Task Saved successfully!" });
            }
            catch (Exception ex)
            {
                // Handle general exceptions gracefully (e.g., logging, user-friendly message)
                return Json(new { IsValid = false, message = ex.Message });
            }
        }

        public async Task<IActionResult> Delete(TaskModel taskModel)
        {
            try
            {
                var status = await this.taskManagementBuisness.Delete(taskModel);

                return Json(new { IsValid = true, message = "Task Deleted Successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { IsValid = false, message = ex.Message });
            }


        }
    }
}
