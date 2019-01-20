using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeGraphX
{
    /// <summary>
    /// Represents an <see cref="IGraphDataPoint"/> base class.
    /// </summary>
    /// <seealso cref="RealTimeGraphX.IGraphDataPoint" />
    public abstract class GraphDataPoint : IGraphDataPoint
    {
        #region IComparable

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="obj" /> in the sort order. Zero This instance occurs in the same position in the sort order as <paramref name="obj" />. Greater than zero This instance follows <paramref name="obj" /> in the sort order.
        /// </returns>
        public abstract int CompareTo(object obj);

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        #region Logical Operators

        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator >(GraphDataPoint a, GraphDataPoint b)
        {
            return a.CompareTo(b) == 1;
        }

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator <(GraphDataPoint a, GraphDataPoint b)
        {
            return a.CompareTo(b) == -1;
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(GraphDataPoint a, GraphDataPoint b)
        {
            if (Object.ReferenceEquals(a, null) && Object.ReferenceEquals(b, null)) return true;
            if (Object.ReferenceEquals(a, null) && !Object.ReferenceEquals(b, null)) return false;

            return a.CompareTo(b) == 0;
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(GraphDataPoint a, GraphDataPoint b)
        {
            if (Object.ReferenceEquals(a, null) && Object.ReferenceEquals(b, null)) return false;
            if (Object.ReferenceEquals(a, null) && !Object.ReferenceEquals(b, null)) return true;

            return a.CompareTo(b) != 0;
        }

        #endregion

        #region Arithmetic Operators

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static GraphDataPoint operator -(GraphDataPoint a, GraphDataPoint b)
        {
            return (GraphDataPoint)a.Subtract(b);
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static GraphDataPoint operator +(GraphDataPoint a, GraphDataPoint b)
        {
            return (GraphDataPoint)a.Add(b);
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static GraphDataPoint operator /(GraphDataPoint a, GraphDataPoint b)
        {
            return (GraphDataPoint)a.Divide(b);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static GraphDataPoint operator *(GraphDataPoint a, GraphDataPoint b)
        {
            return (GraphDataPoint)a.Multiply(b);
        }

        #endregion

        #region IGraphDataPoint

        /// <summary>
        /// Sums the value of this instance with another instance value and returns the result.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns></returns>
        public abstract IGraphDataPoint Add(IGraphDataPoint other);

        /// <summary>
        /// Subtract the value of another instance from this instance and returns the result.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns></returns>
        public abstract IGraphDataPoint Subtract(IGraphDataPoint other);

        /// <summary>
        /// Multiplies the value of this instance with another instance value and returns the result.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns></returns>
        public abstract IGraphDataPoint Multiply(IGraphDataPoint other);

        /// <summary>
        /// Divides the value of this instance with another instance value and returns the result.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns></returns>
        public abstract IGraphDataPoint Divide(IGraphDataPoint other);

        /// <summary>
        /// Returns the percentage value of this instance between the specified minimum and maximum values.
        /// </summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns></returns>
        public abstract double ComputeRelativePosition(IGraphDataPoint min, IGraphDataPoint max);

        /// <summary>
        /// Returns the absolute value of the specified percentage value between the specified minimum and maximum values.
        /// </summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <param name="percentage">The percentage.</param>
        /// <returns></returns>
        public abstract IGraphDataPoint ComputeAbsolutePosition(IGraphDataPoint min, IGraphDataPoint max, double percentage);

        /// <summary>
        /// Creates a range of values from the specified minimum and maximum.
        /// </summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public abstract IEnumerable<IGraphDataPoint> CreateRange(IGraphDataPoint min, IGraphDataPoint max, int count);

        /// <summary>
        /// Gets the inner value.
        /// </summary>
        /// <returns></returns>
        public abstract object GetValue();

        /// <summary>
        /// Sets the inner value.
        /// </summary>
        /// <param name="value">The value.</param>
        public abstract void SetValue(object value);

        /// <summary>
        /// Returns a formated string of this data point.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public abstract string ToString(string format);

        /// <summary>
        /// Parses the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public abstract IGraphDataPoint Parse(string value);

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type of this graph data point.
        /// </summary>
        public Type Type
        {
            get { return GetType(); }
        }

        #endregion
    }

    /// <summary>
    /// Represents an <see cref="IGraphDataPoint"/> base class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TDataType">The type of the data type.</typeparam>
    /// <seealso cref="RealTimeGraphX.GraphDataPoint" />
    public abstract class GraphDataPoint<T, TDataType> : GraphDataPoint where T : IComparable where TDataType : GraphDataPoint<T, TDataType>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the encapsulated data point value.
        /// </summary>
        public T Value { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphDataPoint{T, TDataType}"/> class.
        /// </summary>
        public GraphDataPoint()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphDataPoint{T, TDataType}"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public GraphDataPoint(T value)
        {
            Value = value;
        }

        #endregion

        #region IComparable

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="obj" /> in the sort order. Zero This instance occurs in the same position in the sort order as <paramref name="obj" />. Greater than zero This instance follows <paramref name="obj" /> in the sort order.
        /// </returns>
        public override int CompareTo(object obj)
        {
            if (Object.ReferenceEquals(obj, null)) return -1;

            GraphDataPoint<T, TDataType> b = obj as GraphDataPoint<T, TDataType>;
            return Value.CompareTo(b.Value);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            var @base = obj as GraphDataPoint<T, TDataType>;
            return @base != null &&
                   EqualityComparer<T>.Default.Equals(Value, @base.Value);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return -1937169414 + EqualityComparer<T>.Default.GetHashCode(Value);
        }

        #endregion

        #region ToString

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Value.ToString();
        }

        #endregion

        #region IGraphDataPoint

        /// <summary>
        /// Gets the inner value.
        /// </summary>
        /// <returns></returns>
        public override object GetValue()
        {
            return Value;
        }

        /// <summary>
        /// Sets the inner value.
        /// </summary>
        /// <param name="value">The value.</param>
        public override void SetValue(object value)
        {
            Value = (T)value;
        }

        #endregion

        #region Parsing

        /// <summary>
        /// Parses the specified value and returns a new instance of <see cref="TDataType"/> data point.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static TDataType ParseFrom(String value)
        {
            return Activator.CreateInstance<TDataType>().Parse(value) as TDataType;
        }

        #endregion

        #region Implicit Operators

        /// <summary>
        /// Performs an implicit conversion from <see cref="T"/> to <see cref="GraphDataPoint{T, TDataType}"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator GraphDataPoint<T, TDataType>(T value)
        {
            return Activator.CreateInstance(typeof(TDataType), value) as GraphDataPoint<T, TDataType>;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="GraphDataPoint{T, TDataType}"/> to <see cref="T"/>.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator T(GraphDataPoint<T, TDataType> instance)
        {
            return instance.Value;
        }

        #endregion
    }
}
