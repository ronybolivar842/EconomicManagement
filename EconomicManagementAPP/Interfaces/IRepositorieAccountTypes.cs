using EconomicManagementAPP.Models;

namespace EconomicManagementAPP.Interfaces
{
    public interface IRepositorieAccountTypes
    {
        Task Create(AccountTypes accountTypes); 
        Task<bool> Exist(string name, int userId);
        Task<IEnumerable<AccountTypes>> GetAccounts(int userId);
        Task Modify(AccountTypes accountTypes);
        Task<AccountTypes> GetAccountById(int id, int userId); 
        Task Delete(int id);
        Task<bool> AccountTypeIsUsed(int id);
    }
}
