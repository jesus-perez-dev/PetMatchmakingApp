using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IConnectionsRepo
    {
        Task<Connection> GetConnection(int sourceUserId, int likedUserId);
        Task<AppUser> GetConnectionsOfUser(int userId);
        Task<IEnumerable<ConnectionDto>> GetConnections(string connectionType, int userId);
    }
}