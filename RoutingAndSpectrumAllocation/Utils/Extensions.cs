using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace RoutingAndSpectrumAllocation.Utils
{
    public static class Extensions
    {
        public static T CopyObject<T>(this object objSource)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();

                formatter.Serialize(stream, objSource);

                stream.Position = 0;

                return (T)formatter.Deserialize(stream);
            }
        }
    }
}
