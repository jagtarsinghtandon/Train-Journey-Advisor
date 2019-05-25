using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Train.Classes
{
    public class MyStorage
    {
        internal static void SaveXML<T>(T data, string filename)
        {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                FileStream stream;
                stream = new FileStream(filename, FileMode.Create);
                serializer.Serialize(stream, data);
                stream.Close();
           
        }

        internal static T ReadXML<T>(string filename)
        {
            try
            {
                using (StreamReader sr = new StreamReader(filename))
                {
                    XmlSerializer xmlSer = new XmlSerializer(typeof(T));
                    return (T)xmlSer.Deserialize(sr);
                }
            }
            catch 
            {

                return default(T);
            }

        }

       
    }
}
