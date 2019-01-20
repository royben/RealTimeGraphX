using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeGraphX.DataPoints
{
    /// <summary>
    /// Represents a graph <see cref="TimeSpan"/> value data point.
    /// </summary>
    /// <seealso cref="RealTimeGraphX.GraphDataPoint{System.TimeSpan, RealTimeGraphX.DataPoints.TimeSpanDataPoint}" />
    public class TimeSpanDataPoint : GraphDataPoint<TimeSpan, TimeSpanDataPoint>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeSpanDataPoint"/> class.
        /// </summary>
        public TimeSpanDataPoint() : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeSpanDataPoint"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public TimeSpanDataPoint(TimeSpan value) : base(value)
        {

        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.TimeSpan"/> to <see cref="TimeSpanDataPoint"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator TimeSpanDataPoint(TimeSpan value)
        {
            return new TimeSpanDataPoint(value);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static TimeSpanDataPoint operator -(TimeSpanDataPoint a, TimeSpanDataPoint b)
        {
            return new TimeSpanDataPoint(a.Value - b.Value);
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static TimeSpanDataPoint operator +(TimeSpanDataPoint a, TimeSpanDataPoint b)
        {
            return new TimeSpanDataPoint(a.Value + b.Value);
        }

        /// <summary>
        /// Sums the value of this instance with another instance value and returns the result.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns></returns>
        public override IGraphDataPoint Add(IGraphDataPoint other)
        {
            return new TimeSpanDataPoint(this.Value + (other as TimeSpanDataPoint).Value);
        }

        /// <summary>
        /// Subtract the value of another instance from this instance and returns the result.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns></returns>
        public override IGraphDataPoint Subtract(IGraphDataPoint other)
        {
            return new TimeSpanDataPoint(this.Value - (other as TimeSpanDataPoint).Value);
        }

        /// <summary>
        /// Multiplies the value of this instance with another instance value and returns the result.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns></returns>
        public override IGraphDataPoint Multiply(IGraphDataPoint other)
        {
            return new TimeSpanDataPoint(TimeSpan.FromMilliseconds(this.Value.TotalMilliseconds * (other as TimeSpanDataPoint).Value.TotalMilliseconds));
        }

        /// <summary>
        /// Divides the value of this instance with another instance value and returns the result.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns></returns>
        public override IGraphDataPoint Divide(IGraphDataPoint other)
        {
            return new TimeSpanDataPoint(TimeSpan.FromMilliseconds(this.Value.TotalMilliseconds / (other as TimeSpanDataPoint).Value.TotalMilliseconds));
        }

        /// <summary>
        /// Returns the percentage value of this instance between the specified minimum and maximum values.
        /// </summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns></returns>
        public override double ComputeRelativePosition(IGraphDataPoint min, IGraphDataPoint max)
        {
            TimeSpan dMin = min as TimeSpanDataPoint;
            TimeSpan dMax = max as TimeSpanDataPoint;

            var result = ((Value.TotalMilliseconds - dMin.TotalMilliseconds) * 100) / (dMax.TotalMilliseconds - dMin.TotalMilliseconds);

            return double.IsNaN(result) ? dMin.TotalMilliseconds : result;
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
            double minimum = ((TimeSpan)min.GetValue()).TotalMilliseconds;
            double maximum = ((TimeSpan)max.GetValue()).TotalMilliseconds;

            return new TimeSpanDataPoint(TimeSpan.FromMilliseconds(minimum + (maximum - minimum) * percentage));
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
            double minimum = ((TimeSpan)min.GetValue()).TotalMilliseconds;
            double maximum = ((TimeSpan)max.GetValue()).TotalMilliseconds;

            return Enumerable.Range(0, count).
                Select(i => minimum + (maximum - minimum) * ((double)i / (count - 1))).
                Select(x => new TimeSpanDataPoint(TimeSpan.FromMilliseconds(x)));
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
            return new TimeSpanDataPoint(TimeSpan.Parse(value));
        }
    }
}
