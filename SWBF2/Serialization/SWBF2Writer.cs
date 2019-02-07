using System;
using System.IO;

namespace SWBF2.Serialization
{
    internal class SWBF2Writer : StreamWriter
    {
        public SWBF2Writer(Stream stream) : base(stream)
        {
        }

        public void WriteDouble(string name, double value, int indents = 1)
        {
            WriteIndents(indents);
            WriteLine("{0}({1:F6});", name, value);
        }

        public void WriteFloat(string name, float value, int indents = 1)
        {
            WriteIndents(indents);
            WriteLine("{0}({1:F6});", name, value);
        }

        public void WriteVector3(string name, Vector3 value, int indents = 1)
        {
            WriteIndents(indents);
            WriteLine("{0}({1:F6}, {2:F6}, {3:F6});", name, value.X, value.Y, value.Z);
        }

        public void WriteQuaternion(string name, Quaternion value, int indents = 1)
        {
            WriteIndents(indents);
            WriteLine("{0}({1:F6}, {2:F6}, {3:F6}, {4:F6});", name, value.W, value.X, value.Y, value.Z);
        }

        public void WriteInt(string name, int value, int indents = 1)
        {
            WriteIndents(indents);
            WriteLine("{0}({1});", name, value);
        }

        public void WriteEnum<T>(string name, T value, int indents = 1) where T : Enum
        {
            WriteInt(name, Convert.ToInt32(value), indents);
        }

        public void WriteString(string name, string value, int indents = 1)
        {
            WriteIndents(indents);
            WriteLine("{0}(\"{1}\");", name, value);
        }

        public void WriteBool(string name, int indents = 1)
        {
            WriteIndents(indents);
            WriteLine("{0}();", name);
        }

        private void WriteIndents(int indents)
        {
            for (int i = 0; i < indents; i++)
            {
                Write("\t");
            }
        }
    }
}