using System.Collections.Generic;

namespace SWBF2
{
    public class Path
    {
        public string Name;
        public int Data;
        public PathType PathType;
        public PathSpeedType PathSpeedType;
        public float PathTime;
        public int OffsetPath;
        public int Layer;
        public SplineType SplineType;

        public IDictionary<string, float> Properties { get; } = new Dictionary<string, float>();

        public IList<Node> Nodes { get; } = new List<Node>();

        public Path(string name)
        {
            Name = name;
        }
    }
}