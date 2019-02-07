using System.Collections.Generic;
using System.IO;

namespace SWBF2.Serialization
{
    public class HintFormatter : ITypedFormatter<IList<HintNode>>
    {
        public IList<HintNode> Deserialize(Stream serializationStream)
        {
            var hints = new List<HintNode>();
            using (var reader = new SWBF2Reader(serializationStream))
            {
                // Blank
                var line = reader.ReadLine();

                while ((line = reader.ReadLine()) != null)
                {
                    // Constructor
                    line = reader.TrimLine(line);
                    line = line.Replace("\"", "").Replace(", ", ",");
                    var cctorValues = line.Split(',');

                    var hint = new HintNode(cctorValues[0], (HintType)int.Parse(cctorValues[1]));

                    // Open Brace
                    line = reader.ReadLine();

                    // Position
                    hint.Position = reader.ReadVector3();

                    // Rotation
                    hint.Rotation = reader.ReadQuaternion();

                    // Radius if available
                    line = reader.ReadLine();
                    if (line.Contains("Radius"))
                    {
                        hint.Radius = float.Parse(reader.TrimLine(line));
                        line = reader.ReadLine();
                    }

                    // PrimaryStance if available
                    if (line.Contains("PrimaryStance"))
                    {
                        hint.PrimaryStance = (PrimaryStance)(int)(double.Parse(reader.TrimLine(line)));
                        line = reader.ReadLine();
                    }

                    // SecondaryStance if available
                    if (line.Contains("SecondaryStance"))
                    {
                        hint.SecondaryStance = (SecondaryStance)int.Parse(reader.TrimLine(line));
                        line = reader.ReadLine();
                    }

                    hint.HintMode = (HintMode)int.Parse(reader.TrimLine(line));

                    // CommandPost if available, closing brace otherwise
                    line = reader.ReadLine();
                    if (line.Contains("CommandPost"))
                    {
                        hint.CommandPost = reader.TrimLine(line, '"');
                        reader.ReadLine();
                    }

                    hints.Add(hint);

                    // Space
                    reader.ReadLine();
                }

                return hints;
            }

            throw new InvalidDataException("Could not correctly parse hints.");
        }

        public void Serialize(Stream serializationStream, IList<HintNode> obj)
        {
            using (var writer = new SWBF2Writer(serializationStream))
            {
                foreach (var hint in obj)
                {
                    writer.WriteLine();
                    writer.WriteLine(string.Format("Hint(\"{0}\", \"{1}\")", hint.Name, (int)hint.Type));
                    writer.WriteLine("{");
                    writer.WriteVector3("Position", hint.Position);
                    writer.WriteQuaternion("Rotation", hint.Rotation);

                    if (hint.Radius != 0)
                    {
                        writer.WriteDouble("Radius", hint.Radius);
                    }

                    if (hint.PrimaryStance != PrimaryStance.None)
                    {
                        writer.WriteEnum("PrimaryStance", hint.PrimaryStance);
                    }

                    if (hint.SecondaryStance != SecondaryStance.None)
                    {
                        writer.WriteEnum("SecondaryStance", hint.SecondaryStance);
                    }

                    writer.WriteEnum("Mode", hint.HintMode);

                    if (!string.IsNullOrWhiteSpace(hint.CommandPost))
                    {
                        writer.WriteString("CommandPost", hint.CommandPost);
                    }

                    writer.WriteLine("}");
                }
            }
        }
    }
}