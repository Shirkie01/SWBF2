using System;

namespace SWBF2
{
    [Flags]
    public enum MovementType
    {
        Soldier = 1, Hover = 2, Small = 4, Medium = 8, Huge = 16, Flyer = 32
    }
}