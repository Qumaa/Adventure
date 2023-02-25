public class Controller2DInputData
{
    private int _moveSum;

    public int Movement
    {
        get => System.Math.Sign(_moveSum);
        set => _moveSum = value;
    }
    public bool Jump { get; private set; }

    // constructor
    public Controller2DInputData()
    {
        Movement = 0;
        Jump = false;        
    }

    // methods
    public void GiveMoveInput(int signedDirection) => Movement += signedDirection;
    public void ClearMoveInput() => Movement = 0;

    public void GiveJumpInput() => Jump = true;
    public void ClearJumpInput() => Jump = false;
}
