using userauthentication.Models;

namespace userauthentication.Repositories
{
    public interface IUserRepository
    {
        List<User> GetAll();
        User? FindById(string Uid);
        User? FindByEmail(string Email);
        User? FindByRefreshToken(string RefreshToken);
        User? FindByVerificationToken(string VerificationToken);
        User? FindByPasswordResetToken(string PasswordResetToken);
        void Add(User user);
        void Update(User user, User userUpdated);
        void Remove(User user);
        Task SaveAsync();
    }
}