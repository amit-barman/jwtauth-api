using userauthentication.Models;

namespace userauthentication.EntityBuilder
{
    public class UserBuilder
    {
        public int Id;
        public string Uid = string.Empty;
        public string Email = string.Empty;
        public byte[] PasswordHash;
        public byte[] PasswordSalt;
        public DateTime CreatedAt;
        public string AccountType = string.Empty;
        public string? RefreshToken;
        public DateTime? RefreshTokenExpirationTime;
        public string? EmailVerificationToken;
        public DateTime? VerifiedAt;
        public string? PasswordResetToken;
        public DateTime? PasswordResetTokenExpires;

        public UserBuilder()
        {

        }

        public UserBuilder(User user)
        {
            this.Id = user.Id;
            this.Uid = user.Uid;
            this.Email = user.Email;
            this.PasswordHash = user.PasswordHash;
            this.PasswordSalt = user.PasswordSalt;
            this.CreatedAt = user.CreatedAt;
            this.AccountType = user.AccountType;
            this.RefreshToken = user.RefreshToken;
            this.RefreshTokenExpirationTime = user.RefreshTokenExpirationTime;
            this.EmailVerificationToken = user.EmailVerificationToken;
            this.VerifiedAt = user.VerifiedAt;
            this.PasswordResetToken = user.PasswordResetToken;
            this.PasswordResetTokenExpires = user.PasswordResetTokenExpires;
        }

        public User BuildUser()
        {
            return new User
            {
                Id = this.Id,
                Uid = this.Uid,
                Email = this.Email,
                PasswordHash = this.PasswordHash,
                PasswordSalt = this.PasswordSalt,
                CreatedAt = this.CreatedAt,
                AccountType = this.AccountType,
                RefreshToken = this.RefreshToken,
                RefreshTokenExpirationTime = this.RefreshTokenExpirationTime,
                EmailVerificationToken = this.EmailVerificationToken,
                VerifiedAt = this.VerifiedAt,
                PasswordResetToken = this.PasswordResetToken,
                PasswordResetTokenExpires = this.PasswordResetTokenExpires
            };
        }

        public UserBuilder SetUid(string Uid)
        {
            this.Uid = Uid;
            return this;
        }
        public UserBuilder SetEmail(string Email)
        {
            this.Email = Email;
            return this;
        }
        public UserBuilder SetPassword(byte[] PasswordHash)
        {
            this.PasswordHash = PasswordHash;
            return this;
        }
        public UserBuilder SetSalt(byte[] PasswordSalt)
        {
            this.PasswordSalt = PasswordSalt;
            return this;
        }
        public UserBuilder SetCreationTime(DateTime CreatedAt)
        {
            this.CreatedAt = CreatedAt;
            return this;
        }
        public UserBuilder SetAccountType(string AccountType)
        {
            this.AccountType = AccountType;
            return this;
        }
        public UserBuilder SetRefreshToken(string RefreshToken)
        {
            this.RefreshToken = RefreshToken;
            return this;
        }
        public UserBuilder SetRefreshTokenExpirationTime(DateTime RefreshTokenExpirationTime)
        {
            this.RefreshTokenExpirationTime = RefreshTokenExpirationTime;
            return this;
        }
        public UserBuilder SetEmailVerificationToken(string EmailVerificationToken)
        {
            this.EmailVerificationToken = EmailVerificationToken;
            return this;
        }
        public UserBuilder SetVerifiedAt(DateTime VerifiedAt)
        {
            this.VerifiedAt = VerifiedAt;
            return this;
        }
        public UserBuilder SetPasswordResetToken(string PasswordResetToken)
        {
            this.PasswordResetToken = PasswordResetToken;
            return this;
        }
        public UserBuilder SetPasswordResetTokenExpires(DateTime PasswordResetTokenExpires)
        {
            this.PasswordResetTokenExpires = PasswordResetTokenExpires;
            return this;
        }
    }
}