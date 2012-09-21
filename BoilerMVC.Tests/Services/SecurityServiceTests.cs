using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using BoilerMVC.Common;
using BoilerMVC.Data;
using BoilerMVC.Services;
using Moq;
using NUnit;
using NUnit.Framework;

namespace BoilerMVC.Tests.Services
{
    [TestFixture]
    public class SecurityServiceTests
    {
        private SecurityService _securityService;

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
                Salt = "salt",
                PasswordResetRequest = new PasswordResetRequest()
                {
                    Code = Guid.Parse("b52ecfb0-eeb5-4434-81a3-8d2c987aba2f"),
                    DateCreated = new DateTime(2011, 1, 1),
                    UserId = 1
                }
            },
            new User()
            {
                DateCreated = new DateTime(2011, 1, 1),
                Email = "two@test.com",

                // password is "test"
                HashedPassword = "6c838e934e3feefae6cfa53af11375d4954f85c6f5ed888c02cd7806a71696d1cb449f2be78e9e6ea301a95c81f28ad8766f3ae582f9beaac33c7dc2b7ba9187",
                Id = 2,
                LastLogin = new DateTime(2011, 1, 1),
                LastLoginIP = "127.0.0.1",
                LastLogout = new DateTime(2011, 1, 1),
                Salt = "salt",
            },
            new User()
            {
                DateCreated = new DateTime(2011, 1, 1),
                Email = "three@test.com",

                // password is "test"
                HashedPassword = "6c838e934e3feefae6cfa53af11375d4954f85c6f5ed888c02cd7806a71696d1cb449f2be78e9e6ea301a95c81f28ad8766f3ae582f9beaac33c7dc2b7ba9187",
                Id = 3,
                LastLogin = new DateTime(2011, 1, 1),
                LastLoginIP = "127.0.0.1",
                LastLogout = new DateTime(2011, 1, 1),
                Salt = "salt",
            },
            new User()
            {
                DateCreated = new DateTime(2011, 1, 1),
                Email = "four@test.com",

                // password is "test"
                HashedPassword = "6c838e934e3feefae6cfa53af11375d4954f85c6f5ed888c02cd7806a71696d1cb449f2be78e9e6ea301a95c81f28ad8766f3ae582f9beaac33c7dc2b7ba9187",
                Id = 3,
                LastLogin = new DateTime(2011, 1, 1),
                LastLoginIP = "127.0.0.1",
                LastLogout = new DateTime(2011, 1, 1),
                Salt = "salt",
            }
        };

        [SetUp]
        public void Initialize()
        {
            var unitOfWork = TestHelpers.MockUnitOfWork();
            var userRepository = TestHelpers.MockRepository<User>(_users);
            _securityService = new SecurityService(userRepository.Object, unitOfWork.Object);
        }

        [Test]
        public void IssuePasswordResetRequest_Throws_InvalidOperationException_For_Not_Found_User()
        {
            string email = "not_exist@test.com";
            Assert.Throws<InvalidOperationException>(() => _securityService.IssuePasswordResetRequest(email), "Should throw exception for user that doesn't exist");
        }

        [Test]
        public void IssuePasswordResetRequest_Returns_Existing_PasswordResetRequest()
        {
            string email = "one@test.com";
            PasswordResetRequest request = _securityService.IssuePasswordResetRequest(email);
            Assert.AreEqual(1, request.UserId, "ID should be first user's ID");
            Assert.AreEqual(Guid.Parse("b52ecfb0-eeb5-4434-81a3-8d2c987aba2f"), request.Code);
            Assert.AreEqual(new DateTime(2011, 1, 1), request.DateCreated);
        }

        [Test]
        public void IssuePasswordResetRequest_Returns_New_PasswordResetRequest()
        {
            string email = "two@test.com";
            PasswordResetRequest request = _securityService.IssuePasswordResetRequest(email);
            Assert.AreEqual(request, _users.First(item => item.Id == request.UserId).PasswordResetRequest);
        }

        [Test]
        public void ResetPassword_Throws_InvalidOperationException_For_Not_Found_User()
        {
            string email = "not_exist@test.com";

            Assert.Throws<InvalidOperationException>(() => _securityService.ResetPassword(email), "Should throw exception for user that doesn't exist");
        }

        [Test]
        public void ResetPassword_Updates_User_Password_With_New_Password()
        {
            string email = "four@test.com";

            User user = _users[3];
            string oldHashedPassword = user.HashedPassword;
            string oldSalt = user.Salt;

            _securityService.ResetPassword(email);

            Assert.AreNotEqual(oldHashedPassword, user.HashedPassword);
            Assert.AreNotEqual(oldSalt, user.Salt);
        }

        [Test]
        public void ValidateLogin_Returns_False_Invalid_Email()
        {
            string email = "not_exist@test.com";
            string password = "test";
            string ipAddress = "127.0.0.1";
            bool value = _securityService.ValidateLogin(email, password, ipAddress);

            Assert.IsFalse(value, "Should return false if invalid email");
        }

        [Test]
        public void ValidateLogin_Returns_False_Invalid_Password()
        {
            string email = "one@test.com";
            string password = "test_INVALID";
            string ipAddress = "127.0.0.1";
            bool value = _securityService.ValidateLogin(email, password, ipAddress);

            Assert.IsFalse(value, "Should return false if invalid password");
        }

        [Test]
        public void ValidateLogin_Returns_True_Valid_Login()
        {
            string email = "one@test.com";
            string password = "test";
            string ipAddress = "127.0.0.1";
            bool value = _securityService.ValidateLogin(email, password, ipAddress);

            Assert.IsTrue(value, "Should return true for valid login");
        }

        [Test]
        public void ValidateLogin_Updates_Login_Info()
        {
            string email = "one@test.com";
            string password = "test";
            string ipAddress = "127.0.0.2";

            _securityService.ValidateLogin(email, password, ipAddress);

            User user = _users.FirstOrDefault();
            DateTime defaultDate = new DateTime(2011, 1, 1);

            Assert.Greater(user.LastLogin, defaultDate, "LastLogin date should be updated");
            Assert.AreEqual(ipAddress, user.LastLoginIP, "LastLoginIP should be updated");
        }

        [Test]
        public void ValidatePasswordResetRequest_Returns_False_For_Invalid_User()
        {
            string email = "not_exist@test.com";
            Guid code = Guid.Parse("b52ecfb0-eeb5-4434-81a3-8d2c987aba2f");

            bool result = _securityService.ValidatePasswordResetRequest(email, code);
            Assert.IsFalse(result, "Sould return false for user that doesn't exist");
        }

        [Test]
        public void ValidatePasswordResetRequest_Returns_False_For_Null_PasswordResetRequest()
        {
            string email = "three@test.com";
            Guid code = Guid.Parse("b52ecfb0-eeb5-4434-81a3-8d2c987aba2f");

            bool result = _securityService.ValidatePasswordResetRequest(email, code);
            Assert.IsFalse(result, "Sould return false for user that doesn't have a password reset request");
        }

        [Test]
        public void ValidatePasswordResetRequest_Returns_False_For_Invalid_Code()
        {
            string email = "one@test.com";
            Guid code = Guid.Parse("40dca0bb-7794-4e10-a50b-7f5f9501f74a");

            bool result = _securityService.ValidatePasswordResetRequest(email, code);
            Assert.IsFalse(result, "Should return false for invalid code");
        }

        [Test]
        public void ValidatePasswordResetRequest_Returns_True_For_Existing_User_And_Valid_Code()
        {
            string email = "one@test.com";
            Guid code = Guid.Parse("b52ecfb0-eeb5-4434-81a3-8d2c987aba2f");

            bool result = _securityService.ValidatePasswordResetRequest(email, code);
            Assert.IsTrue(result, "Should return true for existing user and valid code");
        }
    }
}