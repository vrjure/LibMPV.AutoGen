namespace LibMPV.AutoGen.Properties;

public class DisposeObject : IDisposable
{
    private readonly Action _disposeAction;

    public DisposeObject(Action disposeAction)
    {
        _disposeAction = disposeAction;
    }
    
    public void Dispose()
    {
        _disposeAction?.Invoke();
    }
}