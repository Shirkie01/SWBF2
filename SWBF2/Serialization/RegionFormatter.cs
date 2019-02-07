using System.Collections.Generic;
using System.IO;

namespace SWBF2.Serialization
{
    public class RegionFormatter : ITypedFormatter<IList<Region>>
    {
        public IList<Region> Deserialize(Stream serializationStream)
        {
            IList<Region> regions = new List<Region>();
            using (var reader = new SWBF2Reader(serializationStream))
            {
                // version
                var line = reader.ReadLine();

                // count
                int regionCount = reader.ReadInt();

                reader.ReadLine();

                for (int i = 0; i < regionCount; i++)
                {
                    // region name and id
                    line = reader.ReadLine();

                    var startIndex = line.IndexOf("\"") + 1;
                    string regionType = line.Substring(startIndex, line.LastIndexOf("\"") - startIndex);

                    startIndex = line.IndexOf(", ") + 1;
                    line = line.Substring(startIndex, line.LastIndexOf(")") - startIndex);
                    int id = int.Parse(line);

                    Region region = new Region(regionType, id);

                    // {
                    reader.ReadLine();

                    line = reader.ReadLine();
                    if (line.Contains("Layer"))
                    {
                        startIndex = line.IndexOf("(") + 1;
                        region.LayerId = int.Parse(line.Substring(startIndex, line.IndexOf(")") - startIndex));
                        line = reader.ReadLine();
                    }

                    // Needs to be parsed due to optional parameter above
                    region.Position = Vector3.Parse(line);

                    region.Rotation = reader.ReadQuaternion();
                    region.Size = reader.ReadVector3();

                    line = reader.ReadLine();

                    if (line.Contains("Name"))
                    {
                        startIndex = line.IndexOf("\"") + 1;
                        line = line.Substring(startIndex, line.LastIndexOf("\"") - startIndex);
                        region.Name = line;
                        line = reader.ReadLine();
                    }

                    if (line.Contains("NextIsGrouped"))
                    {
                        region.NextIsGrouped = true;
                        line = reader.ReadLine();
                    }

                    // }
                    if ("}" != line)
                    {
                        throw new InvalidDataException($"End of region expected. '{line}'");
                    }

                    // blank line
                    line = reader.ReadLine();

                    regions.Add(region);
                }
            }

            return regions;
        }

        public void Serialize(Stream serializationStream, IList<Region> regions)
        {
            using (var writer = new SWBF2Writer(serializationStream))
            {
                writer.WriteLine("Version(1);");
                writer.WriteLine($"RegionCount({regions.Count});");

                foreach (var region in regions)
                {
                    writer.WriteLine();
                    writer.WriteLine($"Region(\"{region.RegionType}\", {region.Id})");
                    writer.WriteLine("{");

                    if (region.LayerId != -1)
                    {
                        writer.WriteLine(string.Format("\tLayer({0});", region.LayerId));
                    }

                    writer.WriteVector3(nameof(region.Position), region.Position);
                    writer.WriteQuaternion(nameof(region.Rotation), region.Rotation);
                    writer.WriteVector3(nameof(region.Size), region.Size);

                    // We print if it is empty, but not null
                    if (region.Name != null)
                    {
                        writer.WriteString("Name", region.Name);
                    }

                    if (region.NextIsGrouped)
                    {
                        writer.WriteBool("NextIsGrouped");
                    }
                    writer.WriteLine("}");
                }
            }
        }
    }
}