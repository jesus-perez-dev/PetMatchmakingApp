using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;

namespace API.Interfaces
{
    public interface IUserRepo
    {
        void Update(AppUser user);
        Task<bool> SaveChanges();
        Task<IEnumerable<AppUser>> GetUsers();
        Task<AppUser> GetUserById(int id);
        Task<AppUser> GetUserByUsername(string username);
    }
}