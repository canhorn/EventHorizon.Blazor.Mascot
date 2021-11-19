namespace EventHorizon.Blazor.Mascot;

using System.Diagnostics.CodeAnalysis;

public class CanvasEnvironment
{
    private readonly List<MascotAvatar> _characters = new();
    private readonly List<EnvironmentItem> _environment = new();

    public int Height { get; }
    public int Width { get; }
    public IEnumerable<EnvironmentItem> Items => _environment;

    public CanvasEnvironment(
        string name,
        int height,
        int width
    )
    {
        Height = height;
        Width = width;

        _environment.Add(
            new EnvironmentItem(
                name,
                0,
                0,
                height,
                width
            )
        );
    }

    public void Make(
        [NotNull] IList<MascotAvatar> characters
    )
    {
        foreach (var character in characters)
        {
            _characters.Add(
                character
            );
            _environment.Add(
                new EnvironmentItem(
                    character.Name,
                    character.OffsetTop,
                    character.OffsetTop + character.OffsetHeight,
                    character.OffsetLeft,
                    character.OffsetLeft + character.OffsetWidth
                )
            );
        }
    }

    public static InteractionKey Interact(
        EnvironmentItem environment,
        Point currentPoint,
        Point nextPoint
    )
    {
        var interact = new List<string>();

        if (IsEnv(environment, nextPoint))
        {
            interact.Add(
                "exit"
            );

            if (currentPoint.Y >= environment.Bottom
                && nextPoint.Y < environment.Bottom
            )
            {
                interact.Add("bottom");
            }
            else if (currentPoint.Y <= environment.Top
                && nextPoint.Y > environment.Top
            )
            {
                interact.Add("top");
            }

            if (currentPoint.X >= environment.Right
                && nextPoint.X < environment.Right
            )
            {
                interact.Add("right");
            }
            else if (currentPoint.X <= environment.Left
                && nextPoint.X > environment.Left
            )
            {
                interact.Add("left");
            }
            return string.Join(
                "_",
                interact
            );
        }

        interact.Add("enter");
        if (currentPoint.Y <= environment.Bottom &&
            nextPoint.Y > environment.Bottom
        )
        {
            interact.Add("bottom");
        }
        else if (currentPoint.Y >= environment.Top
            && nextPoint.Y < environment.Top
        )
        {
            interact.Add("top");
        }

        if (currentPoint.X <= environment.Right &&
            nextPoint.X > environment.Right
        )
        {
            interact.Add("right");
        }
        else if (currentPoint.X >= environment.Left &&
            nextPoint.X < environment.Left
        )
        {
            interact.Add("left");
        }

        return string.Join(
            "_",
            interact
        );
    }

    private static bool IsEnv(
        EnvironmentItem item,
        Point point
    )
    {
        return point.X < item.Right
            && point.X > item.Left
            && point.Y > item.Top
            && point.Y < item.Bottom;
    }
}
