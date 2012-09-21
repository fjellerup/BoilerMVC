using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using BoilerMVC.Common;
using BoilerMVC.Data;

namespace BoilerMVC.Services
{
    public class SecurityService : BaseService
    {
        private IRepository<User> _userRepository;

        public SecurityService(
            IRepository<User> userRepository,
            IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _userRepository = userRepository;
        }

        public PasswordResetRequest IssuePasswordResetRequest(string email)
        {
            User user = _userRepository.First(item => item.Email == email);

            if (user == null)
                throw new InvalidOperationException(string.Format("No user was found with email {0}", email));

            if (user.PasswordResetRequest != null)
                return user.PasswordResetRequest;

            PasswordResetRequest passwordResetRequest = new PasswordResetRequest()
            {
                Code = Guid.NewGuid(),
                DateCreated = DateTime.UtcNow,
                UserId = user.Id
            };

            user.PasswordResetRequest = passwordResetRequest;

            _unitOfWOrk.Commit();
            return user.PasswordResetRequest;
        }

        public string ResetPassword(string email)
        {
            User user = _userRepository.First(item => item.Email == email);

            if (user == null)
                throw new InvalidOperationException(string.Format("No user was found with email {0}", email));

            string newPassword = Utilities.Security.CreateRandomPassword(8);
            string salt = Utilities.Security.CreateSalt();
            string hashedPassword = Utilities.Security.HashPassword(newPassword, salt);

            user.Salt = salt;
            user.HashedPassword = hashedPassword;

            _unitOfWOrk.Commit();

            return newPassword;
        }

        public bool ValidateLogin(string email, string password, string ipAddress)
        {
            User user = _userRepository.First(item => item.Email == email);
            if (user == null)
                return false;

            string hashedPassword = Utilities.Security.HashPassword(password, user.Salt);
            bool valid = hashedPassword == user.HashedPassword;

            if (valid)
            {
                user.LastLogin = DateTime.UtcNow;
                user.LastLoginIP = ipAddress;
                _unitOfWOrk.Commit();
            }

            return valid;
        }

        public bool ValidatePasswordResetRequest(string email, Guid code)
        {
            User user = _userRepository.First(item => item.Email == email);

            if (user == null)
                return false;

            if (user.PasswordResetRequest == null)
                return false;

            return user.PasswordResetRequest.Code == code;
        }
    }
}