using System;

namespace Project.Controller2D
{
    public interface IGroundSensor
    {        
        bool Anything { get; }
        bool Ceiling { get; }
        bool Ground { get; }
        bool Left { get; }
        bool Right { get; }

        event Action OnDataChanged;
    }
}