using System;
using System.Collections.Generic;
using System.Text;

namespace CacheManager.Caching
{
    public class RangeValue : IComparable<RangeValue>, IEquatable<RangeValue>
    {
        public override string ToString()
        {
            if (this.IsChar)
            {
                return new string((char)this.RawValue, 1);
            }
            else
            {
                return this.RawValue.ToString();
            }
        }

        public RangeType Type { get; private set; }

        public virtual object RawValue { get; private set; }

        public RangeValue(Int32 value)
        {
            Type = RangeType.Int32;
            this.RawValue = value;
        }

        public RangeValue(Int64 value)
        {
            Type = RangeType.Int64;
            this.RawValue = value;
        }

        public RangeValue(char value)
        {
            Type = RangeType.Char;
            this.RawValue = value;
        }

        public RangeValue(Double value)
        {
            Type = RangeType.Double;
            this.RawValue = value;
        }

        public RangeValue(Decimal value)
        {
            Type = RangeType.Decimal;
            this.RawValue = value;
        }

        public RangeValue(object value)
        {
            this.RawValue = value;
            if (value == null)
            {
                this.Type = RangeType.Null;
            }
            if (value is Int32) this.Type = RangeType.Int32;
            else if (value is Int64) this.Type = RangeType.Int64;
            else if (value is double) this.Type = RangeType.Double;
            else if (value is char) this.Type = RangeType.Char;
            else if (value is decimal) this.Type = RangeType.Decimal;
            else
            {
                this.Type = RangeType.Null;
                this.RawValue = null;
            }
        }

        public char AsChar
        {
            get
            {
                return this.Type == RangeType.Char ? (char)this.RawValue : default(char);
            }
        }

        public Int32 AsInt32
        {
            get { return this.IsNumber ? Convert.ToInt32(this.RawValue) : default(Int32); }
        }

        public Int64 AsInt64
        {
            get { return this.IsNumber ? Convert.ToInt64(this.RawValue) : default(Int64); }
        }


        public double AsDouble
        {
            get { return this.IsNumber ? Convert.ToDouble(this.RawValue) : default(Double); }
        }

        public decimal AsDecimal
        {
            get { return this.IsNumber ? Convert.ToDecimal(this.RawValue) : default(Decimal); }
        }

        public bool IsChar
        {
            get { return this.Type == RangeType.Char; }
        }

        public bool IsInt32
        {
            get { return this.Type == RangeType.Int32; }
        }

        public bool IsInt64
        {
            get { return this.Type == RangeType.Int64; }
        }

        public bool IsDouble
        {
            get { return this.Type == RangeType.Double; }
        }

        public bool IsDecimal
        {
            get { return this.Type == RangeType.Decimal; }
        }

        public bool IsNumber
        {
            get { return this.IsInt32 || this.IsInt64 || this.IsDouble || this.IsDecimal; }
        }

        //Int32
        public static implicit operator RangeValue(Int32 value)
        {
            return new RangeValue(value);
        }

        //Int32
        public static implicit operator Int32(RangeValue value)
        {
            if (value.IsChar)
            {
                return (Int32)(char)value.RawValue;
            }
            else
                return (Int32)value.RawValue;
        }

        public static implicit operator RangeValue(Int64 value)
        {
            return new RangeValue(value);
        }

        //Int64
        public static implicit operator Int64(RangeValue value)
        {
            return (Int64)value.RawValue;
        }


        // Double
        public static implicit operator Double(RangeValue value)
        {
            return (Double)value.RawValue;
        }

        // Double
        public static implicit operator RangeValue(Double value)
        {
            return new RangeValue(value);
        }

        // Decimal
        public static implicit operator Decimal(RangeValue value)
        {
            return (Decimal)value.RawValue;
        }

        // Decimal
        public static implicit operator RangeValue(Decimal value)
        {
            return new RangeValue(value);
        }

        // String
        public static implicit operator char(RangeValue value)
        {
            return (char)value.RawValue;
        }

        // String
        public static implicit operator RangeValue(char value)
        {
            return new RangeValue(value);
        }

        #region IComparable<BsonValue>, IEquatable<BsonValue>
        public int CompareTo(RangeValue other)
        {
            // first, test if types are different
            if (this.Type != other.Type)
            {
                // if both values are number, convert them to Decimal (128 bits) to compare
                // it's the slowest way, but more secure
                if (this.IsNumber && other.IsNumber)
                {
                    return Convert.ToDecimal(this.RawValue).CompareTo(Convert.ToDecimal(other.RawValue));
                }
                // if not, order by sort type order
                else
                {
                    return this.Type.CompareTo(other.Type);
                }
            }
            switch (this.Type)
            {
                case RangeType.Null:
                    return 0;
                case RangeType.Int32: return ((Int32)this.RawValue).CompareTo((Int32)other.RawValue);
                case RangeType.Int64: return ((Int64)this.RawValue).CompareTo((Int64)other.RawValue);
                case RangeType.Double: return ((Double)this.RawValue).CompareTo((Double)other.RawValue);
                case RangeType.Decimal: return ((Decimal)this.RawValue).CompareTo((Decimal)other.RawValue);

                case RangeType.Char: return ((char)this.RawValue).CompareTo((char)other.RawValue);
                default: throw new NotImplementedException();
            }
        }

        public bool Equals(RangeValue other)
        {
            return this.CompareTo(other) == 0;
        }

        #endregion

        #region Operators
        public static bool operator ==(RangeValue lhs, RangeValue rhs)
        {
            if (object.ReferenceEquals(lhs, null)) return object.ReferenceEquals(rhs, null);
            if (object.ReferenceEquals(rhs, null)) return false; // don't check type because sometimes different types can be ==

            return lhs.Equals(rhs);
        }

        public static bool operator !=(RangeValue lhs, RangeValue rhs)
        {
            return !(lhs == rhs);
        }

        public static bool operator >=(RangeValue lhs, RangeValue rhs)
        {
            return lhs.CompareTo(rhs) >= 0;
        }

        public static bool operator >(RangeValue lhs, RangeValue rhs)
        {
            return lhs.CompareTo(rhs) > 0;
        }

        public static bool operator <(RangeValue lhs, RangeValue rhs)
        {
            return lhs.CompareTo(rhs) < 0;
        }

        public static bool operator <=(RangeValue lhs, RangeValue rhs)
        {
            return lhs.CompareTo(rhs) <= 0;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(new RangeValue(obj));
        }

        public override int GetHashCode()
        {
            var hash = 17;
            hash = 37 * hash + this.Type.GetHashCode();
            hash = 37 * hash + this.RawValue.GetHashCode();
            return hash;
        }
        #endregion
    }
}
