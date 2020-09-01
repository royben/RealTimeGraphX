using System;
using System.Collections.Generic;
using System.Text;

namespace RealTimeGraphX
{
    /// <summary>
    /// Represents an <see cref="IGraphRange"/> AutoY fall-back mode for then all Y values are equal.
    /// </summary>
    public enum GraphRangeAutoYFallBackMode
    {
        /// <summary>
        /// Create fake min/max margins of 0.5 from the Y value.
        /// </summary>
        Margins,
        /// <summary>
        /// Fallback to <see cref="IGraphRange.MinimumY"/> and <see cref="IGraphRange.MaximumY"/>.
        /// </summary>
        MinMax,
        /// <summary>
        /// Do nothing when all Y values are equal.
        /// </summary>
        None,
    }
}
