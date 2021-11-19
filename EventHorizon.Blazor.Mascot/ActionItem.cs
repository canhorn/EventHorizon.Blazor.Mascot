namespace EventHorizon.Blazor.Mascot;

using EventHorizon.Blazor.Mascot.Character;

public record ActionItem(ActionItemType Type, Action? Action, CharacterFrame? Frame);
