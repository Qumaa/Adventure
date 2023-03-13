namespace Project.Controller2D
{
    public interface IGroundSensorPlayer : IGroundSensor
    {
        FiltersHolderPlayer Filters { get; }
        bool Slope { get; }
        bool GroundOrSlope { get; }
    }
}