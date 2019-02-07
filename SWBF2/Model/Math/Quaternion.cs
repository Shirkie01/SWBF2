using System;
using System.Text.RegularExpressions;

namespace SWBF2
{
    public struct Quaternion
    {
        public readonly double W, X, Y, Z;

        public static readonly Quaternion Identity = new Quaternion(0, 0, 0, 1);

        /// <summary>
        /// Parses doubles
        /// </summary>
        private static readonly Regex r = new Regex(@"(-?)(0|([1-9][0-9]*))(\.[0-9]+)?");

        public Quaternion(double w, double x, double y, double z)
        {
            this.W = w;
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public static Quaternion Parse(string s)
        {
            var values = r.Matches(s);

            if (values.Count != 4)
            {
                throw new FormatException("Quaternion must have 4 matches for regex: " + r.ToString());
            }

            var x = double.Parse(values[0].Value);
            var y = double.Parse(values[1].Value);
            var z = double.Parse(values[2].Value);
            var w = double.Parse(values[3].Value);

            return new Quaternion(w, x, y, z);
        }

        public override string ToString()
        {
            return string.Format("{0:F6}, {1:F6}, {2:F6}, {3:F6}", W, X, Y, Z);
        }

        public Vector3 ToEulerAngles()
        {
            var eulerX = Math.Atan2(2 * (W * X + Y * Z), 1 - 2 * (X * X + Y * Y));
            var eulerY = Math.Asin(2 * (W * Y - Z * X));
            var eulerZ = Math.Atan2(2 * (W * Z + X * Y), 1 - 2 * (Y * Y + Z * Z));

            return new Vector3(eulerX, eulerY, eulerZ);
        }

        public static Quaternion FromEulerAngles(double yaw, double pitch, double roll)
        {
            // Abbreviations for the various angular functions
            double cy = Math.Cos(yaw * 0.5);
            double sy = Math.Sin(yaw * 0.5);
            double cp = Math.Cos(pitch * 0.5);
            double sp = Math.Sin(pitch * 0.5);
            double cr = Math.Cos(roll * 0.5);
            double sr = Math.Sin(roll * 0.5);

            var qw = cy * cp * cr + sy * sp * sr;
            var qx = cy * cp * sr - sy * sp * cr;
            var qy = sy * cp * sr + cy * sp * cr;
            var qz = sy * cp * cr - cy * sp * sr;
            return new Quaternion(qw, qx, qy, qz);
        }
    }
}