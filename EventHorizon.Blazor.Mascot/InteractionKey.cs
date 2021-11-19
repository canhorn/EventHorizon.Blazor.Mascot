namespace EventHorizon.Blazor.Mascot;

public struct InteractionKey
{
    public string Value { get; }

    public InteractionKey(
        string key
    )
    {
        Value = key;
    }

    public static implicit operator InteractionKey(
        string result
    ) => new(
        result
    );
}
