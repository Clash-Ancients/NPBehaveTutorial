public class WaitUntilStopped : Task
{

    bool success = false;
    public WaitUntilStopped() : base("WaitUntilStopped")
    {
        success = false;
    }

    protected override void DoStop()
    {
        Stopped(success);
    }
}
