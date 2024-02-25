using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Point
    {
        public long X;
        public long Y;
    }
    public struct QuickPoint
    {
        public long X
        {
            get;
            private set;
        }

        public long Y
        {
            get;
            private set;
        }

        public QuickPoint(long _x, long _y)
        {
            this.X = _x;
            this.Y = _y;
        }

        public QuickPoint(double _x, double _y)
        {
            this.X = (long)_x;
            this.Y = (long)_y;
        }

        public QuickPoint(Point _start, Point _end)
        {
            this.X = (long)(_end.X - _start.X);
            this.Y = (long)(_end.Y - _start.Y);
        }

        public long SquaredLength()
        {
            return DotProduct(this, this);
        }

        public static QuickPoint operator +(QuickPoint _a, QuickPoint _b)
        {
            return new QuickPoint(_a.X + _b.X, _a.Y + _b.Y);
        }

        public static QuickPoint operator -(QuickPoint _a, QuickPoint _b)
        {
            return new QuickPoint(_a.X - _b.X, _a.Y - _b.Y);
        }

        public static QuickPoint operator -(QuickPoint _a)
        {
            return new QuickPoint(-_a.X, -_a.Y);
        }

        public static QuickPoint operator *(QuickPoint _a, long _k)
        {
            return new QuickPoint(_a.X * _k, _a.Y * _k);
        }

        public static QuickPoint operator /(QuickPoint _a, long _k)
        {
            return new QuickPoint(_a.X / _k, _a.Y / _k);
        }

        public static long DotProduct(QuickPoint _a, QuickPoint _b)
        {
            return _a.X * _b.X + _a.Y * _b.Y;
        }

        public static long CrossProduct(QuickPoint _a, QuickPoint _b)
        {
            return _a.X * _b.Y - _a.Y * _b.X;
        }

        public static explicit operator QuickPoint(Point _p)
        {
            return new QuickPoint(_p.X, _p.Y);
        }

        public static implicit operator Point(QuickPoint _p)
        {
            return new Point() { X = _p.X, Y = _p.Y };
        }

        public static bool operator !=(QuickPoint a, QuickPoint b)
        {
            return !(a == b);
        }

        public static bool operator ==(QuickPoint a, QuickPoint b)
        {
            return Eq(a, b);
        }

        private static bool Eq(QuickPoint a, QuickPoint b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public override bool Equals(object _obj)
        {
            if (_obj is QuickPoint p)
            {
                return Eq(this, p);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (X, Y).GetHashCode();
        }

        public override string ToString()
        {
            return $"X: {X}; Y: {Y}";
        }
    }
}
