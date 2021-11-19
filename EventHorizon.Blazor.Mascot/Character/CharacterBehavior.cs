namespace EventHorizon.Blazor.Mascot.Character;

using System.Text.Json;
using System.Text.Json.Serialization;

public class CharacterBehavior
{
    [JsonPropertyName("_states")]
    public List<string> States { get; set; } = new List<string>();
    public Dictionary<string, CharacterBehaviorState> Data { get; set; } = new Dictionary<string, CharacterBehaviorState>();

    [JsonExtensionData]
    public Dictionary<string, JsonElement> ExtensionData { get; set; } = new Dictionary<string, JsonElement>();

    public CharacterBehaviorState Get(
        string key
    )
    {
        if (Data.ContainsKey(key))
        {
            return Data[key];
        }

        ExtensionData.TryGetValue(
            key,
            out var value
        );
        var val = JsonSerializer.Deserialize<CharacterBehaviorState>(
            value.ToString(),
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            }
        ) ?? new CharacterBehaviorState();

        Data[key] = val;

        return val;
    }

}
