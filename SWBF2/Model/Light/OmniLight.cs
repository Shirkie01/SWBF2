namespace SWBF2
{
    public class OmniLight : Light
    {
        public float Range { get; set; }

        public override LightType Type => LightType.Omni;
    }
}