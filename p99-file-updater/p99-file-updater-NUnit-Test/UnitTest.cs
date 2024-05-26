using Microsoft.VisualStudio.TestTools.UnitTesting;
using p99FileUpdater;
using System;

namespace p99_file_updater_NUnit_Test
{
    [TestClass]
    public class p99ViewModelTests
    {
        p99FileDownloaderViewModel hw;

        [TestInitialize]
        public void initializeTest()
        {
            hw = new p99FileDownloaderViewModel();
        }
        [TestCategory("MessageStringTests")]
        [TestMethod]
        public void TestMessageProperty()
        {
            Assert.AreEqual(hw.MessageBox, string.Empty);
            const string msg = "Test String";
            hw.MessageBox = msg;
            Assert.AreEqual(hw.MessageBox, msg);
            Assert.AreNotEqual(hw.MessageBox, null);
        }

        [TestCategory("MessageDisplayedTests")]
        [TestMethod]
        public void TestButtonMessageDisplayedPropertyAreNotEqual()
        {
            hw.MessageDisplayed = true;
            Assert.AreNotEqual(hw.MessageDisplayed, false);
            hw.MessageDisplayed = false;
            Assert.AreNotEqual(hw.MessageDisplayed, true);
        }

        [TestMethod]
        public void TestOverrideChecksumValidationTrue()
        {
            hw.OverrideChecksumValidation = true;
            Assert.AreEqual(hw.OverrideChecksumValidation, true);
        }

        [TestMethod]
        public void TestOverrideChecksumValidationFalse()
        {
            hw.OverrideChecksumValidation = false;
            Assert.AreEqual (hw.OverrideChecksumValidation, false);
        }

        [TestMethod]
        public void TestdownloadAddressEmpty()
        {
            hw.DownloadAddress = new Uri(string.Empty);
            Assert.AreEqual(hw.DownloadAddress, new Uri(string.Empty));
        }

        [TestMethod]
        public void TestDownloadAddress()
        {
            Uri uri = new Uri("https://project1999.com/latest.zip");
            hw.DownloadAddress = new Uri(uri.OriginalString);
            Assert.AreEqual(hw.DownloadAddress, uri);
        }
    }
}
