namespace EventHorizon.Blazor.Mascot.Character;
public class CharacterConfig
{
    public string Name { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;

    public Dictionary<string, List<CharacterFrame>> Actions { get; set; } = new();
    public Dictionary<string, CharacterBehavior> Behavior { get; set; } = new();
}
