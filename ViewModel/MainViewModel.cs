using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Model;

namespace ViewModel
{
    public interface IUIServices
    {
        string ConfirmSave(bool useMessageBox);

        string ConfirmOpen();

        void DrawGraphic(double[] x, double[] y, Property property);

        void ClearChart();

    }

    public class Property
    {
        public string name;

        public string format;

        public string metadata;

        public Property(string s1 , string s2 = null, string s3 = null)
        {
            name = s1;
            format = s2;
            metadata = s3;
        }
    }

    public delegate void SimpleFunc();

    public class MainViewModel: IDataErrorInfo
    {

        public event SimpleFunc UpdateCollectionReference;

            
        private IUIServices uIServices;

        public bool IsValidAdd { get; set; }

        public bool IsValidDraw { get; set; }

        public double X { get; set; }

        public double P { get; set; }

        public int Nodes_count { get; set; }

        public string Format { get; set; }

        public int Count { get { return observableData.Count; } }

        public string Error { get { return "Error ViewModel.Validation"; } }

        public string this[string property]
        {
            get
            {
                string msg = null;
                switch (property)
                {
                    case "Nodes_count":
                        if (Nodes_count > Model.ModelData.nMax || Nodes_count < Model.ModelData.nMin)
                            msg = "Nodes counts must be less nMax and greater nMin";
                        break;
                    case "P":
                        if (P > Model.ModelData.pMax || P < Model.ModelData.pMin)
                            msg = "Parameter P must be less pMax and greater pMin";
                        break;
                    case "X":
                        if (X > 1 || X < 0)
                            msg = "Argument X counts must be less 1 and greater 0";
                        break;
                    default:
                        break;
                }
                return msg;
            }
        }


        public static double pMin { get => ModelData.pMin; }

        public static double pMax { get => ModelData.pMin; }

        public static int nMin { get => ModelData.nMin; }

        public static int nMax { get => ModelData.nMax; }


        public ObservableModelData observableData = new ObservableModelData();

        public IEnumerable ObservableData { get => observableData; private set { } }


        private readonly ICommand addCommand;

        private readonly ICommand newCommand;

        private readonly ICommand saveCommand;

        private readonly ICommand openCommand;

        private readonly ICommand removeCommand;

        private readonly ICommand drawCommand;

        public ICommand AddCommand { get => addCommand; }
        public ICommand NewCommand { get => newCommand; }
        public ICommand SaveCommand { get => saveCommand; }
        public ICommand OpenCommand { get => openCommand; }
        public ICommand RemoveCommand { get => removeCommand; }
        public ICommand DrawCommand { get => drawCommand; }

        public MainViewModel(IUIServices uIServices)
        {
            this.uIServices = uIServices;
            P = 0;
            Nodes_count = 2;
            X = 0.1;
            IsValidAdd = true;
            IsValidDraw = true;
            Format = "F1";

            addCommand = new RelayCommand(_ => this["Nodes_count"] != "Nodes counts must be less nMax and greater nMin" && this["P"] != "Parameter P must be less pMax and greater pMin",
                _ => observableData.Add_ModelData(new ModelData(Nodes_count, P))); 
            
            newCommand = new RelayCommand(_ => true, _ => { if (observableData.IsChanged)  Serializing.Save(uIServices.ConfirmSave(true), observableData); 
                    observableData.Clear(); observableData.IsChanged = false; });
            
            saveCommand = new RelayCommand(_=> observableData.IsChanged, _=> { Serializing.Save(uIServices.ConfirmSave(false), observableData); 
                observableData.IsChanged = false; } );
            
            openCommand = new RelayCommand(_ => true, _=>
            {
                if (observableData.IsChanged) Serializing.Save(uIServices.ConfirmSave(true), observableData);
                Serializing.Load(uIServices.ConfirmOpen(), ref observableData); observableData.IsChanged = false;
                observableData.CollectionChanged += observableData.ObservableModelData_CollectionChanged; UpdateCollectionReference?.Invoke();
            } );

            removeCommand = new RelayCommand(param => (param != null) && ((param as IList<object>).Count() > 0), 
                param => { while ((param as IList<object>).Count() > 0) observableData.Remove_ModelData((param as IList<object>)[0] as ModelData); });

            drawCommand = new RelayCommand(param => this["X"] != "Argument X counts must be less 1 and greater 0" && ((param as IList<object>).Count() > 0),
                param => { uIServices.ClearChart();  foreach (var item in (param as IList<object>)) ExtractAndReceiveData(item as ModelData); CollectParameters(); });
                
        }

        private void ExtractAndReceiveData(ModelData modelData)
        {
            uIServices.DrawGraphic(modelData.Nodes, modelData.Function_Values, new Property("P = " + modelData.P, Format, "Area 1"));
        }

        private void CollectParameters()
        {
            List<double> Parameter = new List<double>();
            List<double> Func = new List<double>();
            foreach (var item in observableData)
            {
                Parameter.Add(item.P);
                Func.Add(Interpolate.F(X, item)); 
            }

            uIServices.DrawGraphic(Parameter.ToArray(), Func.ToArray(), new Property("", Format, "Area 2"));
        }

    }
}
