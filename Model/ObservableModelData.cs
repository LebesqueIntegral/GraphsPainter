using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.ComponentModel;

namespace Model
{
    [Serializable]
    public class ObservableModelData: ObservableCollection<ModelData>
    {
        public bool IsChanged { get; set; }

        public ObservableModelData() : base()
        {
            IsChanged = false;
            CollectionChanged += ObservableModelData_CollectionChanged;
        }

        public void ObservableModelData_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            IsChanged = true;
        }

        public void Add_ModelData(ModelData modelData)
        {
            base.Add(modelData);
        }

        public void Remove_ModelData(ModelData modelData)
        {
            base.Remove(modelData);
        }

        public void AddDefaults()
        {
            base.Add(new ModelData(2, 1));
            base.Add(new ModelData(20, 2.5));
            base.Add(new ModelData(11, 0.3));
        }

    }
}
