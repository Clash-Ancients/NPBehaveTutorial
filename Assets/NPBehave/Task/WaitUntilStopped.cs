public class WaitUntilStopped : Task
{
  
    public WaitUntilStopped() : base("WaitUntilStopped")
    {
    }

    protected override void DoStop()
    {
        Stopped();
    }
}
