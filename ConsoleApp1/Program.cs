using ConsoleApp1;
using System;
using System.Collections.Generic;
using System.Linq;


public class Program
{
    public class SweepLineSegment : IComparable
    {
        public Point StartPoint;
        public Point EndPoint;
        public int id;

        public int CompareTo(object obj)
        {
            if (obj is SweepLineSegment lineSegment)
            {
                var ans = this.StartPoint.X.CompareTo(lineSegment.StartPoint.X);
                if (ans != 0)
                {
                    return ans;
                }
                else
                {
                    return this.id.CompareTo(lineSegment.id);
                }
            }
            else
            {
                throw new TypeLoadException("Неверный формат");
            }
        }
    }

    private class Event :IComparable
    {
        public double x;
        public int tp;
        public int id;

        public Event(double x, int tp, int id)
        {
            this.x = x;
            this.tp = tp;
            this.id = id;
        }

        public int CompareTo(object obj)
        {
            if (obj is Event e)
                if (x.CompareTo(e.x) != 0)
                    return x.CompareTo(e.x);
                else
                    return id.CompareTo(e.id);
            else
                throw new TypeLoadException("Неверный формат");
        }

        public static bool operator <(Event e1, Event e2)
        {
            if (e1.x - e2.x != 0)
                return e1.x < e2.x;
            return e1.tp > e2.tp;
        }

        public static bool operator >(Event e1, Event e2)
        {
            return !(e1.tp < e2.tp);
        }
    }
    private static (long, long) GetMinMax(long _a, long _b)
    {
        if (_a < _b)
        {
            return (_a, _b);
        }
        else
        {
            return (_b, _a);
        }
    }

    public static double FindXIntersection(SweepLineSegment line1, SweepLineSegment line2)
    {
        double x = (
            (line1.StartPoint.X * line1.EndPoint.Y - line1.StartPoint.Y * line1.EndPoint.X) * 
            (line2.StartPoint.X - line2.EndPoint.X) - (line1.StartPoint.X - line1.EndPoint.X) * 
            (line2.StartPoint.X * line2.EndPoint.Y - line2.StartPoint.Y * line2.EndPoint.X)
            ) / (
            (line1.StartPoint.X - line1.EndPoint.X) * (line2.StartPoint.Y - line2.EndPoint.Y) - 
            (line1.StartPoint.Y - line1.EndPoint.Y) * (line2.StartPoint.X - line2.EndPoint.X)
            );

        return x;
    }

    private static bool AreSegmentsIntersecting(SweepLineSegment line1, SweepLineSegment line2)
    {
        {
            var (minX1, maxX1) = GetMinMax(line1.StartPoint.X, line1.EndPoint.X);
            var (minX2, maxX2) = GetMinMax(line2.StartPoint.X, line2.EndPoint.X);
            if (maxX1 < minX2 || minX1 > maxX2)
            {
                return false;
            }
        }
        {
            var (minY1, maxY1) = GetMinMax(line1.StartPoint.Y, line1.EndPoint.Y);
            var (minY2, maxY2) = GetMinMax(line2.StartPoint.Y, line2.EndPoint.Y);
            if (maxY1 < minY2 || minY1 > maxY2)
            {
                return false;
            }
        }

        var delta1 = new QuickPoint(line1.StartPoint, line1.EndPoint);
        var delta2 = new QuickPoint(line2.StartPoint, line2.EndPoint);
        var deltaStart = new QuickPoint(line2.StartPoint, line1.StartPoint);

        var det = QuickPoint.CrossProduct(delta1, delta2);
        var det1 = QuickPoint.CrossProduct(delta1, deltaStart);
        var det2 = QuickPoint.CrossProduct(delta2, deltaStart);
        if (det == 0)
        {
            return det1 == 0 && det2 == 0;
        }

        if (det > 0)
        {
            return 0 <= det1 && det1 <= det
                && 0 <= det2 && det2 <= det;
        }
        else
        {
            return det <= det1 && det1 <= 0
                && det <= det2 && det2 <= 0;
        }
    }

    public static void Main()
    {
        var Points = new List<Point>();
        Points.Add(new Point() { X = 1, Y = 0 });
        Points.Add(new Point() { X = 3, Y = 5 });
        Points.Add(new Point() { X = 5, Y = 0 });
        Points.Add(new Point() { X = 0, Y = 3 });
        Points.Add(new Point() { X = 6, Y = 3 });
        var l_pointsWithoutDuplicates = new List<Point>();
        var l_borderLines = new List<SweepLineSegment>();
        for (int i = 1; i < Points.Count; i++)
        {
            if (Points[i - 1].X == Points[i].X && Points[i - 1].Y == Points[i].Y)
            {
                continue;
            }
            l_borderLines.Add(new SweepLineSegment() { StartPoint = Points[i - 1], EndPoint = Points[i], id = i - 1 });
            l_pointsWithoutDuplicates.Add(Points[i - 1]);
        }
        var l_pointBegin = Points[Points.Count - 1];
        var l_pointEnd = Points[0];
        l_borderLines.Add(new SweepLineSegment() { StartPoint = l_pointBegin, EndPoint = l_pointEnd, id = Points.Count - 1 });

        List<Event> events = new List<Event>();
        for (int i = 0; i < l_borderLines.Count; i++)
        {
            events.Add(new Event(Math.Min(l_borderLines[i].StartPoint.X, l_borderLines[i].EndPoint.X), +1, i));
            events.Add(new Event(Math.Max(l_borderLines[i].StartPoint.X, l_borderLines[i].EndPoint.X), -1, i));
        }
        events.Sort();
        var sweep = new RedBlackTree<SweepLineSegment>();
        foreach (var e in events)
        {
            int id = e.id;
            if (e.tp == +1)
            {
                var (up, down) = sweep.InsertAndGetNeighbours(l_borderLines[id]);
                var prev = down.FirstOrDefault();
                var next = up.FirstOrDefault();
                if (next != null && AreSegmentsIntersecting(next, l_borderLines[id]))
                {
                    var dif1 = Math.Abs(next.id - l_borderLines[id].id);
                    if (!(dif1 == 1 || dif1 == l_borderLines.Count - 1))
                    {
                        Console.WriteLine(next.id + ", " + l_borderLines[id].id);
                        events.Add(new Event(FindXIntersection(next, l_borderLines[id]), 0, 0));
                    }
                }
                if (prev != null && AreSegmentsIntersecting(prev, l_borderLines[id]))
                {
                    var dif2 = Math.Abs(l_borderLines[id].id - prev.id);
                    if (!(dif2 == 1 || dif2 == l_borderLines.Count - 1))
                    {
                        Console.WriteLine(prev.id + ", " + l_borderLines[id].id);
                        events.Add(new Event(FindXIntersection(prev, l_borderLines[id]), 0, 0));
                    }
                }
            }
            else if (e.tp == -1)
            {
                var (up, down) = sweep.FindAndGetNeighbours(l_borderLines[id]);
                var prev = down.FirstOrDefault();
                var next = up.FirstOrDefault();
                if (next != null && prev != null && AreSegmentsIntersecting(next, prev))
                {
                    var dif = Math.Abs(next.id - prev.id);
                    if (!(dif == 1 || dif == l_borderLines.Count - 1))
                        Console.WriteLine(next.id + ", " + prev.id);
                }
                sweep.Delete(l_borderLines[id]);
            }
        }
        Console.ReadLine();
    }
}


