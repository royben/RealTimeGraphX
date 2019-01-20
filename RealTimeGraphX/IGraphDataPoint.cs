using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RealTimeGraphX
{
    /// <summary>
    /// Represents a graph X or Y data point that can be pushed to a <see cref="IGraphController{TColor, TBrush, TXDataPoint, TYDataPoint}">Graph Controller</see>.
    /// </summary>
    /// <seealso cref="System.IComparable" />
    [TypeConverter(typeof(GraphDataPointTypeConverter))]
    public interface IGraphDataPoint : IComparable
    {
        /// <summary>
        /// Sums the value of this instance with another instance value and returns the result.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns></returns>
        IGraphDataPoint Add(IGraphDataPoint other);

        /// <summary>
        /// Subtract the value of another instance from this instance and returns the result.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns></returns>
        IGraphDataPoint Subtract(IGraphDataPoint other);

        /// <summary>
        /// Multiplies the value of this instance with another instance value and returns the result.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns></returns>
        IGraphDataPoint Multiply(IGraphDataPoint other);

        /// <summary>
        /// Divides the value of this instance with another instance value and returns the result.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns></returns>
        IGraphDataPoint Divide(IGraphDataPoint other);

        /// <summary>
        /// Creates a range of values from the specified minimum and maximum.
        /// </summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        IEnumerable<IGraphDataPoint> CreateRange(IGraphDataPoint min, IGraphDataPoint max, int count);

        /// <summary>
        /// Returns the percentage value of this instance between the specified minimum and maximum values.
        /// </summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns></returns>
        double ComputeRelativePosition(IGraphDataPoint min, IGraphDataPoint max);

        /// <summary>
        /// Returns the absolute value of the specified percentage value between the specified minimum and maximum values.
        /// </summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <param name="percentage">The percentage.</param>
        /// <returns></returns>
        IGraphDataPoint ComputeAbsolutePosition(IGraphDataPoint min, IGraphDataPoint max, double percentage);

        /// <summary>
        /// Gets the encapsulated value.
        /// </summary>
        /// <returns></returns>
        object GetValue();

        /// <summary>
        /// Sets the encapsulated value.
        /// </summary>
        /// <param name="value">The value.</param>
        void SetValue(object value);

        /// <summary>
        /// Returns a formated string of this data point.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        String ToString(String format);

        /// <summary>
        /// Parses the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        IGraphDataPoint Parse(String value);
    }
}
