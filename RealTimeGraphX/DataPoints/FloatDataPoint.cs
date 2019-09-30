using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeGraphX.DataPoints
{
    /// <summary>
    /// Represents a graph <see cref="float"/> value data point.
    /// </summary>
    /// <seealso cref="RealTimeGraphX.GraphDataPoint{float, RealTimeGraphX.DataPoints.FloatDataPoint}" />
    public class FloatDataPoint : GraphDataPoint<float, FloatDataPoint>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FloatDataPoint"/> class.
        /// </summary>
        public FloatDataPoint() : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FloatDataPoint"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public FloatDataPoint(float value) : base(value)
        {

        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="float"/> to <see cref="FloatDataPoint"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator FloatDataPoint(float value)
        {
            return new FloatDataPoint(value);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static FloatDataPoint operator -(FloatDataPoint a, FloatDataPoint b)
        {
            return new FloatDataPoint(a.Value - b.Value);
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static FloatDataPoint operator +(FloatDataPoint a, FloatDataPoint b)
        {
            return new FloatDataPoint(a.Value + b.Value);
        }

        /// <summary>
        /// Sums the value of this instance with another instance value and returns the result.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns></returns>
        public override IGraphDataPoint Add(IGraphDataPoint other)
        {
            return new FloatDataPoint(this.Value + (other as FloatDataPoint).Value);
        }

        /// <summary>
        /// Subtract the value of another instance from this instance and returns the result.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns></returns>
        public override IGraphDataPoint Subtract(IGraphDataPoint other)
        {
            return new FloatDataPoint(this.Value - (other as FloatDataPoint).Value);
        }

        /// <summary>
        /// Multiplies the value of this instance with another instance value and returns the result.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns></returns>
        public override IGraphDataPoint Multiply(IGraphDataPoint other)
        {
            return new FloatDataPoint(this.Value * (other as FloatDataPoint).Value);
        }

        /// <summary>
        /// Divides the value of this instance with another instance value and returns the result.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns></returns>
        public override IGraphDataPoint Divide(IGraphDataPoint other)
        {
            return new FloatDataPoint(this.Value / (other as FloatDataPoint).Value);
        }

        /// <summary>
        /// Returns the percentage value of this instance between the specified minimum and maximum values.
        /// </summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns></returns>
        public override double ComputeRelativePosition(IGraphDataPoint min, IGraphDataPoint max)
        {
            FloatDataPoint dMin = min as FloatDataPoint;
            FloatDataPoint dMax = max as FloatDataPoint;

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

            return new FloatDataPoint((float)(minimum + (maximum - minimum) * percentage));
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
                Select(x => new FloatDataPoint((float)x));
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
            return new FloatDataPoint(float.Parse(value));
        }
    }
}
