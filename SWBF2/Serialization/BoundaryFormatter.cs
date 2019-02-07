using System.Collections.Generic;
using System.IO;

namespace SWBF2.Serialization
{
    public class BoundaryFormatter : ITypedFormatter<IList<Boundary>>
    {
        public IList<Boundary> Deserialize(Stream serializationStream)
        {
            var boundaries = new List<Boundary>();
            using (var reader = new SWBF2Reader(serializationStream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Boundary boundary = new Boundary();

                    // Open brace
                    line = reader.ReadLine();

                    while ((line = reader.ReadLine()) != null && line.Contains("Path"))
                    {
                        line = reader.TrimLine(line, '"');
                        boundary.Paths.Add(new Path(line));
                    }

                    boundaries.Add(boundary);
                }

                return boundaries;
            }

            throw new InvalidDataException("Boundary file invalid");
        }

        public void Serialize(Stream serializationStream, IList<Boundary> obj)
        {
            using (var writer = new SWBF2Writer(serializationStream))
            {
                foreach (var boundary in obj)
                {
                    writer.WriteLine("Boundary()");
                    writer.WriteLine("{");
                    foreach (var path in boundary.Paths)
                    {
                        writer.WriteString("Path", path.Name);
                    }
                    writer.WriteLine("}");
                }
            }
        }
    }
}