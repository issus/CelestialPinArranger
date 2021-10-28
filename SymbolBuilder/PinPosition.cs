using System;

namespace SymbolBuilder
{
    public enum PinSide
    {
        Left,
        Right,
        Top,
        Bottom
    }

    public enum PinAlignment
    {
        Left,
        Middle,
        Lower,
        Upper = 0,
        Center,
        Right
    }

    public readonly struct PinPosition : IEquatable<PinPosition>
    {
        public PinSide Side { get; }
        public PinAlignment Alignment { get; }

        public int ClockDirection => Side switch
        {
            PinSide.Left => Alignment switch
            {
                PinAlignment.Upper => 10,
                PinAlignment.Middle => 9,
                PinAlignment.Lower => 8,
                _ => throw new ArgumentOutOfRangeException()
            },
            PinSide.Right => Alignment switch
            {
                PinAlignment.Upper => 2,
                PinAlignment.Middle => 3,
                PinAlignment.Lower => 4,
                _ => throw new ArgumentOutOfRangeException()
            },
            PinSide.Top => Alignment switch
            {
                PinAlignment.Left => 11,
                PinAlignment.Center => 12,
                PinAlignment.Right => 1,
                _ => throw new ArgumentOutOfRangeException()
            },
            PinSide.Bottom => Alignment switch
            {
                PinAlignment.Left => 7,
                PinAlignment.Center => 6,
                PinAlignment.Right => 5,
                _ => throw new ArgumentOutOfRangeException()
            },
            _ => throw new ArgumentOutOfRangeException()
        };

        internal PinPosition(PinSide side, PinAlignment alignment)
        {
            Side = side;
            Alignment = alignment;
        }

        public static PinPosition From(PinSide side, PinAlignment alignment) =>
            new PinPosition(side, alignment);

        public static PinPosition From(int clockDirection) => (clockDirection % 12) switch
        {
            0 => new PinPosition(PinSide.Top, PinAlignment.Center),
            1 => new PinPosition(PinSide.Top, PinAlignment.Right),
            2 => new PinPosition(PinSide.Right, PinAlignment.Upper),
            3 => new PinPosition(PinSide.Right, PinAlignment.Middle),
            4 => new PinPosition(PinSide.Right, PinAlignment.Lower),
            5 => new PinPosition(PinSide.Bottom, PinAlignment.Right),
            6 => new PinPosition(PinSide.Bottom, PinAlignment.Center),
            7 => new PinPosition(PinSide.Bottom, PinAlignment.Left),
            8 => new PinPosition(PinSide.Left, PinAlignment.Lower),
            9 => new PinPosition(PinSide.Left, PinAlignment.Middle),
            10 => new PinPosition(PinSide.Left, PinAlignment.Upper),
            11 => new PinPosition(PinSide.Top, PinAlignment.Left),
            _ => throw new ArgumentOutOfRangeException()
        };

        public override string ToString() => $@"{Side}-{Alignment}";

        public bool Equals(PinPosition other) =>
            Side == other.Side && Alignment == other.Alignment;

        public override bool Equals(object obj) =>
            obj is PinPosition other && Equals(other);

        public override int GetHashCode() =>
            Side.GetHashCode() ^ Alignment.GetHashCode();

        public static bool operator ==(PinPosition left, PinPosition right) =>
            left.Equals(right);

        public static bool operator !=(PinPosition left, PinPosition right) =>
            !(left == right);
    }
}
