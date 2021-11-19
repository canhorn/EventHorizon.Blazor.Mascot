namespace EventHorizon.Blazor.Mascot;

using EventHorizon.Blazor.Mascot.Actions;
using EventHorizon.Blazor.Mascot.Character;

public class MascotAvatar
    : IDisposable
{
    public string Name { get; internal set; }
    public Point Position { get; internal set; } = new Point();
    public int OffsetTop { get; internal set; }
    public int OffsetHeight { get; internal set; }
    public int OffsetLeft { get; internal set; }
    public int OffsetWidth { get; internal set; }
    public MascotImage Image { get; private set; } = new();
    public string Transition { get; private set; } = string.Empty;
    public CharacterConfig Config => _config;

    private Canvas Canvas { get; }
    private readonly CharacterConfig _config;
    private readonly Queue<ActionItem> _actionQueue = new();
    private readonly Timer _timer;
    private readonly Dictionary<string, BehaviorItem> _behaviors = new();
    private bool _isTimerRunning;
    private CharacterFrame _currentFrame = new();

    public MascotAvatar(
        Canvas canvas,
        CharacterConfig config
    )
    {
        Canvas = canvas;
        _config = config;
        Name = _config.Name;

        // Create time, not running
        _isTimerRunning = false;
        _timer = new(
            HandleCallback,
            this,
            Timeout.Infinite,
            0
        );
    }

    public void Behavior(
        string trigger,
        StandardAction action
    )
    {
        _behaviors.Add(
            trigger,
            new BehaviorItem(
                BehaviorItemType.Action,
                action.Run
            //(interactionKey,
            //    environment
            //) => action.Run(
            //    interactionKey,
            //    environment
            //)
            )
        );
    }

#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
    public void Dispose()
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
    {
        _timer.Dispose();
    }

    public void Init(
        int x,
        int y
    )
    {
        Position = new Point(x, y);
    }

    public void Action(
        string action,
        int repeat = -1,
        int reverseH = 0,
        int reverseV = 0,
        int reverseM = 0,
        Action? onEnd = null
    )
    {
        if (!_config.Actions.ContainsKey(action))
        {
            return;
        }

        for (int i = 0; i < repeat; i++)
        {
            for (int index = 0; index < _config.Actions[action].Count; index++)
            {
                var frame = new CharacterFrame(
                    _config.Actions[action][index]
                )
                {
                    ReverseVert = reverseV != 0,
                    ReverseHori = reverseH != 0
                };

                if (reverseM != 0)
                {
                    frame.Move = frame.Move.Inverse();
                }

                _actionQueue.Enqueue(
                    new ActionItem(
                        ActionItemType.Frame,
                        null,
                        frame
                    )
                );
            }
        }

        if (onEnd != null)
        {
            _actionQueue.Enqueue(
                new ActionItem(
                    ActionItemType.Function,
                    onEnd,
                    null
                )
            );
        }

        if (!_isTimerRunning)
        {
            StartTimer();
        }
    }

    private void StartTimer()
    {
        _isTimerRunning = true;
        _ = _timer.Change(0, 0);
    }

    private void StopTimer()
    {
        _ = _timer.Change(Timeout.Infinite, Timeout.Infinite);
        _isTimerRunning = false;
    }

    private void OnChange()
    {
        Canvas.TriggerChange();
    }

    private static void HandleCallback(object? state)
    {
        if (state is not MascotAvatar Mascot)
        {
            return;
        }
        Mascot.StopTimer();

        if (Mascot._actionQueue.Count == 0)
        {
            return;
        }


        var action = Mascot._actionQueue.Dequeue();
        if (action == null)
        {
            return;
        }

        if (action.Type == ActionItemType.Function
            && action.Action != null
        )
        {
            action.Action();
            Mascot.OnChange();
            return;
        }

        if (action.Type == ActionItemType.Frame
            && action.Frame != null
        )
        {
            Mascot.Pose(
                action.Frame
            );
            Mascot._timer.Change(
                action.Frame.Duration,
                Timeout.Infinite
            );
            Mascot.OnChange();
            return;
        }
    }

    private void Pose(
        CharacterFrame frame
    )
    {
        _currentFrame = frame;

        var lastPosition = new Point(Position);
        Position = new Point(
            Position.X + ((frame.ReverseHori ? -1 : 1) * frame.Move.X),
            Position.Y + ((frame.ReverseVert ? -1 : 1) * frame.Move.Y)
        );

        Interact(
            Canvas.Environment,
            lastPosition
        );

        Update();
    }

    private void Update()
    {
        Image = new MascotImage()
        {
            Src = _config.BaseUrl + _currentFrame.Src,
            Left = -_currentFrame.Anchor[0] + "px",
            LeftInPx = -_currentFrame.Anchor[0],
            Top = -_currentFrame.Anchor[1] + "px",
            TopInPx = -_currentFrame.Anchor[1],
            ClassName = (_currentFrame.ReverseVert ? "reverse-vertical" : string.Empty)
                + " "
                + (_currentFrame.ReverseHori ? "reverse-horizontal" : string.Empty)
        };
        Transition = $"top {_currentFrame.Duration}ms, left {_currentFrame.Duration}ms linear";
    }


    public void Interact(
        CanvasEnvironment canvasEnvironment,
        Point lastLocation
    )
    {
        foreach (var environment in canvasEnvironment.Items)
        {
            if (Position.Equals(lastLocation))
            {
                continue;
            }

            var interactionKey = CanvasEnvironment.Interact(
                environment,
                Position,
                lastLocation
            );

            if (_behaviors.TryGetValue(
                environment.Name,
                out var environmentBehavior
            ))
            {
                environmentBehavior.Action(
                    interactionKey,
                    environment
                );
            }
            else if (_behaviors.TryGetValue(
                interactionKey.Value,
                out var behaviorItem
            ))
            {
                if (behaviorItem.Type == BehaviorItemType.Action)
                {
                    behaviorItem.Action(
                        interactionKey,
                        environment
                    );
                    return;
                }

                Action(
                    interactionKey.Value
                );
            }
        }
    }

    internal void Place(
        int x,
        int y
    )
    {
        Position = new Point(x, y);
    }

    internal void CancelAction()
    {
        _actionQueue.Clear();
    }
}
