using System;
using System.Globalization;
using Guards;

namespace Options
{
    [Serializable]
    public struct Option<T> : IEquatable<Option<T>>, IEquatable<Option.NoneOption>
    {
        internal Option(T value)
        {
            _hasValue = true;
            _value = value;
        }

        public TResult Match<TResult>(Func<T, TResult> map, Func<TResult> missingValue)
        {
            Guard.CheckNotNull(map, nameof(map));
            Guard.CheckNotNull(missingValue, nameof(missingValue));

            return _hasValue ? map(_value) : missingValue();
        }

        public bool IsNone => !_hasValue;

        public bool IsSome => _hasValue;

        public static implicit operator Option<T>(Option.NoneOption noValue)
        {
            return None;
        }

        public bool Equals(Option<T> other)
        {
            if (_hasValue != other._hasValue)
            {
                return false;
            }

            return !_hasValue || Equals(_value, other._value);
        }

        public bool Equals(Option.NoneOption other)
        {
            return !_hasValue;
        }

        public override bool Equals(object other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (other is Option<T> option)
            {
                return Equals(option);
            }

            if (other is Option.NoneOption noneOption)
            {
                return Equals(noneOption);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return _hasValue
                ? ReferenceEquals(_value, null) ? -1 : _value.GetHashCode()
                : 0;
        }

        public override string ToString()
        {
            return _hasValue
                ? string.Format(CultureInfo.InvariantCulture, "Value: {0}", _value)
                : "No Value";
        }

        public static bool operator ==(Option<T> left, Option<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Option<T> left, Option<T> right)
        {
            return !left.Equals(right);
        }

        private static readonly Option<T> None = new Option<T>();

        private readonly bool _hasValue;
        private readonly T _value;
    }

    public static class Option
    {
        [Serializable]
        public struct NoneOption
        {
            public override bool Equals(object obj)
            {
                if (obj == null)
                {
                    throw new ArgumentNullException(nameof(obj));
                }

                var type = obj.GetType();
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Option<>))
                {
                    return obj.Equals(this);
                }

                if (obj is NoneOption)
                {
                    return true;
                }

                return base.Equals(obj);
            }

            public bool IsNone => true;

            public bool IsSome => false;

            public override int GetHashCode()
            {
                return 0;
            }
        }

        public static Option<T> Some<T>(T value)
        {
            if (typeof(T).IsClass || typeof(T).IsInterface)
            {
                Guard.CheckTrue(value != null, "Cannot create Option.Some for 'null' value of referenced type.");
            }

            return new Option<T>(value);
        }

        public static NoneOption None()
        {
            return new NoneOption();
        }
    }
}