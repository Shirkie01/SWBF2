﻿namespace SWBF2
{
    public struct Vector3
    {
        public readonly float x, y, z;

        public static readonly Vector3 zero = new Vector3(0, 0, 0);

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}