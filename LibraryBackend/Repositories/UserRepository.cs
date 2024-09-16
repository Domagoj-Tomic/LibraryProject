using System.Linq;
using LibraryBackend.Models;

namespace LibraryBackend.Repositories
{
    public class UserRepository
    {
        private readonly LibraryEntities _context;

        public UserRepository(LibraryEntities context)
        {
            _context = context;
        }

        public User ValidateUser(string username, decimal pin)
        {
            return _context.User.FirstOrDefault(u => u.Username == username && u.PIN == pin);
        }
    }
}
