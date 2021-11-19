namespace EventHorizon.Blazor.Mascot.Behaviors;

using EventHorizon.Blazor.Mascot.Character;

public class RandomSelectorBehavior
{
    public static void Run(
        MascotAvatar mascot,
        CharacterBehavior behaviorProps,
        Action? onExitCallback = null
    )
    {
        static void RandomAction(
            MascotAvatar mascot,
            CharacterBehavior behaviorProps,
            Action? onExitCallback
        )
        {
            var transition = Random.Shared.NextDouble();
            var _r = 0d;
            foreach (var state in behaviorProps.States)
            {
                if (_r <= transition
                    && _r + behaviorProps.Get(state).Prob >= transition
                )
                {
                    if (!behaviorProps.Get(state).IsExit)
                    {
                        var behavior = behaviorProps.Get(state);
                        mascot.Action(
                            behavior.Name,
                            behavior.Repeat,
                            behavior.RevH,
                            behavior.RevV,
                            behavior.RevM,
                            () => RandomAction(mascot, behaviorProps, onExitCallback)
                        );
                        return;
                    }
                    onExitCallback?.Invoke();
                    return;
                }
                _r += behaviorProps.Get(state).Prob;
            }
            onExitCallback?.Invoke();
        }

        RandomAction(mascot, behaviorProps, onExitCallback);
    }
}
