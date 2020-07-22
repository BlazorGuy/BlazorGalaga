using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorGalaga.Models
{
    public class EnemyGridPoint
    {
        public EnemyGridPoint(float x, float y, int row, int col)
        {
            Point = new PointF(x,y);
            Row = row;
            Column = col;
        }

        public PointF Point { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
    }

    public class EnemyGrid
    {
        public List<EnemyGridPoint> GridPoints { get; set; }

        public PointF GetPointByRowCol(int row, int col)
        {
            return GridPoints.FirstOrDefault(a => a.Row == row && a.Column == col).Point;
        }

        public int GridLeft { get; set; }

        public EnemyGrid()
        {
            GridLeft = 270;
            
            const int GridTop = 150;
            const int HSpacing = 45;
            const int VSpacing = 45;
            const int XOffset = 0;
            const int YOffset = 0;

            GridPoints = new List<EnemyGridPoint>();

            GridPoints.Add(new EnemyGridPoint(GridLeft + (HSpacing * 0) + XOffset, GridTop + YOffset, 1, 1));
            GridPoints.Add(new EnemyGridPoint(GridLeft + (HSpacing * 1) + XOffset, GridTop + YOffset, 1, 2));
            GridPoints.Add(new EnemyGridPoint(GridLeft + (HSpacing * 2) + XOffset, GridTop + YOffset, 1, 3));
            GridPoints.Add(new EnemyGridPoint(GridLeft + (HSpacing * 3) + XOffset, GridTop + YOffset, 1, 4));

            GridPoints.Add(new EnemyGridPoint(GridLeft - (HSpacing * 2) + XOffset, GridTop + (VSpacing * 1) + YOffset, 2, 1));
            GridPoints.Add(new EnemyGridPoint(GridLeft - (HSpacing * 1) + XOffset, GridTop + (VSpacing * 1) + YOffset, 2, 2));
            GridPoints.Add(new EnemyGridPoint(GridLeft - (HSpacing * 0) + XOffset, GridTop + (VSpacing * 1) + YOffset, 2, 3));
            GridPoints.Add(new EnemyGridPoint(GridLeft + (HSpacing * 1) + XOffset, GridTop + (VSpacing * 1) + YOffset, 2, 4));
            GridPoints.Add(new EnemyGridPoint(GridLeft + (HSpacing * 2) + XOffset, GridTop + (VSpacing * 1) + YOffset, 2, 5));
            GridPoints.Add(new EnemyGridPoint(GridLeft + (HSpacing * 3) + XOffset, GridTop + (VSpacing * 1) + YOffset, 2, 6));
            GridPoints.Add(new EnemyGridPoint(GridLeft + (HSpacing * 4) + XOffset, GridTop + (VSpacing * 1) + YOffset, 2, 7));
            GridPoints.Add(new EnemyGridPoint(GridLeft + (HSpacing * 5) + XOffset, GridTop + (VSpacing * 1) + YOffset, 2, 8));
            
            GridPoints.Add(new EnemyGridPoint(GridLeft - (HSpacing * 2) + XOffset, GridTop + (VSpacing * 2) + YOffset, 3, 1));
            GridPoints.Add(new EnemyGridPoint(GridLeft - (HSpacing * 1) + XOffset, GridTop + (VSpacing * 2) + YOffset, 3, 2));
            GridPoints.Add(new EnemyGridPoint(GridLeft - (HSpacing * 0) + XOffset, GridTop + (VSpacing * 2) + YOffset, 3, 3));
            GridPoints.Add(new EnemyGridPoint(GridLeft + (HSpacing * 1) + XOffset, GridTop + (VSpacing * 2) + YOffset, 3, 4));
            GridPoints.Add(new EnemyGridPoint(GridLeft + (HSpacing * 2) + XOffset, GridTop + (VSpacing * 2) + YOffset, 3, 5));
            GridPoints.Add(new EnemyGridPoint(GridLeft + (HSpacing * 3) + XOffset, GridTop + (VSpacing * 2) + YOffset, 3, 6));
            GridPoints.Add(new EnemyGridPoint(GridLeft + (HSpacing * 4) + XOffset, GridTop + (VSpacing * 2) + YOffset, 3, 7));
            GridPoints.Add(new EnemyGridPoint(GridLeft + (HSpacing * 5) + XOffset, GridTop + (VSpacing * 2) + YOffset, 3, 8));

            GridPoints.Add(new EnemyGridPoint(GridLeft - (HSpacing * 3) + XOffset, GridTop + (VSpacing * 3) + YOffset, 4, 1));
            GridPoints.Add(new EnemyGridPoint(GridLeft - (HSpacing * 2) + XOffset, GridTop + (VSpacing * 3) + YOffset, 4, 2));
            GridPoints.Add(new EnemyGridPoint(GridLeft - (HSpacing * 1) + XOffset, GridTop + (VSpacing * 3) + YOffset, 4, 3));
            GridPoints.Add(new EnemyGridPoint(GridLeft - (HSpacing * 0) + XOffset, GridTop + (VSpacing * 3) + YOffset, 4, 4));
            GridPoints.Add(new EnemyGridPoint(GridLeft + (HSpacing * 1) + XOffset, GridTop + (VSpacing * 3) + YOffset, 4, 5));
            GridPoints.Add(new EnemyGridPoint(GridLeft + (HSpacing * 2) + XOffset, GridTop + (VSpacing * 3) + YOffset, 4, 6));
            GridPoints.Add(new EnemyGridPoint(GridLeft + (HSpacing * 3) + XOffset, GridTop + (VSpacing * 3) + YOffset, 4, 7));
            GridPoints.Add(new EnemyGridPoint(GridLeft + (HSpacing * 4) + XOffset, GridTop + (VSpacing * 3) + YOffset, 4, 8));
            GridPoints.Add(new EnemyGridPoint(GridLeft + (HSpacing * 5) + XOffset, GridTop + (VSpacing * 3) + YOffset, 4, 9));
            GridPoints.Add(new EnemyGridPoint(GridLeft + (HSpacing * 6) + XOffset, GridTop + (VSpacing * 3) + YOffset, 4, 10));

            GridPoints.Add(new EnemyGridPoint(GridLeft - (HSpacing * 3) + XOffset, GridTop + (VSpacing * 4) + YOffset, 5, 1));
            GridPoints.Add(new EnemyGridPoint(GridLeft - (HSpacing * 2) + XOffset, GridTop + (VSpacing * 4) + YOffset, 5, 2));
            GridPoints.Add(new EnemyGridPoint(GridLeft - (HSpacing * 1) + XOffset, GridTop + (VSpacing * 4) + YOffset, 5, 3));
            GridPoints.Add(new EnemyGridPoint(GridLeft - (HSpacing * 0) + XOffset, GridTop + (VSpacing * 4) + YOffset, 5, 4));
            GridPoints.Add(new EnemyGridPoint(GridLeft + (HSpacing * 1) + XOffset, GridTop + (VSpacing * 4) + YOffset, 5, 5));
            GridPoints.Add(new EnemyGridPoint(GridLeft + (HSpacing * 2) + XOffset, GridTop + (VSpacing * 4) + YOffset, 5, 6));
            GridPoints.Add(new EnemyGridPoint(GridLeft + (HSpacing * 3) + XOffset, GridTop + (VSpacing * 4) + YOffset, 5, 7));
            GridPoints.Add(new EnemyGridPoint(GridLeft + (HSpacing * 4) + XOffset, GridTop + (VSpacing * 4) + YOffset, 5, 8));
            GridPoints.Add(new EnemyGridPoint(GridLeft + (HSpacing * 5) + XOffset, GridTop + (VSpacing * 4) + YOffset, 5, 9));
            GridPoints.Add(new EnemyGridPoint(GridLeft + (HSpacing * 6) + XOffset, GridTop + (VSpacing * 4) + YOffset, 5, 10));
        }
    }
}
