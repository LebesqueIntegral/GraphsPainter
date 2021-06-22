using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Model
{
    [Serializable]
    public class Serializing
    {
        public static bool Save(string filename, ObservableModelData obj)
        {
            if (filename == null)
                return false;
            FileStream fileStream = null;
            bool finish = false;
            try
            {
                fileStream = File.Create(filename);
                BinaryFormatter binaryFormatter = new BinaryFormatter();

                binaryFormatter.Serialize(fileStream, obj);
                finish = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Исключение в Save: " + ex.Message);
            }
            finally
            {
                if (fileStream != null)
                    fileStream.Close();
            }
            return finish;
        }

        public static bool Load(string filename, ref ObservableModelData obj)
        {
            if (filename == null)
                return false;
            FileStream fileStream = null;
            bool finish = false;
            try
            {
                fileStream = File.OpenRead(filename);
                BinaryFormatter binaryFormatter = new BinaryFormatter();

                obj = binaryFormatter.Deserialize(fileStream) as ObservableModelData;
                finish = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Исключение в Load: " + ex.Message);
            }
            finally
            {
                if (fileStream != null)
                    fileStream.Close();
            }
            return finish;
        }

    }
}
