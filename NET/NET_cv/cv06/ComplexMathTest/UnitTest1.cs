using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ComplexMath;

namespace ComplexMathTest
{
    [TestClass]
    public class UnitTest1
    {
        public TestContext TestContext { get; set; }

        [DeploymentItem("complex_abs.csv")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\complex_abs.csv", "complex_abs#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void Abs_ValidComplex_AbsoluteValue()
        {
            Complex c = new Complex() { Re = (double)TestContext.DataRow[0], Im = (double)TestContext.DataRow[1] };
            double expected = (double)TestContext.DataRow[2];

            double result = c.Abs;

            Assert.AreEqual(expected, result, $"Complex {c.Re} + {c.Im}i");
        }
    }
}
