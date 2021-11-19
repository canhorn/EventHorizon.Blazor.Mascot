namespace EventHorizon.Blazor.Mascot;

public class MascotImage
{
    public MascotImage()
    {
    }

    public string Src { get; set; } = string.Empty;
    public string Left { get; set; } = "0px";
    public int LeftInPx { get; set; } = 0;
    public string Top { get; set; } = "0px";
    public int TopInPx { get; set; } = 0;
    public string ClassName { get; set; } = string.Empty;
}
