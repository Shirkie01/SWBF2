using System.Drawing;

namespace SWBF2
{
    public abstract class Light
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public Quaternion Rotation;
        public Vector3 Position;
        public abstract LightType Type { get; }
        public bool CastShadow;
        public bool Static { get; } = true;
        public string TextureName { get; set; }
        public bool WrapTexture { get; set; }
        public Color Color { get; } = Color.White;
        public bool CastSpecular { get; set; }
        public bool PS2Overbright { get; set; }
    }
}