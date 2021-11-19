namespace EventHorizon.Blazor.Mascot;

public record BehaviorItem(
    BehaviorItemType Type,
    Func<InteractionKey, EnvironmentItem, Task> Action
);
