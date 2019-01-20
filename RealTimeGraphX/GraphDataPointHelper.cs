using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeGraphX
{
    /// <summary>
    /// Represents an <see cref="IGraphDataPoint"/> helper class.
    /// </summary>
    public static class GraphDataPointHelper
    {
        /// <summary>
        /// Returns the absolute value of the specified percentage value between the specified minimum and maximum values.
        /// </summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <param name="percentage">The percentage.</param>
        /// <returns></returns>
        public static T ComputeAbsolutePosition<T>(T min, T max, double percentage) where T : class, IGraphDataPoint
        {
            return (Activator.CreateInstance(min.GetType()) as T).ComputeAbsolutePosition(min, max, percentage) as T;
        }

        /// <summary>
        /// Creates a range of values from the specified minimum and maximum.
        /// </summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public static IEnumerable<T> CreateRange<T>(IGraphDataPoint min, IGraphDataPoint max, int count) where T : class, IGraphDataPoint
        {
            return (Activator.CreateInstance(min.GetType()) as T).CreateRange(min, max, count) as IEnumerable<T>;
        }

        /// <summary>
        /// Initializes a new instance of the specified <see cref="IGraphDataPoint"/> type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Init<T>() where T : class, IGraphDataPoint
        {
            return Activator.CreateInstance(typeof(T)) as T;
        }
    }
}
