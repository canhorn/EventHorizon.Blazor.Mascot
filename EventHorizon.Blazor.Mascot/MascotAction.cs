namespace EventHorizon.Blazor.Mascot;

public abstract class MascotAction
{
    protected readonly string trigger;
    protected readonly MascotAvatar _avatar;

    public MascotAction(
        string trigger,
        MascotAvatar avatar
    )
    {
        this.trigger = trigger;
        this._avatar = avatar;
    }

    public abstract Task Run(
        InteractionKey interactionKey,
        EnvironmentItem environment
    );
}
