using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BoilerMVC.Common;
using BoilerMVC.Data;
using BoilerMVC.Services;
using NUnit.Framework;

namespace BoilerMVC.Tests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private UserService _userService;

        private List<User> _users = new List<User>()
        {
            new User()
            {
                DateCreated = new DateTime(2011, 1, 1),
                Email = "one@test.com",

                // password is "test"
                HashedPassword = "6c838e934e3feefae6cfa53af11375d4954f85c6f5ed888c02cd7806a71696d1cb449f2be78e9e6ea301a95c81f28ad8766f3ae582f9beaac33c7dc2b7ba9187",
                Id = 1,
                LastLogin = new DateTime(2011, 1, 1),
                LastLoginIP = "127.0.0.1",
                LastLogout = new DateTime(2011, 1, 1),
                Salt = "salt"
            }
        };

        [SetUp]
        public void Initialize()
        {
            var unitOfWork = TestHelpers.MockUnitOfWork();
            var userRepository = TestHelpers.MockRepository<User>(_users);

            _userService = new UserService(userRepository.Object, unitOfWork.Object);
        }

        [Test]
        public void EmailInUse_Returns_False_Unused_Email()
        {
            string email = "not_exist@test.com";
            bool value = _userService.EmailInUse(email);
            Assert.IsFalse(value);
        }

        [Test]
        public void EmailInUse_Returns_True_Used_Email()
        {
            string email = "one@test.com";
            bool value = _userService.EmailInUse(email);
            Assert.IsTrue(value);
        }

        [Test]
        public void Register_New_User_Added()
        {
            string email = "new@test.com";
            string password = "test";
            string ipAddress = "127.0.0.1";

            _userService.Register(email, password, ipAddress);

            User user = _users.FirstOrDefault(item => item.Email == email);

            Assert.IsNotNull(user, "User should be added to repository");
            Assert.AreEqual(email, user.Email, "Email must be the same as registered email");
            Assert.IsNull(user.LastLoginIP, "IP address should not be set yet");
        }
    }
}