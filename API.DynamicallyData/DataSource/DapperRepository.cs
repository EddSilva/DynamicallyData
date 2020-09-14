using API.DynamicallyData.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace API.DynamicallyData.DataSource
{
    public class DapperRepository
    {
        private readonly ILogger<DapperRepository> logger;
        private readonly IConfiguration configuration;
        private readonly string connetionString;

        public DapperRepository(ILogger<DapperRepository> logger, IConfiguration configuration)
        {
            connetionString = configuration.GetConnectionString("SqlConnectionString");

            if (string.IsNullOrEmpty(connetionString))
            {
                throw new InvalidOperationException("Connnection string is invalid.");
            }

            this.logger = logger;
            this.configuration = configuration;
        }

        public async Task<IEnumerable<IEnumerable<Element>>> GetTablesAsync()
        {
            using (var connection = new SqlConnection(connetionString))
            {
                await connection.OpenAsync();

                var sqlQuery = $@"SELECT TABLE_NAME
                                  FROM INFORMATION_SCHEMA.TABLES
                                  WHERE TABLE_SCHEMA='SalesLT'";

                var result = await connection.QueryAsync(sqlQuery);

                await connection.CloseAsync();

                return result.ToElements();
            }
        }

        public async Task<IEnumerable<IEnumerable<Element>>> GetTableAsync(string tableName, int pageSize, int pageNumber)
        {
            using (var connection = new SqlConnection(connetionString))
            {
                await connection.OpenAsync();

                var sqlQuery = $@"SELECT * FROM [SalesLT].[{tableName}]  
                                  ORDER BY CURRENT_TIMESTAMP
                                  OFFSET {pageNumber} ROWS       -- skip rows
                                  FETCH NEXT {pageSize} ROWS ONLY; -- take rows";

                var result = await connection.QueryAsync(sqlQuery);

                await connection.CloseAsync();

                return result.ToElements();
            }
        }

        public async Task<int> GetTableCountAsync(string tableName)
        {
            using (var connection = new SqlConnection(connetionString))
            {
                await connection.OpenAsync();

                var sqlQuery = $@"SELECT COUNT(*) FROM [SalesLT].[{tableName}]";

                var result = await connection.QueryAsync<int>(sqlQuery);

                await connection.CloseAsync();

                return result.FirstOrDefault();
            }
        }
    }
}
