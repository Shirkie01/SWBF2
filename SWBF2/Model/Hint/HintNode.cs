using System.Collections.Generic;

namespace SWBF2
{
    public class HintNode
    {
        public string Name { get; set; }
        public HintType Type { get; set; }
        public Vector3 Position { get; set; } = Vector3.Zero;
        public Quaternion Rotation { get; set; } = Quaternion.Identity;
        public double Radius { get; set; }

        public PrimaryStance PrimaryStance { get; set; }
        public SecondaryStance SecondaryStance { get; set; }

        public HintMode HintMode { get; set; }

        public string CommandPost { get; set; }
        public string Building { get; set; }
        public string Target { get; set; }

        public IList<string> Connections { get; } = new List<string>();

        public HintNode(string name, HintType type)
        {
            Name = name;
            Type = type;
        }
    }
}