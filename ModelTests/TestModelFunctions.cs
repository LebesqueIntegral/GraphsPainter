using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;

namespace ModelTests
{
    [TestClass]
    public class TestModelFunctions
    {
        [TestMethod]
        public void TestFunc()
        {
            Assert.AreEqual(400.5, ModelData.Func(20, 0.5));
        }

        [TestMethod]
        public void TestInterpolate()
        {
            ModelData modelData = new ModelData(2, 3);
            Assert.AreEqual(3.5, Interpolate.F(0.5, modelData));
        }

        [TestMethod]
        public void TestAdd_ModelData()
        {
            ObservableModelData observableData = new ObservableModelData();
            ModelData modelData = new ModelData(2, 3);
            observableData.Add_ModelData(modelData);
            Assert.AreEqual(1, observableData.Count);
            Assert.AreEqual(3, observableData[0].P);
            Assert.AreEqual(2, observableData[0].Nodes_count);
        }

        [TestMethod]
        public void TestRemove_ModelData()
        {
            ObservableModelData observableData = new ObservableModelData();
            ModelData modelData = new ModelData(2, 3);
            observableData.Add_ModelData(modelData);
            observableData.Remove_ModelData(modelData);
            Assert.AreEqual(0, observableData.Count);
        }

        [TestMethod]
        public void TestAddDefaults()
        {
            ObservableModelData observableData = new ObservableModelData();
            observableData.AddDefaults();
            Assert.AreEqual(3, observableData.Count);
            Assert.AreEqual(2, observableData[0].Nodes_count);
            Assert.AreEqual(2.5, observableData[1].P);
            Assert.AreEqual(0.1, observableData[2].Nodes[1]);
        }

        [TestMethod]
        public void TestSerializing()
        {
            ObservableModelData observableData = new ObservableModelData();
            observableData.AddDefaults();
            Assert.AreEqual(false, Serializing.Save(null, observableData));
            Assert.AreEqual(false, Serializing.Load(null, ref observableData));
            Assert.AreEqual(true, Serializing.Save("1.txt", observableData));
        }

    }
}
