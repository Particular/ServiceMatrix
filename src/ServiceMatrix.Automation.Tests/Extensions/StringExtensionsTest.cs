namespace ServiceMatrix.Automation.Tests.Extensions
{
    using NServiceBusStudio.Automation.Extensions;
    using NUnit.Framework;
 
    [TestFixture]
    public class StringExtensionsTest
    {
        [TestCase("lowerCase", "lowerCase")]
        [TestCase("LowerCase", "lowerCase")]
        [TestCase("L", "l")]
        [TestCase("l", "l")]
        [TestCase("LO", "lO")]
        [TestCase("", "")]
        [TestCase(null, null)]
        public void ChangeFirstLetterToLowerCaseTest(string input, string expected)
        {
            var convertedString = input.LowerCaseFirstCharacter();
            Assert.AreEqual(expected, convertedString);
        }
    }
}
