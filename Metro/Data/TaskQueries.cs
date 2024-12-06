using Metro.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = Metro.Models.Task;

namespace Metro.Data
{
    public class TaskQueries : Database
    {
        public List<Task> GetTasks()
        {
            var tasks = new List<Task>();

            Connection.Open();

            if (Connection.State == ConnectionState.Open)
            {
                string getAllRecords = "SELECT * FROM Tasks";
                var command = new SqlCommand(getAllRecords, Connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var task = new Task
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Description = reader["description"].ToString()!,
                        StartTime = TimeOnly.FromDateTime(DateTime.Parse(reader["start_time"].ToString()!)),
                        EndTime = reader["end_time"] != null ? TimeOnly.FromDateTime(DateTime.Parse(reader["end_time"].ToString()!)) : null,
                        WorkDayId = Convert.ToInt32(reader["work_day_id"])
                    };

                    tasks.Add(task);

                }

                Connection.Close();
            }

            return tasks;
        }

        public int GetTasksCount()
        {
            int count = 0;
            Connection.Open();

            if (Connection.State == ConnectionState.Open)
            {
                string getAllRecordsCount = "SELECT COUNT(*) FROM Tasks";
                var command = new SqlCommand(getAllRecordsCount, Connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    count = Convert.ToInt32(reader[0]);
                }

                Connection.Close();
            }

            return count;
        }

        public Task? GetTask(int taskId)
        {
            Task? task = null;
            Connection.Open();

            if (Connection.State == ConnectionState.Open)
            {
                string getTask = $"SELECT * FROM Tasks WHERE task.Id = {taskId}";
                var command = new SqlCommand(getTask, Connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    task = new Task
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Description = reader["description"].ToString()!,
                        StartTime = TimeOnly.FromDateTime(DateTime.Parse(reader["start_time"].ToString()!)),
                        EndTime = reader["end_time"] != null ? TimeOnly.FromDateTime(DateTime.Parse(reader["end_time"].ToString()!)) : null,
                        WorkDayId = Convert.ToInt32(reader["work_day_id"])
                    };

                }
            }

            Connection.Close();
            return task;
        }

        public bool CreateNewTask(Task task)
        {
            Connection.Open();

            if (Connection.State == ConnectionState.Open)
            {
                string createNewTask = $@"INSERT INTO Tasks 
                                                ([description], [start_time], [end_time], [work_day_id]) 
                                            VALUES
                                                ('{task.Description}','{task.StartTime}', '{task.EndTime}', {task.WorkDayId})";

                var command = new SqlCommand(createNewTask, Connection);
                command.ExecuteNonQuery();
            }

            Connection.Close();

            return true;
        }

        public List<Task> SearchTable(string searchTerm)
        {
            var tasks = new List<Task>();

            Connection.Open();

            var searchTableForTerm = $"SELECT * FROM Tasks WHERE id LIKE '%{searchTerm}%'";

            var command = new SqlCommand(searchTableForTerm, Connection);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                var task = new Task
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Description = reader["description"].ToString()!,
                    StartTime = TimeOnly.FromDateTime(DateTime.Parse(reader["start_time"].ToString()!)),
                    EndTime = reader["end_time"] != null ? TimeOnly.FromDateTime(DateTime.Parse(reader["end_time"].ToString()!)) : null,
                    WorkDayId = Convert.ToInt32(reader["work_day_id"])
                };

                tasks.Add(task);
            }

            Connection.Close();
            return tasks;
        }

        public bool UpdateTask(Task task)
        {
            Connection.Open();

            var taskExists = false;

            var checkTaskExists = $"SELECT * FROM Tasks WHERE id = {task.Id}";

            var command = new SqlCommand(checkTaskExists, Connection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                if (Convert.ToInt32(reader[0]) == task.Id)
                {
                    taskExists = true;
                }
            }
            reader.Close();

            if (taskExists == false)
            {
                var exception = new DataException("Work day does not exist");
                Connection.Close();
                throw exception;
            }

            if (Connection.State == ConnectionState.Open)
            {
                string updateTask = $@"UPDATE Tasks 
                                         SET [description] = '{task.Description}', [start_time] = '{task.StartTime}', [end_time] = '{task.EndTime}', [work_day_id] = {task.WorkDayId} WHERE [id] = {task.Id}";

                command = new SqlCommand(updateTask, Connection);
                command.ExecuteNonQuery();
            }

            Connection.Close();

            return true;
        }
    }
}
