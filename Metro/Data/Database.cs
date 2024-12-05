using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using Metro.Models;

namespace ResourceManager.Core.Data
{
    public class Database
    {
        protected SqlConnection Connection { get; set; }
        protected string ConnectionString { get; set; } = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Metro;Integrated Security=True;Pooling=False;Encrypt=True;Trust Server Certificate=True";

        public Database()
        {
            Connection = new SqlConnection(ConnectionString);
        }
    }
}
