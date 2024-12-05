using Metro.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metro.Data
{
    public class WorkDayQueries : Database
    {
        public List<WorkDay> GetWorkDays()
        {
            var workDays = new List<WorkDay>();

            Connection.Open();

            if (Connection.State == ConnectionState.Open)
            {
                string getAllRecords = "SELECT * FROM WorkDays";
                var command = new SqlCommand(getAllRecords, Connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var workDay = new WorkDay
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        WorkDate = DateOnly.FromDateTime(DateTime.Parse(reader["work_date"].ToString()!)),
                        ClockInTime = TimeOnly.FromDateTime(DateTime.Parse(reader["clock_in_time"].ToString()!)),
                        ClockOutTime = reader["clock_out_time"] != null ? TimeOnly.FromDateTime(DateTime.Parse(reader["clock_out_time"].ToString()!)) : null
                    };

                    workDays.Add(workDay);

                }

                Connection.Close();
            }

            return workDays;
        }

        public int GetWorkDaysCount()
        {
            int count = 0;
            Connection.Open();

            if (Connection.State == ConnectionState.Open)
            {
                string getAllRecordsCount = "SELECT COUNT(*) FROM WorkDays";
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

        public WorkDay? GetWorkDay(int workDayId)
        {
            WorkDay? workDay = null;
            Connection.Open();

            if (Connection.State == ConnectionState.Open)
            {
                string getWorkDay = $"SELECT * FROM WorkDays WHERE workDay.Id = {workDay.Id}";
                var command = new SqlCommand(getWorkDay, Connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    workDay = new WorkDay
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        WorkDate = DateOnly.Parse(reader["work_date"].ToString()!),
                        ClockInTime = TimeOnly.Parse(reader["clock_in_time"].ToString()!),
                        ClockOutTime = reader["clock_out_time"] != null ? TimeOnly.Parse(reader["clock_out_time"].ToString()!) : null
                    };

                }
            }

            Connection.Close();
            return workDay;
        }

        public bool CreateNewWorkDay(WorkDay workDay)
        {
            Connection.Open();

            var workDayExists = false;

            var checkWorkDayExists = $"SELECT * FROM WorkDays WHERE id = {workDay.Id}";

            var command = new SqlCommand(checkWorkDayExists, Connection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                if (Convert.ToInt32(reader[0]) == workDay.Id)
                {
                    workDayExists = true;
                }
            }
            reader.Close();

            if (workDayExists)
            {
                var exception = new DataException("WorkDay already exists");
                Connection.Close();
                throw exception;
            }

            if (Connection.State == ConnectionState.Open)
            {
                string createNewWorkDay = $@"INSERT INTO WorkDays 
                                                ([work_date], [clock_in_time], [clock_out_time]) 
                                            VALUES
                                                ('{workDay.WorkDate}', '{workDay.ClockInTime}', '{workDay.ClockOutTime}')";

                command = new SqlCommand(createNewWorkDay, Connection);
                command.ExecuteNonQuery();
            }

            Connection.Close();

            return true;
        }

        public List<WorkDay> SearchTable(string searchTerm)
        {
            var workDays = new List<WorkDay>();

            Connection.Open();

            var searchTableForTerm = $"SELECT * FROM WorkDays WHERE id LIKE '%{searchTerm}%'";

            var command = new SqlCommand(searchTableForTerm, Connection);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                var workDay = new WorkDay
                {
                    Id = Convert.ToInt32(reader["id"]),
                    WorkDate = DateOnly.FromDateTime(DateTime.Parse(reader["work_date"].ToString()!)),
                    ClockInTime = TimeOnly.FromDateTime(DateTime.Parse(reader["clock_in_time"].ToString()!)),
                    ClockOutTime = reader["clock_out_time"] != null ? TimeOnly.FromDateTime(DateTime.Parse(reader["clock_out_time"].ToString()!)) : null
                };

                workDays.Add(workDay);
            }

            Connection.Close();
            return workDays;
        }

        public bool UpdateWorkDay(WorkDay workDay)
        {
            Connection.Open();

            var workDayExists = false;

            var checkWorkDayExists = $"SELECT * FROM WorkDays WHERE id = {workDay.Id}";

            var command = new SqlCommand(checkWorkDayExists, Connection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                if (Convert.ToInt32(reader[0]) == workDay.Id)
                {
                    workDayExists = true;
                }
            }
            reader.Close();

            if (workDayExists == false)
            {
                var exception = new DataException("Work day does not exist");
                Connection.Close();
                throw exception;
            }

            if (Connection.State == ConnectionState.Open)
            {
                string updateWorkDay = $@"UPDATE WorkDay 
                                         SET [work_date] = '{workDay.WorkDate.ToString()}', [clock_in_time] = '{workDay.ClockInTime.ToString()}', [clock_out_time] = {workDay.ClockOutTime.ToString()} WHERE [id] = {workDay.Id}";

                command = new SqlCommand(updateWorkDay, Connection);
                command.ExecuteNonQuery();
            }

            Connection.Close();

            return true;
        }

        //public bool DeleteStockItem(string workDay.Id)
        //{
        //    Connection.Open();

        //    var workDayExists = false;

        //    var checkWorkDayExists = $"SELECT * FROM WorkDays WHERE id = {workDay.Id}";

        //    var command = new SqlCommand(checkWorkDayExists, Connection);
        //    SqlDataReader reader = command.ExecuteReader();

        //    while (reader.Read())
        //    {
        //        if (Convert.ToInt32(reader[0]) == workDay.Id)
        //        {
        //            workDayExists = true;
        //        }
        //    }
        //    reader.Close();

        //    if (!workDayExists)
        //    {
        //        var exception = new DataException("StockItem does not exist");
        //        Connection.Close();
        //        throw exception;
        //    }

        //    if (Connection.State == ConnectionState.Open)
        //    {
        //        string deleteStockItem = $"DELETE FROM WorkDays WHERE id = {workDay.Id}";

        //        command = new SqlCommand(deleteStockItem, Connection);
        //        command.ExecuteNonQuery();
        //    }

        //    Connection.Close();

        //    return true;
        //}
    }
}
