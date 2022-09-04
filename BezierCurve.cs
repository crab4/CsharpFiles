using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;


/* 
 * Так как решение пришло на ум пока читал книгу про Дарта Бейна, то и нельзя не упомянуть слова Ревана из голокрона
 * Только двое их, мастер и ученик. Не больше. Один воплощает могущество, второй стремится к нему.
 * Да, тут всё ещё очень сумбурно и некрасиво, но код постепенно становится всё опрятнее, понятнее и функциональней что ли, не знаю, другого слова.  Здесь ты снова начал использовать перегрузки
 * стандартных методов и операторов. Может быть постепенно вспомнишь всё, что использовал. Задачу можно было решить гораздо короче. Но я увлёкся, подписал классы, подписал к ним интересующие меня сегодня методы и функции. 
 * Можно половину убрать, но это хотелось написать и в целом, оно может быть полезно
 */
namespace BezierCurve {
    class Program {
        static void Main(string[] args) {
            string[] tmp = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            int count = Int32.Parse(tmp[0]);
            double stepOfDelta = 1.0 / (Int32.Parse(tmp[1]) - 1);
            int countPoint = Int32.Parse(tmp[1]);
            double currentDelta = 0.0;
            string answer = string.Empty;
            List<Point> points = new List<Point>();
            while (count-- > 0) {
                tmp = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                points.Add(new Point(Int32.Parse(tmp[0]), Int32.Parse(tmp[1])));
            }
            List<Vector> newVectors = new List<Vector>();
            for (var i = 0; i < points.Count - 1; i++)
                newVectors.Add(new Vector(points[i], points[i + 1]));
            // пробовал с CurrentDelta делать for, но последний не считает, видимо, немного выходит за радиус единички?
            for (int i = 0; i < countPoint; i++) {
                answer += $"{Vector.BezierPoint(newVectors, currentDelta)} ";
                currentDelta += stepOfDelta;
            }
            Console.WriteLine(answer);
        }
    }

    //Хотел использовать прошлый шаблонный метод. Но решил взять без шаблона. Писать меньше
    class Point : ICloneable { // класс точки. Содержит координаты, которые не меняются и символ. 
        double m_x;
        double m_y;
        public Point(double x = 0, double y = 0) { m_x = x; m_y = y; }

        public double GetX { set { m_x = value; } get { return m_x; } }
        public double GetY { set { m_y = value; } get { return m_y; } }
        public static double DistanceBetween(Point a, Point b) => Math.Sqrt(Math.Pow(a.m_x - b.m_x, 2) + Math.Pow(a.m_y - b.m_y, 2));// метод расчёта расстояния между двумя точками
        public override string ToString() {
            return $"{Math.Round(this.GetX,MidpointRounding.AwayFromZero)} {Math.Round(this.GetY,MidpointRounding.AwayFromZero)}";
       
        }
        public object Clone() {
            return new Point(this.GetX, this.GetY);
        }

    }
    class Vector {
        //Логично, что для вектора хватает двух координат или координаты и координаты направления(координаты вектора, называй как хочешь)
        // Но чтобы каждый раз не расчитывать эту штуку, пусть будen все 3
        Point m_base;
        Point m_final;
        Point m_direction;
        public Point Base { get { return m_base; } private set { m_base = value; } }
        public Point Final { get { return m_final; } private set { m_final = value; } }
        public Point Direction { get { return m_direction; } private set { m_direction = value; } }
        public Vector (Point zero, Point final) {
            m_base = (Point)zero.Clone();
            m_final = (Point)final.Clone();
            m_direction = new Point(final.GetX - zero.GetX, final.GetY - zero.GetY);
        }
        //чтобы как - то отличать два конструктора
        public Vector(Point zero, Point direction, bool fear) {
            m_base = (Point)zero.Clone();
            m_final = new Point(zero.GetX + direction.GetX, zero.GetY + direction.GetY);
            m_direction = (Point)direction.Clone();

        }
        public override string ToString() {
            return $"Base = {this.Base}, End = {this.Final}, Vector = {this.Direction}";
        }
        // Возвращает вектор, получившийся от умножения на число. точка начала остаётся на месте, меняется лишь длина и, возможно, направление.
        public Vector MulOnNumber(double delta) =>new Vector((Point)Base.Clone(), new Point(Direction.GetX*delta, Direction.GetY*delta), true);
        // Возвращает вектор, являющийся разницей базового вектор и вектора от MulOnNumber, при этом базовая точка сместится на длину MulOnNumber, а вот финальная точка останется на месте.
        public Vector MulOnNumberSecondPart(double delta) => new Vector(new Point(Base.GetX+Direction.GetX*delta,Base.GetY+Direction.GetY*delta), (Point)Final.Clone());
        // сумма векторов, базовая точка от первого вектора. А последняя точка принадлежит сумме их направлений. В конкретной задаче можно было просто указать точку конца второго вектора. Но не всегда же векторы будут один возле другого (=
        public Vector SumOFVectors(Vector s2) =>new Vector((Point)this.Base.Clone(), new Point(this.Direction.GetX + s2.Direction.GetX, this.Direction.GetY + s2.Direction.GetY), true);
        

        //И вот она, после долгой подготовке, поиске информации о перегрузке методов, воспоминаниях о c++ родилась таки функция для поиска точки на Кривой Безье)
        public static Point BezierPoint(List<Vector> lines, double delta) {
            if (lines.Count < 1)
                throw new Exception("Он убил юнлингов");
            if (lines.Count == 1)
                return lines[0].MulOnNumber(delta).Final;
            List<Point> points = new List<Point>();
            foreach(var line in lines) {
                points.Add(line.MulOnNumber(delta).Final);
            }
            List<Vector> newVectors = new List<Vector>();
            for (var i = 0; i < points.Count - 1; i++)
                newVectors.Add(new Vector(points[i], points[i + 1]));
            return BezierPoint(newVectors, delta);
        }

    }

}
