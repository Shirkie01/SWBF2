using System.IO;

namespace SWBF2.Serialization
{
    public interface ITypedFormatter<T>
    {
        T Deserialize(Stream serializationStream);

        void Serialize(Stream serializationStream, T obj);
    }
}