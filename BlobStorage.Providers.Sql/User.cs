using BlobStorage.Core.Interfaces;
using BlobStorage.Core.Interfaces.user;
using BlobStorage.Core.Models;
using Microsoft.EntityFrameworkCore;




namespace BlobStorage.Providers.Sql
{
    public class UserRep : IUserRepository<User>
    {
        private readonly AppDbContext _context;

        public UserRep(AppDbContext context)
        {
            _context = context;
        }


        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public  async Task<User> GetUserByEmailAsync(string email)
        {
         
            return await _context.Users.FirstOrDefaultAsync(u =>u.Email == email);
        }


        public async Task AddAsync(User User)
        {

            await _context.Users.AddAsync(User);
            await _context.SaveChangesAsync();
           
        }

        public async Task UpdateAsync(User User)
        {
       
            _context.Users.Update(User);
            await _context.SaveChangesAsync();
        }

        public async Task ChangePasswordAsync(User User)
        {
          
            _context.Users.Update(User);
            await _context.SaveChangesAsync();
        }

    }
}
