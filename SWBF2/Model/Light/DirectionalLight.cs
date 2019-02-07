using System.Drawing;

namespace SWBF2
{
    public class DirectionalLight : Light
    {
        public PointF TileUV { get; } = new PointF();
        public PointF OffsetUV { get; } = new PointF();
        public Region BoundingRegion { get; set; }
        public PS2BlendMode PS2BlendMode { get; set; }

        public override LightType Type => LightType.Directional;
    }
}