using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeGraphX.EventArguments
{
    /// <summary>
    /// Represents a graph range change event arguments.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class RangeChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the minimum x value.
        /// </summary>
        public GraphDataPoint MinimumX { get; set; }

        /// <summary>
        /// Gets or sets the maximum x value.
        /// </summary>
        public GraphDataPoint MaximumX { get; set; }

        /// <summary>
        /// Gets or sets the minimum y value.
        /// </summary>
        public GraphDataPoint MinimumY { get; set; }

        /// <summary>
        /// Gets or sets the maximum y value.
        /// </summary>
        public GraphDataPoint MaximumY { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RangeChangedEventArgs"/> class.
        /// </summary>
        public RangeChangedEventArgs()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RangeChangedEventArgs"/> class.
        /// </summary>
        /// <param name="minimumX">The minimum x value.</param>
        /// <param name="maximumX">The maximum x value.</param>
        /// <param name="minimumY">The minimum y value.</param>
        /// <param name="maximumY">The maximum y value.</param>
        public RangeChangedEventArgs(GraphDataPoint minimumX, GraphDataPoint maximumX,GraphDataPoint minimumY,GraphDataPoint maximumY) : this()
        {
            MinimumX = minimumX;
            MaximumX = maximumX;
            MinimumY = minimumY;
            MaximumY = maximumY;
        }
    }
}
