﻿namespace SWBF2
{
    public abstract class AnimationKey
    {
        public float Time;
        public AnimationTransitionType Transition;
        public Vector3 StartSplinePosition;
        public Vector3 EndSplinePosition;
    }
}