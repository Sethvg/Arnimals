using System;
using System.Collections.Generic;
using System.IO;
using AzureMLLibrary.Training;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AzureMLTest
{
    [TestClass]
    public class CSUploadAndTrainTests
    {
        [TestMethod]
        public void UploadImageTagAndTrain ()
        {
            string trainingKey = "4a1c89faa74d479cb97ee40e93bc140d";
            Guid projectId = Guid.Parse("76e820e5-5b11-49d5-872e-05a672e3689c");

            var root = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string pathToImages = Path.Combine(root, "TestAssets");

            List<string> images = new List<string>{ Path.Combine(pathToImages, "Peter_1.png"),
                                                    Path.Combine(pathToImages, "Peter_2.jpg"),
                                                    Path.Combine(pathToImages, "Peter_3.jpg"),
                                                    Path.Combine(pathToImages, "Peter_4.jpg"),
                                                    Path.Combine(pathToImages, "Peter_5.jpg")};

            Guid newTag = CSUploadAndTrain.UploadImageAndReturnTag(projectId, trainingKey, images);
            Assert.IsTrue(newTag != Guid.Empty);

            bool trainSuccess = CSUploadAndTrain.Train(projectId, trainingKey);
            Assert.IsTrue(trainSuccess);

            bool deleteTagSuccess = CSUploadAndTrain.RemoveImagesByTag(projectId, trainingKey, newTag);
            Assert.IsTrue(deleteTagSuccess);
        }
    }
}
