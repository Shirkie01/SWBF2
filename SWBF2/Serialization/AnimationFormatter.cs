using System.Collections.Generic;
using System.IO;

namespace SWBF2.Serialization
{
    internal class AnimationFormatter : ITypedFormatter<IList<Animation>>
    {
        public IList<Animation> Deserialize(Stream serializationStream)
        {
            throw new System.NotImplementedException();
        }

        public void Serialize(Stream serializationStream, IList<Animation> animations)
        {
            using (var writer = new StreamWriter(serializationStream))
            {
                foreach (Animation animation in animations)
                {
                    writer.WriteLine("Animation()");
                    writer.WriteLine("{");

                    foreach (var positionKey in animation.PositionKeys)
                    {
                        writer.WriteLine(
                            string.Format("AddPositionKey({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10});",
                            positionKey.Time,
                            positionKey.Position.X, positionKey.Position.Y, positionKey.Position.Z,
                            positionKey.Transition,
                            positionKey.StartSplinePosition.X, positionKey.StartSplinePosition.Y, positionKey.StartSplinePosition.Z,
                            positionKey.EndSplinePosition.X, positionKey.EndSplinePosition.Y, positionKey.EndSplinePosition.Z));
                    }
                    foreach (var rotationKey in animation.RotationKeys)
                    {
                        var angles = rotationKey.Rotation.ToEulerAngles();
                        writer.WriteLine(
                            string.Format("AddPositionKey({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10});",
                            rotationKey.Time,
                            angles.X, angles.Y, angles.Z,
                            rotationKey.Transition,
                            rotationKey.StartSplinePosition.X, rotationKey.StartSplinePosition.Y, rotationKey.StartSplinePosition.Z,
                            rotationKey.EndSplinePosition.X, rotationKey.EndSplinePosition.Y, rotationKey.EndSplinePosition.Z));
                    }
                    writer.WriteLine();
                }
            }
        }
    }
}