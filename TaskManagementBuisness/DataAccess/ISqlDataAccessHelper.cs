using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Buisness.DataAccess
{
    public interface ISqlDataAccessHelper
    {
        public Task<int> ExecuteNonQueryAsync(SqlCommand cmd);

        public Task<DataSet> GetDataSet(SqlCommand cmd, string connectionKey = "");

        public void ExecuteNonQuery(SqlCommand cmd);
        public int ExecuteScalar(SqlCommand cmd);
        public Task<int> ExecuteScalarAsync(System.Data.SqlClient.SqlCommand cmd);
    }
}
