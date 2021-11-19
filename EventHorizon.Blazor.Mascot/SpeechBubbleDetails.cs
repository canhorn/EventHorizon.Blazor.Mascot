namespace EventHorizon.Blazor.Mascot;

public class SpeechBubbleDetails
{
    public bool Show { get; set; }
    public bool AsMarquee { get; set; }
    public string Type { get; set; } = "normal";
    public string Text { get; set; } = string.Empty;
}
