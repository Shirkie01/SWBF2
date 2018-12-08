using System.Collections.Generic;
using System.IO;

namespace SWBF2.Serialization
{
    public class BarrierFormatter : ITypedFormatter<IList<Barrier>>
    {
        public IList<Barrier> Deserialize(Stream serializationStream)
        {
            IList<Barrier> barriers = new List<Barrier>();

            using (var reader = new StreamReader(serializationStream))
            {
                var line = reader.ReadLine();
                var count = int.Parse(line.Substring(line.IndexOf("(" + 1), line.IndexOf(")")));

                // Blank
                reader.ReadLine();

                for (int i = 0; i < count; i++)
                {
                    line = reader.ReadLine();
                    var name = line.Substring(line.IndexOf("\"") + 1, line.LastIndexOf("\""));
                    var barrier = new Barrier(name);

                    if (!"{".Equals(reader.ReadLine()))
                    {
                        throw new InvalidDataException("File corrupted at " + reader.BaseStream.Position);
                    }

                    for (int j = 0; j < 4; j++)
                    {
                        barrier.Corners.Add(ReadCorner(reader));
                    }

                    line = reader.ReadLine();

                    barrier.Flag = int.Parse(line.Substring(line.IndexOf("(") + 1, line.IndexOf(")")));

                    if (!"}".Equals(reader.ReadLine()))
                    {
                        throw new InvalidDataException("File corrupted at " + reader.BaseStream.Position);
                    }

                    barriers.Add(barrier);

                    reader.ReadLine();
                }
            }

            return barriers;
        }

        private Vector3 ReadCorner(StreamReader reader)
        {
            var line = reader.ReadLine();
            line.Substring(line.IndexOf("("), line.IndexOf(")"));
            var positions = line.Split(', ');
            return new Vector3(float.Parse(positions[0]), float.Parse(positions[1]), float.Parse(positions[2]));
        }

        public void Serialize(Stream serializationStream, IList<Barrier> barriers)
        {
            using (var writer = new StreamWriter(serializationStream))
            {
                writer.WriteLine(string.Format("BarrierCount({0});", barriers.Count));
                writer.WriteLine();

                foreach (var barrier in barriers)
                {
                    writer.WriteLine(string.Format("Barrier(\"{0}\")", barrier.Name));
                    writer.WriteLine("{");

                    foreach (var corner in barrier.Corners)
                    {
                        writer.WriteLine(string.Format("\tCorner({0}, {1}, {2});", corner.x, corner.y, corner.z));
                    }

                    writer.WriteLine(string.Format("\tFlag({0});", barrier.Flag));
                    writer.WriteLine("}");
                    writer.WriteLine();
                }
            }
        }
    }
}