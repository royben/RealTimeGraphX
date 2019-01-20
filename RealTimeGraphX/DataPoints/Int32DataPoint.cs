using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeGraphX.DataPoints
{
    /// <summary>
    /// Represents a graph <see cref="Int32"/> value data point.
    /// </summary>
    /// <seealso cref="RealTimeGraphX.GraphDataPoint{System.Int32, RealTimeGraphX.DataPoints.Int32DataPoint}" />
    public class Int32DataPoint : GraphDataPoint<Int32, Int32DataPoint>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Int32DataPoint"/> class.
        /// </summary>
        public Int32DataPoint() : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Int32DataPoint"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public Int32DataPoint(int value) : base(value)
        {

        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Int32"/> to <see cref="Int32DataPoint"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Int32DataPoint(int value)
        {
            return new Int32DataPoint(value);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Int32DataPoint operator -(Int32DataPoint a, Int32DataPoint b)
        {
            return new Int32DataPoint(a.Value - b.Value);
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Int32DataPoint operator +(Int32DataPoint a, Int32DataPoint b)
        {
            return new Int32DataPoint(a.Value + b.Value);
        }

        /// <summary>
        /// Sums the value of this instance with another instance value and returns the result.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns></returns>
        public override IGraphDataPoint Add(IGraphDataPoint other)
        {
            return new Int32DataPoint(this.Value + (other as Int32DataPoint).Value);
        }

        /// <summary>
        /// Subtract the value of another instance from this instance and returns the result.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns></returns>
        public override IGraphDataPoint Subtract(IGraphDataPoint other)
        {
            return new Int32DataPoint(this.Value - (other as Int32DataPoint).Value);
        }

        /// <summary>
        /// Multiplies the value of this instance with another instance value and returns the result.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns></returns>
        public override IGraphDataPoint Multiply(IGraphDataPoint other)
        {
            return new Int32DataPoint(this.Value * (other as Int32DataPoint).Value);
        }

        /// <summary>
        /// Divides the value of this instance with another instance value and returns the result.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns></returns>
        public override IGraphDataPoint Divide(IGraphDataPoint other)
        {
            return new Int32DataPoint(this.Value / (other as Int32DataPoint).Value);
        }

        /// <summary>
        /// Returns the percentage value of this instance between the specified minimum and maximum values.
        /// </summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns></returns>
        public override double ComputeRelativePosition(IGraphDataPoint min, IGraphDataPoint max)
        {
            Int32DataPoint dMin = min as Int32DataPoint;
            Int32DataPoint dMax = max as Int32DataPoint;

            var result = ((Value - dMin) * 100) / (dMax - dMin);

            return result;
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
            int minimum = (int)min.GetValue();
            int maximum = (int)max.GetValue();

            return new Int32DataPoint((int)(minimum + (maximum - minimum) * percentage));
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
            int minimum = (int)min.GetValue();
            int maximum = (int)max.GetValue();

            return Enumerable.Range(0, count).
                Select(i => minimum + (maximum - minimum) * ((int)i / (count - 1))).
                Select(x => new Int32DataPoint(x));
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
            return new Int32DataPoint(int.Parse(value));
        }
    }
}
