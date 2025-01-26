using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FungiSimulation
{
    public class Branch
    {
        public Point _startPoint;
        public Point _endPoint;
        public bool _isActive;
        public double _currentAngle;
        public double _length;

        public double technicalEndpointXCoordinates;
        public double technicalEndpointYCoordinates;

        public Branch(Point startPoint, Point endPoint,bool isActive, double currentAngle)
        {
            _startPoint = startPoint;
            _endPoint = endPoint;
            _isActive = isActive;
            _currentAngle = currentAngle;
            _length = Math.Sqrt( Math.Pow(_startPoint.X - _endPoint.X,2) + Math.Pow(_startPoint.Y - _endPoint.Y,2) );
            technicalEndpointXCoordinates = (double)_endPoint.X + 0.5;
            technicalEndpointYCoordinates = (double)_endPoint.Y + 0.5;
        }

    }
}
