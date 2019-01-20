namespace RealTimeGraphX
{
    /// <summary>
    /// Represents a graph transformation.
    /// </summary>
    public class GraphTransform
    {
        /// <summary>
        /// Gets or sets the horizontal scale factor.
        /// </summary>
        public float ScaleX { get; set; }

        /// <summary>
        /// Gets or sets the vertical scale factor.
        /// </summary>
        public float ScaleY { get; set; }

        /// <summary>
        /// Gets or sets the horizontal translate transformation.
        /// </summary>
        public float TranslateX { get; set; }

        /// <summary>
        /// Gets or sets the vertical translate transformation.
        /// </summary>
        public float TranslateY { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphTransform"/> class.
        /// </summary>
        public GraphTransform()
        {
            ScaleX = 1;
            ScaleY = 1;
            TranslateX = 0;
            TranslateY = 0;
        }
    }
}
