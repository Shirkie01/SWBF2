using System.Collections.Generic;
using System.Drawing;

namespace SWBF2
{
    public class Sector
    {
        public string Name { get; set; }
        public float Base { get; set; }
        public float Height { get; set; }
        public IList<PointF> Points { get; } = new List<PointF>();
        public IList<GameObject> Objects { get; } = new List<GameObject>();
    }
}