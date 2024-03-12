using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Buisness.DataAccess
{
    public class SqlDataAccessHelper:ISqlDataAccessHelper
    {
        private string mainConnectionString = string.Empty;
        private string userDataConnectionString = string.Empty;
        private string logConnectionString = string.Empty;

        public SqlDataAccessHelper(IConfiguration configuration)
        {
            this.mainConnectionString = configuration.GetConnectionString("DatabaseConnection");
            //this.logConnectionString = configuration.GetConnectionString("LoggingConnectionString");
        }
        public async Task<int> ExecuteNonQueryAsync(System.Data.SqlClient.SqlCommand cmd)
        {
            using (SqlConnection con = new SqlConnection(this.mainConnectionString))
            {
                try
                {
                    await con.OpenAsync();
                    cmd.Connection = con;
                    var result = await cmd.ExecuteNonQueryAsync();
                    await con.CloseAsync();
                    return result;
                }
                finally
                {
                    await con.DisposeAsync();
                    await con.DisposeAsync();
                }
            }
        }
        public async Task<DataSet> GetDataSet(System.Data.SqlClient.SqlCommand cmd, string connectionKey = "")
        {
            using (SqlConnection con = new SqlConnection(this.mainConnectionString))
            {
                try
                {
                    var ds = new DataSet();
                    await con.OpenAsync();
                    cmd.Connection = con;
                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(ds);
                    }

                    await con.CloseAsync();
                    return ds;
                }
                finally
                {
                    await cmd.DisposeAsync();
                    await con.DisposeAsync();
                }
            }
        }

        public void ExecuteNonQuery(System.Data.SqlClient.SqlCommand cmd)
        {
            using (SqlConnection con = new SqlConnection(this.logConnectionString))
            {

                try
                {
                    con.Open();
                    cmd.Connection = con;
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                finally
                {
                    cmd.Dispose();
                    con.Dispose();
                }
            }
        }

        public int ExecuteScalar(System.Data.SqlClient.SqlCommand cmd)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(this.mainConnectionString))
            {
                try
                {

                    con.Open();
                    cmd.Connection = con;
                    result = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                }
                finally
                {
                    cmd.Dispose();
                    con.Dispose();
                }
            }
            return result;
        }
        public async Task<int> ExecuteScalarAsync(System.Data.SqlClient.SqlCommand cmd)
        {
            int result = 0;
            using (SqlConnection con = new(this.mainConnectionString))
            {

                try
                {
                    await con.OpenAsync();
                    cmd.Connection = con;
                    object scalarResult = await cmd.ExecuteScalarAsync();
                    if (scalarResult != null && scalarResult != DBNull.Value)
                    {
                        result = Convert.ToInt32(scalarResult);
                    }
                    await con.CloseAsync();
                }
                finally
                {
                    await cmd.DisposeAsync();
                    await con.DisposeAsync();
                }

            }
            return result;
        }

    }
}