namespace SWBF2
{
    public struct Quaternion
    {
        public readonly float x, y, z, w;

        public static readonly Quaternion identity = new Quaternion(0, 0, 0, 1);

        public Quaternion(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
    }
}