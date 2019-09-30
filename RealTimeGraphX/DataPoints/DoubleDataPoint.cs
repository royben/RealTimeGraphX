using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeGraphX.DataPoints
{
    /// <summary>
    /// Represents a graph <see cref="Double"/> value data point.
    /// </summary>
    /// <seealso cref="RealTimeGraphX.GraphDataPoint{System.Double, RealTimeGraphX.DataPoints.DoubleDataPoint}" />
    public class DoubleDataPoint : GraphDataPoint<Double, DoubleDataPoint>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleDataPoint"/> class.
        /// </summary>
        public DoubleDataPoint() : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleDataPoint"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public DoubleDataPoint(double value) : base(value)
        {

        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Double"/> to <see cref="DoubleDataPoint"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator DoubleDataPoint(double value)
        {
            return new DoubleDataPoint(value);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static DoubleDataPoint operator -(DoubleDataPoint a, DoubleDataPoint b)
        {
            return new DoubleDataPoint(a.Value - b.Value);
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static DoubleDataPoint operator +(DoubleDataPoint a, DoubleDataPoint b)
        {
            return new DoubleDataPoint(a.Value + b.Value);
        }

        /// <summary>
        /// Sums the value of this instance with another instance value and returns the result.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns></returns>
        public override IGraphDataPoint Add(IGraphDataPoint other)
        {
            return new DoubleDataPoint(this.Value + (other as DoubleDataPoint).Value);
        }

        /// <summary>
        /// Subtract the value of another instance from this instance and returns the result.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns></returns>
        public override IGraphDataPoint Subtract(IGraphDataPoint other)
        {
            return new DoubleDataPoint(this.Value - (other as DoubleDataPoint).Value);
        }

        /// <summary>
        /// Multiplies the value of this instance with another instance value and returns the result.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns></returns>
        public override IGraphDataPoint Multiply(IGraphDataPoint other)
        {
            return new DoubleDataPoint(this.Value * (other as DoubleDataPoint).Value);
        }

        /// <summary>
        /// Divides the value of this instance with another instance value and returns the result.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns></returns>
        public override IGraphDataPoint Divide(IGraphDataPoint other)
        {
            return new DoubleDataPoint(this.Value / (other as DoubleDataPoint).Value);
        }

        /// <summary>
        /// Returns the percentage value of this instance between the specified minimum and maximum values.
        /// </summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns></returns>
        public override double ComputeRelativePosition(IGraphDataPoint min, IGraphDataPoint max)
        {
            DoubleDataPoint dMin = min as DoubleDataPoint;
            DoubleDataPoint dMax = max as DoubleDataPoint;

            var result = ((Value - dMin) * 100) / (dMax - dMin);

            return double.IsNaN(result) || double.IsInfinity(result) ? dMin.Value : result;
        }

        /// <summary>
        /// Returns the absolute value of the specified percentage value between the specified minimum and maximum values.
        /// </summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <param name="percentage">The percentage.</param>
        /// <returns></returns>
        public override IGraphDataPoint ComputeAbsolutePosition(IGraphDataPoint min, IGraphDataPoint max, double percentage)
        {
            double minimum = (double)min.GetValue();
            double maximum = (double)max.GetValue();

            return new DoubleDataPoint(minimum + (maximum - minimum) * percentage);
        }

        /// <summary>
        /// Creates a range of values from the specified minimum and maximum.
        /// </summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public override IEnumerable<IGraphDataPoint> CreateRange(IGraphDataPoint min, IGraphDataPoint max, int count)
        {
            double minimum = (double)min.GetValue();
            double maximum = (double)max.GetValue();

            return Enumerable.Range(0, count).
                Select(i => minimum + (maximum - minimum) * ((double)i / (count - 1))).
                Select(x => new DoubleDataPoint(x));
        }

        /// <summary>
        /// Returns a formated string of this data point.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public override string ToString(string format)
        {
            return Value.ToString(format);
        }

        /// <summary>
        /// Parses the specified value and returns a new instance of <see cref="!:TDataType" /> data point.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public override IGraphDataPoint Parse(string value)
        {
            return new DoubleDataPoint(double.Parse(value));
        }
    }
}
