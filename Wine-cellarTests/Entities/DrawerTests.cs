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
    public class DrawerTests
    {
        [TestMethod()]
        public void IsFullTest()
        {
            Drawer drawer = new Drawer()
            {
                DrawerId= 1,
                Index = 0,
                NbBottleMax = 2,
                CellarId= 1,
                Cellar = new Cellar() { },
                Wines = new List<Wine> {new Wine(), new Wine() }

                
            };

            bool result = drawer.IsFull();
            Assert.IsTrue(result);

        }
        [TestMethod()]
        public void IsFullTest2()
        {
            Drawer drawer = new Drawer()
            {
                Index
            = 0
            };

            bool result = drawer.IsFull();
            Assert.IsFalse(result);

        }


    }
}