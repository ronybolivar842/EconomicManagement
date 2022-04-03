using EconomicManagementAPP.Models;

namespace EconomicManagementAPP.Interfaces
{
    public interface IRepositorieUsers
    {
        Task Create(Users user); 
        Task<bool> Exist(string email);
        Task<IEnumerable<Users>> GetUsers();
        Task Modify(Users user);
        Task<Users> GetUserById(int id); 
        Task<Users> Login(string email, string password);
        Task<Users> GetUserByEmail(string standarEmail);
    }
}
