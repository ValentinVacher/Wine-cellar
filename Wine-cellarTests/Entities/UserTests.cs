using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wine_cellar.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wine_cellar.Entities.Tests
{
    [TestClass()]
    public class UserTests
    {
        [TestMethod()]
        public void IsOlderTest()
        {
            User user = new User()
            {
                FirstName = "Test",
                LastName = "Test",
                Email = "Test@email.com",
                Password = "MDP",
                IsAdmin = false,
                DateOfBirth = new DateTime(1999, 12, 26),
                Cellars = new List<Cellar>()

            };
            bool result = user.IsOlder();
            Assert.IsTrue(result);
        }
    }
}