using System;

namespace SWBF2
{
    [Flags]
    public enum SecondaryStance
    {
        Stand = 1, Crouch = 2, Prone = 4, Left = 8, Right = 16
    }
}