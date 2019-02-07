namespace SWBF2
{
    public class SpotLight : Light
    {
        public float Range { get; set; }
        public SpotLightCone Cone { get; } = new SpotLightCone();
        public PS2BlendMode PS2BlendMode { get; set; }
        public bool Bidirectional { get; set; }

        public override LightType Type => LightType.Spot;
    }
}