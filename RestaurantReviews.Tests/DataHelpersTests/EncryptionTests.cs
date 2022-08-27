﻿using RestaurantReviews.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReviews.Tests.DataHelpersTests
{
    [TestClass]
    public class EncryptionTests
    {
        [TestMethod]
        public void CanUseEncyptFunction()
        {
            // Arrange
            var password = "123Password!@#Pass@WOrd";

            // Act
            var encryptedPassword = DataHelpers.PasswordEncrypt(password);

            // Assert
            Assert.IsNotNull(encryptedPassword);
            Assert.AreEqual(64, encryptedPassword.Length);
        }

        [TestMethod]
        public void CanEncryptAndDecryptPasswords()
        {
            // Arrange
            var password = "123Password!@#Pass@WOrd";

            // Act
            var encryptedPassword = DataHelpers.PasswordEncrypt(password);
            var decryptedPassword = DataHelpers.PasswordDecrypt(encryptedPassword);

            // Assert
            Assert.IsNotNull(encryptedPassword);
            Assert.AreEqual(64, encryptedPassword.Length);

            Assert.AreEqual(password.Length, decryptedPassword.Length);
            Assert.AreEqual(password, decryptedPassword);
        }
    }
}
