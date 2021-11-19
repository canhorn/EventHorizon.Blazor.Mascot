namespace EventHorizon.Blazor.Mascot.Character;

public class CharacterFrame
{
    public CharacterFrame() { }
    public CharacterFrame(CharacterFrame selectedAction)
    {
        Src = selectedAction.Src;
        Anchor = selectedAction.Anchor;
        Move = selectedAction.Move;
        Duration = selectedAction.Duration;
    }

    public string Src { get; set; } = string.Empty;
    public List<int> Anchor { get; set; } = new List<int>();
    public Point Move { get; set; } = new Point();
    public int Duration { get; set; }

    public bool ReverseVert { get; set; }
    public bool ReverseHori { get; set; }
}
