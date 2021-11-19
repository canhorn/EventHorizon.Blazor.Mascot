namespace EventHorizon.Blazor.Mascot.Components;

using Microsoft.AspNetCore.Components;

public partial class SpeechBubble
{
    [Parameter]
    public bool AsMarquee { get; set; }

    [Parameter]
    public int PositionX { get; set; }

    [Parameter]
    public int PositionY { get; set; }

    [Parameter]
    public int OffsetLeft { get; set; }

    [Parameter]
    public int OffsetTop { get; set; }

    [Parameter]
    public string Transition { get; set; } = string.Empty;
    [Parameter]
    public string BubbleType { get; set; } = "normal";
    [Parameter]
    public string Text { get; set; } = string.Empty;

    private double _topOffset = 1.75;
    private double _leftOffset = 30;

    protected override void OnInitialized()
    {
        base.OnInitialized();
        Setup();
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        Setup();
    }

    private void Setup()
    {
        _topOffset = 1.5;
        _leftOffset = 30;
        if (AsMarquee)
        {
            return;
        }

        // This is awesome! This is
        var lines = Math.Floor((double)(Text.Length / 24));
        for (var i = 0; i < lines; i++)
        {
            _topOffset += 0.2;
        }
    }
}
