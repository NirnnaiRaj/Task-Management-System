using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Buisness.DataAccess;
using TaskManagement.Buisness.Interface;
using TaskManagement.Buisness.ViewModels;

namespace TaskManagement.Buisness.Handlers
{
    public class TaskManagementBuisness : ITaskManagementBuisness
    {
        private readonly ISqlDataAccessHelper sqlDataAccessHelper;
        private readonly IHttpContextAccessor httpContextAccessor;
        public TaskManagementBuisness(ISqlDataAccessHelper sqlDataAccessHelper, IHttpContextAccessor httpContextAccessor)
        {
            this.sqlDataAccessHelper = sqlDataAccessHelper;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<TaskModel>> GetAllTaskManagement(TaskManagementFilters taskManagementFilters)
        {
            List<TaskModel> taskManagementList = new List<TaskModel>();
            var userID = httpContextAccessor.HttpContext.Session.GetString("userID");
            int statusValue = (int)taskManagementFilters.Status;
            taskManagementFilters.User = new UserModel { Id = Convert.ToInt32(userID) };
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "GetAllTaskManagement";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Status", statusValue);
                cmd.Parameters.AddWithValue("@UserID", taskManagementFilters.User.Id);
                using (DataSet ds = await this.sqlDataAccessHelper.GetDataSet(cmd))
                {
                    if (ds != null && ds.Tables != null)
                    {
                        var taskListTable = ds.Tables[0];

                        taskManagementList = (from DataRow row in taskListTable.Rows
                                              select new TaskModel
                                              {
                                                  Id = Convert.ToInt32(row["Id"]),
                                                  Title = Convert.ToString(row["TaskTitle"]),
                                                  Description = Convert.ToString(row["TaskDescription"]),
                                                  DueDate = Convert.ToDateTime(row["TaskDueDate"]),
                                                  Status = Convert.ToBoolean(row["TaskStatus"]),
                                                  TaskStatus = (Task_Status)Convert.ToInt32(row["TaskStatusType"]),
                                                  Deleted = Convert.ToBoolean(row["Deleted"]),
                                              }).ToList();

                    }
                }
                return taskManagementList;

            }

        }
        public async Task<TaskModel> GetTaskByID(TaskModel taskModel)
        {
            TaskModel taskManagementList = new TaskModel();
            var userID = httpContextAccessor.HttpContext.Session.GetString("userID");
            taskModel.User = new UserModel { Id = Convert.ToInt32(userID) };
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "GetTaskManagementByID";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TaskID", taskModel.Id);
                cmd.Parameters.AddWithValue("@UserID", taskModel.User.Id);
                using (DataSet ds = await this.sqlDataAccessHelper.GetDataSet(cmd))
                {
                    if (ds != null && ds.Tables != null)
                    {
                        var taskListTable = ds.Tables[0];
                        foreach (DataRow row in taskListTable.Rows)
                        {
                            TaskModel taskModels = new TaskModel
                            {
                                Id = Convert.ToInt32(row["ID"]),
                                Title = row["TaskTitle"].ToString(),
                                Status = Convert.ToBoolean(row["TaskStatus"]),
                                DueDate = Convert.ToDateTime(row["TaskDueDate"]),
                                Description = row["TaskDescription"].ToString(),
                                Deleted = Convert.ToBoolean(row["Deleted"])
                            };
                            taskManagementList = taskModels;
                        }

                    }
                }
                return taskManagementList;
            }
        }

        public async Task<bool> SaveOrUpdate(TaskModel taskModel)
        {
            var userID = httpContextAccessor.HttpContext.Session.GetString("userID");
            taskModel.User = new UserModel { Id = Convert.ToInt32(userID) };
            var cmd = new SqlCommand();
            cmd.CommandText = "UpsertTask";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", taskModel.Id);
            cmd.Parameters.AddWithValue("@Title", taskModel.Title);
            cmd.Parameters.AddWithValue("@Description", taskModel.Description);
            cmd.Parameters.AddWithValue("@DueDate", taskModel.DueDate);
            cmd.Parameters.AddWithValue("@Status", taskModel.Status);
            cmd.Parameters.AddWithValue("@UserID", taskModel.User.Id);
            var result = await sqlDataAccessHelper.ExecuteNonQueryAsync(cmd);
            return result > 0;

        }


        public async Task<bool> Delete(TaskModel taskModel)
        {
            var userID = httpContextAccessor.HttpContext.Session.GetString("userID");
            taskModel.User = new UserModel { Id = Convert.ToInt32(userID) };
            var cmd = new SqlCommand();
            cmd.CommandText = "DeleteTask";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", taskModel.Id);
            cmd.Parameters.AddWithValue("@UserID", taskModel.User.Id);
            var result = await sqlDataAccessHelper.ExecuteNonQueryAsync(cmd);
            return result > 0;
        }
    }
}
