using System.Collections.Generic;

namespace SWBF2
{
    public class Hierarchy
    {
        public string Name { get; set; }
        public IList<GameObject> Objects { get; } = new List<GameObject>();
    }
}