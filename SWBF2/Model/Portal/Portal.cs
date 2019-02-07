namespace SWBF2
{
    public class Portal
    {
        public string Name { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Position { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public Sector Sector1 { get; set; }
        public Sector Sector2 { get; set; }
    }
}