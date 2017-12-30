
public enum Target
{
    Left = -1,
    Right = 1
}
public class UFO
{
    public UFO_IdleState Idle = new UFO_IdleState();
    public UFO_HuntState Hunt = new UFO_HuntState();

}
public class Animal
{
    public ANI_Idle idle = new ANI_Idle();
    public ANI_Traped Traped = new ANI_Traped();
    public ANI_Saved Saved = new ANI_Saved();
    public ANI_Cheering Cheering = new ANI_Cheering();
    public ANI_Cry Cry = new ANI_Cry();
}
public enum AnimalState
{
    Idle,
    Help,
    Saved,
    Success,
    Fail,
}
