using System.IO;
using System.Xml.Serialization;

namespace Adas.Core
{
    public static class SerializationHelper<T>
    {
        #region Public Methods

        public static void XmlSerialize(T obj, string filepath)
        {
            var serializer = new XmlSerializer(typeof (T));
            using (var stream = new StreamWriter(filepath, false))
            {
                serializer.Serialize(stream, obj);
            }
        }

        public static T XmlDeserialize(string filepath)
        {
            var serializer = new XmlSerializer(typeof (T));
            using (var stream = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                return (T) serializer.Deserialize(stream);
            }
        }

        #endregion
    }
}