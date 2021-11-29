using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepo : IUserRepo
    {
        private readonly DataContext _context;

        public UserRepo(DataContext context)
        {
            _context = context;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<AppUser>> GetUsers()
        {
            return await _context.Users.Include(p => p.Photos).ToListAsync();
        }

        public async Task<AppUser> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsername(string username)
        {
            return await _context.Users.Include(p => p.Photos).SingleOrDefaultAsync(u => u.UserName == username);
        }
    }
}