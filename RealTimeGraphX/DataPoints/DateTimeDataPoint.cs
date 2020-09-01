using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RealTimeGraphX.DataPoints
{
    public class DateTimeDataPoint : GraphDataPoint<DateTime, DateTimeDataPoint>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeDataPoint"/> class.
        /// </summary>
        public DateTimeDataPoint() : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeDataPoint"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public DateTimeDataPoint(DateTime value) : base(value)
        {

        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.TimeSpan"/> to <see cref="DateTimeDataPoint"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator DateTimeDataPoint(DateTime value)
        {
            return new DateTimeDataPoint(value);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static DateTimeDataPoint operator -(DateTimeDataPoint a, DateTimeDataPoint b)
        {
            return new DateTimeDataPoint(new DateTime(a.Value.Ticks - b.Value.Ticks));
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static DateTimeDataPoint operator +(DateTimeDataPoint a, DateTimeDataPoint b)
        {
            return new DateTimeDataPoint(new DateTime(a.Value.Ticks + b.Value.Ticks));
        }

        /// <summary>
        /// Sums the value of this instance with another instance value and returns the result.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns></returns>
        public override IGraphDataPoint Add(IGraphDataPoint other)
        {
            return new DateTimeDataPoint(new DateTime(this.Value.Ticks + (other as DateTimeDataPoint).Value.Ticks));
        }

        /// <summary>
        /// Subtract the value of another instance from this instance and returns the result.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns></returns>
        public override IGraphDataPoint Subtract(IGraphDataPoint other)
        {
            return new DateTimeDataPoint(new DateTime(this.Value.Ticks - (other as DateTimeDataPoint).Value.Ticks));
        }

        /// <summary>
        /// Multiplies the value of this instance with another instance value and returns the result.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns></returns>
        public override IGraphDataPoint Multiply(IGraphDataPoint other)
        {
            return new DateTimeDataPoint(new DateTime(this.Value.Ticks * (other as DateTimeDataPoint).Value.Ticks));
        }

        /// <summary>
        /// Divides the value of this instance with another instance value and returns the result.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns></returns>
        public override IGraphDataPoint Divide(IGraphDataPoint other)
        {
            return new DateTimeDataPoint(new DateTime(this.Value.Ticks / (other as DateTimeDataPoint).Value.Ticks));
        }

        /// <summary>
        /// Returns the percentage value of this instance between the specified minimum and maximum values.
        /// </summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns></returns>
        public override double ComputeRelativePosition(IGraphDataPoint min, IGraphDataPoint max)
        {
            DateTime dMin = min as DateTimeDataPoint;
            DateTime dMax = max as DateTimeDataPoint;

            if (dMax.Ticks - dMin.Ticks == 0) //Prevent divide by zero
            {
                return dMin.Ticks;
            }

            var result = ((Value.Ticks - dMin.Ticks) * 100) / (dMax.Ticks - dMin.Ticks);

            return double.IsNaN(result) ? dMin.Ticks : result;
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
            double minimum = ((DateTime)min.GetValue()).Ticks;
            double maximum = ((DateTime)max.GetValue()).Ticks;

            return new DateTimeDataPoint(new DateTime((long)(minimum + (maximum - minimum) * percentage)));
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
            double minimum = ((DateTime)min.GetValue()).Ticks;
            double maximum = ((DateTime)max.GetValue()).Ticks;

            return Enumerable.Range(0, count).
                Select(i => minimum + (maximum - minimum) * ((double)i / (count - 1))).
                Select(x => new DateTimeDataPoint(new DateTime((long)x)));
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
            return new DateTimeDataPoint(DateTime.Parse(value));
        }

        /// <summary>
        /// Return the default margins for this data point type.
        /// <see cref="P:RealTimeGraphX.IGraphRange.AutoYFallbackMode" /> and <see cref="F:RealTimeGraphX.GraphRangeAutoYFallBackMode.Margins" />.
        /// </summary>
        /// <returns></returns>
        protected override DateTimeDataPoint OnGetDefaultMargins()
        {
            return new DateTimeDataPoint(new DateTime(1, 1, 1, 1, 1, 1));
        }
    }
}
