namespace EventHorizon.Blazor.Mascot;

public class Canvas
{
    public delegate void OnChangeHandler();

    public CanvasEnvironment Environment { get; }
    public event OnChangeHandler OnChange = () => { };


    public Canvas(
        CanvasEnvironment environment
    )
    {
        Environment = environment;
    }

    public void TriggerChange()
    {
        OnChange();
    }
}
