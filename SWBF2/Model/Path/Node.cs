using System.Collections.Generic;

namespace SWBF2
{
    public class Node
    {
        public Vector3 Position;
        public float Knot;
        public int Data;
        public float Time;
        public float PauseTime;
        public Quaternion Rotation;
        public IDictionary<string, float> Properties { get; } = new Dictionary<string, float>();
    }
}