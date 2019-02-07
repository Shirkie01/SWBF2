using System.Collections.Generic;

namespace SWBF2
{
    public class Layer
    {
        public string Name;
        public string Description;
        public Camera Camera;
        public string LightName;
        public WorldExtents WorldExtents;
        public int Version;
        public int SaveType;

        private IList<HintNode> HintNodes { get; } = new List<HintNode>();
        private IList<Light> Lights { get; } = new List<Light>();
        private IList<Path> Paths { get; } = new List<Path>();
        private IList<Region> Regions { get; } = new List<Region>();

        public Layer(string name, int int1)
        {
            Name = name;
        }
    }
}