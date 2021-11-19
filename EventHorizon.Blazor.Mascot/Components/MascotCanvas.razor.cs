namespace EventHorizon.Blazor.Mascot.Components;

using Microsoft.AspNetCore.Components;

public partial class MascotCanvas
{
    [Parameter]
    public CanvasEnvironment Environment { get; set; } = null!;
    [Parameter]
    public MascotAvatar Avatar { get; set; } = null!;
    [Parameter]
    public SpeechBubbleDetails SpeechBubbleDetails { get; set; } = new SpeechBubbleDetails();
}
