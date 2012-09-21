using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BoilerMVC.Common;
using BoilerMVC.Data;

namespace BoilerMVC.Services
{
    public class UserService : BaseService
    {
        private IRepository<User> _userRepository;

        public UserService(
            IRepository<User> userRepository,
            IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _userRepository = userRepository;
        }

        public bool EmailInUse(string email)
        {
            User user = _userRepository.First(item => item.Email == email.ToLower());
            return user != null;
        }

        public User Register(string email, string password, string ipAddress)
        {
            string salt = Utilities.Security.CreateSalt();
            string hashedPassword = Utilities.Security.HashPassword(password, salt);

            User user = new User()
            {
                DateCreated = DateTime.UtcNow,
                Email = email.ToLower(),
                HashedPassword = hashedPassword,
                Salt = salt
            };

            _userRepository.Add(user);
            _unitOfWOrk.Commit();
            return user;
        }
    }
}