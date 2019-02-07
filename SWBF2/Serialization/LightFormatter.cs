using System.Collections.Generic;
using System.IO;

namespace SWBF2.Serialization
{
    public class LightFormatter : ITypedFormatter<IList<Light>>
    {
        public IList<Light> Deserialize(Stream serializationStream)
        {
            IList<Light> lights = new List<Light>();
            using (var reader = new StreamReader(serializationStream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var startIndex = line.IndexOf("\"");
                    var lightName = line.Substring(startIndex, line.IndexOf("\"") - startIndex);

                    startIndex = line.IndexOf(", ");
                    var lightId = int.Parse(line.Substring(startIndex, line.LastIndexOf(")") - startIndex));

                    // {
                    reader.ReadLine();

                    line = reader.ReadLine();
                    var lightRotation = Quaternion.Parse(line);

                    line = reader.ReadLine();
                    var lightPosition = Vector3.Parse(line);

                    line = reader.ReadLine();
                    startIndex = line.IndexOf("(");
                    var lightType = (LightType)int.Parse(line.Substring(startIndex, line.LastIndexOf(")") - startIndex));

                    Light light = null;
                    switch (lightType)
                    {
                        case LightType.Spot:
                            light = new SpotLight();
                            break;

                        case LightType.Omni:
                            light = new OmniLight();
                            break;

                        case LightType.Directional:
                            light = new DirectionalLight();
                            break;
                    }

                    light.Name = lightName;
                    light.Id = lightId;
                    light.Rotation = lightRotation;
                    light.Position = lightPosition;

                    line = reader.ReadLine();

                    //light.Color = ;

                    // }
                    reader.ReadLine();
                }
            }
            return lights;
        }

        public void Serialize(Stream serializationStream, IList<Light> lights)
        {
            using (var writer = new StreamWriter(serializationStream))
            {
                foreach (var light in lights)
                {
                    writer.WriteLine(string.Format("Light(\"{0}\", {1})", light.Name, light.Id));
                    writer.WriteLine("{");
                    writer.WriteLine(string.Format("\tRotation({0});", light.Rotation.ToString()));
                    writer.WriteLine(string.Format("\tPosition({0});", light.Position.ToString()));
                    writer.WriteLine(string.Format("\tType({0});", light.Type));
                    writer.WriteLine(string.Format("\tColor({0});", light.Color));

                    if (light.Type == LightType.Directional)
                    {
                        var directionalLight = (DirectionalLight)light;
                        if (directionalLight.CastShadow)
                        {
                            writer.WriteLine("\tCastShadow();");
                        }

                        if (directionalLight.CastSpecular)
                        {
                            writer.WriteLine("\tCastSpecular(1);");
                        }

                        if (directionalLight.Static)
                        {
                            writer.WriteLine("\tStatic();");
                        }

                        if (directionalLight.BoundingRegion != null)
                        {
                            writer.WriteLine(string.Format("\tRegion(\"{0}\")", directionalLight.BoundingRegion.Name));
                        }

                        writer.WriteLine(string.Format("\tPS2BlendMode({0})", directionalLight.PS2BlendMode));

                        writer.WriteLine(string.Format("\tTileUV({0}, {1})", directionalLight.TileUV.X, directionalLight.TileUV.Y));
                        writer.WriteLine(string.Format("\tOffsetUV({0}, {1})", directionalLight.OffsetUV.X, directionalLight.OffsetUV.Y));
                    }
                    else if (light.Type == LightType.Omni)
                    {
                        writer.WriteLine(string.Format("\tRange({0})", ((OmniLight)light).Range));
                    }

                    writer.WriteLine("}");
                }
            }
        }
    }
}