using System;
using AzureMLLibrary.Prediction;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AzureMLTest
{
    [TestClass]
    public class CSPredictionTests
    {
        [TestMethod]
        public void PredictionByImageUrlTest()
        {
            string url = "http://www.cowtownceramics.com/images/Corgi%203%20in.jpg";
            string response = CSPrediction.MakePredictionRequestByImageUrl(url).Result;
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Contains("predictions"));
        }

        [TestMethod]
        public void PredictionByImagePathTest()
        {
            string filePath = @"TestAssets\IMG_9681.JPG";
            string response = CSPrediction.MakePredictionRequestByImagePath(filePath).Result;
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Contains("predictions"));
        }
    }
}
