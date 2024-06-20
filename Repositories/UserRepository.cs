using userauthentication.Data;
using userauthentication.Models;

namespace userauthentication.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _context;
        public UserRepository(UserDbContext context)
        {
            _context = context;
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
        }

        public User? FindByEmail(string Email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == Email);
        }

        public User? FindById(string Uid)
        {
            return _context.Users.FirstOrDefault(u => u.Uid == Uid);
        }

        public User? FindByPasswordResetToken(string PasswordResetToken)
        {
            return _context.Users.FirstOrDefault(u => u.PasswordResetToken == PasswordResetToken);
        }

        public User? FindByRefreshToken(string RefreshToken)
        {
            return _context.Users.FirstOrDefault(u => u.RefreshToken == RefreshToken);
        }

        public User? FindByVerificationToken(string VerificationToken)
        {
            return _context.Users.FirstOrDefault(u => u.EmailVerificationToken == VerificationToken);
        }

        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public void Remove(User user)
        {
            _context.Users.Remove(user);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}