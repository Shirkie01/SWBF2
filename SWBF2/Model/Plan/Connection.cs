namespace SWBF2
{
    internal class Connection
    {
        public string Name { get; set; }

        public string Start { get; set; }
        public string End { get; set; }

        public int Flag { get; set; }
        public int Dynamic { get; set; }

        public bool Jump { get; set; }
        public bool JetJump { get; set; }
        public bool OneWay { get; set; }

        public Connection(string name)
        {
            Name = name;
        }
    }
}