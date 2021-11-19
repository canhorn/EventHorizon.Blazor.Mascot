namespace EventHorizon.Blazor.Mascot.Character;

public class CharacterBehaviorState
{
    public bool IsExit { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Repeat { get; set; } = 1;
    public int RevH { get; set; }
    public int RevV { get; set; }
    public int RevM { get; set; }
    public double Prob { get; set; } = 0;
}
