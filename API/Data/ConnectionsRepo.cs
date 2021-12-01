using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class ConnectionsRepo : IConnectionsRepo
    {
        private readonly DataContext _context;

        public ConnectionsRepo(DataContext context)
        {
            _context = context;
        }

        public async Task<Connection> GetConnection(int sourceUserId, int likedUserId)
        {
            return await _context.Connections.FindAsync(sourceUserId, likedUserId);
        }

        public async Task<AppUser> GetConnectionsOfUser(int userId)
        {
            return await _context.Users.Include(u => u.LikedUsers).FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<IEnumerable<ConnectionDto>> GetConnections(string connectionType, int userId)
        {
            var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
            var connections = _context.Connections.AsQueryable();

            if (connectionType == "liked")
            {
                connections = connections.Where(l => l.SourceUserId == userId);
                users = connections.Select(l => l.LikedUser);
            }
            if (connectionType == "likedBy")
            {
                connections = connections.Where(l => l.LikedUserId == userId);
                users = connections.Select(l => l.SourceUser);
            }

            return await users.Select(u => new ConnectionDto
            {
                Username = u.UserName,
                Alias = u.Alias,
                Age = u.GetAge(),
                ProfilePhoto = u.Photos.FirstOrDefault(p => p.IsProfilePic).Url,
                City = u.City,
                Id = u.Id
            }).ToListAsync();
        }
    }
}
