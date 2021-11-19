namespace EventHorizon.Blazor.Mascot;

public struct Point
{
    public int X { get; set; }
    public int Y { get; set; }

    public Point(
        int x,
        int y
    )
    {
        X = x;
        Y = y;
    }

    public Point(
        Point position
    ) : this()
    {
        X = position.X;
        Y = position.Y;
    }

    public Point Inverse()
    {
        return new Point(
            X * -1,
            Y * -1
        );
    }

    public override string ToString()
    {
        return $"Point({X}, {Y})";
    }
}
