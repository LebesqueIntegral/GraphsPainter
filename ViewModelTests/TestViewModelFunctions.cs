using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViewModel;

namespace ViewModelTests
{
    public class FakeUIServices : IUIServices
    {
        public void ClearChart()
        {
            throw new NotImplementedException();
        }

        public string ConfirmOpen()
        {
            return "1.txt";
        }

        public string ConfirmSave(bool useMessageBox)
        {
            return "1.txt";
        }

        public void DrawGraphic(double[] x, double[] y, Property property)
        {
            throw new NotImplementedException();
        }
    }

    [TestClass]
    public class TestViewModelFunctions
    {

        [TestMethod]
        public void TestAddCommand()
        {
            MainViewModel mainViewModel = new MainViewModel(null);
            int count = mainViewModel.Count;
            mainViewModel.AddCommand.Execute(null);
            mainViewModel.AddCommand.Execute(null);
            mainViewModel.AddCommand.Execute(null);
            Assert.AreEqual(count + 3, mainViewModel.Count);
            mainViewModel.Nodes_count = -10;
            Assert.AreEqual(false, mainViewModel.AddCommand.CanExecute(null));
        }

        [TestMethod]
        public void TestRemoveCommand()
        {
            MainViewModel mainViewModel = new MainViewModel(null);
            Assert.AreEqual(false, mainViewModel.RemoveCommand.CanExecute(null));
        }

        [TestMethod]
        public void TestNewCommand()
        {
            FakeUIServices fakeUIServices = new FakeUIServices();
            MainViewModel mainViewModel = new MainViewModel(fakeUIServices);
            mainViewModel.AddCommand.Execute(null);
            mainViewModel.AddCommand.Execute(null);
            mainViewModel.NewCommand.Execute(null);
            Assert.AreEqual(0, mainViewModel.Count);
            mainViewModel.AddCommand.Execute(null);
            Assert.AreEqual(1, mainViewModel.Count);
        }

        [TestMethod]
        public void TestSaveCommand()
        {
            FakeUIServices fakeUIServices = new FakeUIServices();
            MainViewModel mainViewModel = new MainViewModel(fakeUIServices);
            mainViewModel.AddCommand.Execute(null);
            int count = mainViewModel.Count;
            mainViewModel.SaveCommand.Execute(fakeUIServices.ConfirmSave(false)); 
            Assert.AreEqual(count, mainViewModel.Count);
        }

        [TestMethod]
        public void TestDrawCommand()
        {
            MainViewModel mainViewModel = new MainViewModel(null);
            mainViewModel.X = 5;
            Assert.AreEqual(false, mainViewModel.DrawCommand.CanExecute(null));
        }

        [TestMethod]
        public void TestOpenCommand()
        {
            FakeUIServices fakeUIServices = new FakeUIServices();
            MainViewModel mainViewModel = new MainViewModel(fakeUIServices);
            mainViewModel.AddCommand.Execute(null);
            mainViewModel.AddCommand.Execute(null);
            int count = mainViewModel.Count;
            mainViewModel.SaveCommand.Execute(null);
            mainViewModel.NewCommand.Execute(null);
            mainViewModel.OpenCommand.Execute(null);
            Assert.AreEqual(count, mainViewModel.Count);
        }


    }
}
