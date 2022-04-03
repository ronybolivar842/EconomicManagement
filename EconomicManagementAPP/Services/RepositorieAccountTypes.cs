using EconomicManagementAPP.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using EconomicManagementAPP.Interfaces;

namespace EconomicManagementAPP.Services
{
    public class RepositorieAccountTypes : IRepositorieAccountTypes
    {
        private readonly string connectionString;

        public RepositorieAccountTypes(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        
        public async Task Create(AccountTypes accountTypes)
        {
            using var connection = new SqlConnection(connectionString);
            
            var id = await connection.QuerySingleAsync<int>(
                "SP_AccountType_Insert",
                new { accountTypes.UserId, accountTypes.Name },
                commandType : System.Data.CommandType.StoredProcedure
                );
            accountTypes.Id = id;
        }

        
        public async Task<bool> Exist(string name, int userId)
        {
            using var connection = new SqlConnection(connectionString);
            
            var exist = await connection.QueryFirstOrDefaultAsync<int>(@"SELECT 1 FROM AccountTypes
                                                                         WHERE Name = @name AND UserId = @userId;",
                                                                         new { name, userId });
            return exist == 1;
        }

        public async Task<bool> AccountTypeIsUsed(int id)
        {
            using var connection = new SqlConnection(connectionString);
            var used = await connection.QueryFirstOrDefaultAsync<int>(@"SELECT 1 FROM AccountTypes at
                                                                        JOIN Accounts a ON a.AccountTypeId = at.Id
                                                                        WHERE at.Id = @id",
                                                                        new { id });
            return used == 1;
        }

        
        public async Task<IEnumerable<AccountTypes>> GetAccounts(int userId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<AccountTypes>(@"SELECT Id, Name, OrderAccount
                                                               FROM AccountTypes
                                                               WHERE UserId = @UserId
                                                               ORDER BY OrderAccount", new { userId });
        }
        
        public async Task Modify(AccountTypes accountTypes)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE AccountTypes
                                            SET Name = @Name
                                            WHERE Id = @Id", accountTypes);
        }

        public async Task<AccountTypes> GetAccountById(int id, int userId)
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryFirstOrDefaultAsync<AccountTypes>(@"SELECT Id, Name, UserId, OrderAccount
                                                                             FROM AccountTypes
                                                                             WHERE Id = @Id AND UserID = @UserID",
                                                                             new { id, userId });
        }

       
        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE AccountTypes WHERE Id = @Id", new { id });
        }
    }
}
