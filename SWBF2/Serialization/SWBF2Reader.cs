using System;
using System.IO;

namespace SWBF2.Serialization
{
    public class SWBF2Reader : StreamReader
    {
        public SWBF2Reader(Stream stream) : base(stream)
        {
        }

        public Quaternion ReadQuaternion()
        {
            var line = TrimLine(ReadLine());
            var split = line.Split(',');
            return new Quaternion(float.Parse(split[0]), float.Parse(split[1]), float.Parse(split[2]), float.Parse(split[3]));
        }

        public Vector3 ReadVector3()
        {
            var line = TrimLine(ReadLine());
            var split = line.Split(',');
            return new Vector3(float.Parse(split[0]), float.Parse(split[1]), float.Parse(split[2]));
        }

        public float ReadFloat()
        {
            var line = TrimLine(ReadLine());
            return float.Parse(line);
        }

        public int ReadInt()
        {
            var line = TrimLine(ReadLine());
            return int.Parse(line);
        }

        public T ReadEnum<T>() where T : Enum
        {
            return (T)Enum.Parse(typeof(T), ReadInt().ToString());
        }

        public string ReadString()
        {
            return TrimLine(ReadLine(), '"');
        }

        public string TrimLine(string line, char trimChar)
        {
            return TrimLine(line, trimChar, trimChar);
        }

        public string TrimLine(string line, char startChar = '(', char endChar = ')')
        {
            var startIndex = line.IndexOf(startChar) + 1;
            return line.Substring(startIndex, line.LastIndexOf(endChar) - startIndex);
        }
    }
}