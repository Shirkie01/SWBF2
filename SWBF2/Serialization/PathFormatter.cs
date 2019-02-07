using System.Collections.Generic;
using System.IO;

namespace SWBF2.Serialization
{
    public class PathFormatter : ITypedFormatter<IList<Path>>
    {
        public IList<Path> Deserialize(Stream serializationStream)
        {
            throw new System.NotImplementedException();
        }

        public void Serialize(Stream serializationStream, IList<Path> paths)
        {
            using (var writer = new StreamWriter(new FileStream("path", FileMode.OpenOrCreate)))
            {
                writer.WriteLine("Version(10);");
                writer.WriteLine($"PathCount({paths.Count});");
                writer.WriteLine();

                foreach (Path path in paths)
                {
                    writer.WriteLine($"Path(\"{path.Name}\")");
                    writer.WriteLine("{");

                    writer.WriteLine($"\tData({path.Data});");
                    writer.WriteLine($"\tPathType({path.PathType})");
                    writer.WriteLine($"\tPathSpeedType({path.PathType})");
                    writer.WriteLine("\tPathTime({0:F6})", path.PathTime);
                    writer.WriteLine($"\tOffsetPath({path.OffsetPath})");

                    writer.WriteLine($"\tLayer({path.Layer})");
                    writer.WriteLine("\tSplineType(\"{0}\")", path.SplineType);
                    writer.WriteLine();

                    writer.WriteLine("\tProperties({0})\n\t{");
                    foreach (var p in path.Properties)
                    {
                    }
                    writer.WriteLine("\t}\n");

                    writer.WriteLine("\tNodes({0})\n\t{", path.Nodes.Count);

                    foreach (Node node in path.Nodes)
                    {
                        WriteNode(writer, node);
                    }
                    writer.WriteLine("\t}\n");
                    writer.WriteLine("}\n");
                }
            }
        }

        private void WriteNode(StreamWriter writer, Node node)
        {
            writer.WriteLine("\t\tNode()\n\t\t{");
            writer.WriteLine("\t\t\tPosition({0:F6}, {1:F6}, {2:F6});", node.Position.X, node.Position.Y, node.Position.Z);
            writer.WriteLine("\t\t\tKnot({0:F6});", node.Knot);
            writer.WriteLine("\t\t\tData({0});", node.Data);
            writer.WriteLine("\t\t\tTime({0:F6});", node.Time);
            writer.WriteLine("\t\t\tPauseTime({0:F6});", node.PauseTime);
            writer.WriteLine("\t\t\tRotation({0:F6}, {1:F6}, {2:F6}, {3:F6});", node.Rotation.W, node.Rotation.X, node.Rotation.Y, node.Rotation.Z);
            writer.WriteLine("\t\t\tProperties({0})\n\t\t\t{", node.Properties.Count);

            foreach (var p in node.Properties)
            {
                WriteProperty(writer, p);
            }

            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
        }

        private void WriteProperty(StreamWriter writer, KeyValuePair<string, float> p)
        {
            writer.WriteLine("\t\t\t\t\"{0}(\"{1}\");");
        }
    }
}