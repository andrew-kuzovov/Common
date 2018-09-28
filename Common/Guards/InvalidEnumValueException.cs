using System;
using System.Runtime.Serialization;

namespace Guards
{
	[Serializable]
	public sealed class InvalidEnumValueException : Exception
	{
		public InvalidEnumValueException(object value)
			: base($"The value '{Guard.GetNotNull(value, "value")}' is invalid for Enum type '{value.GetType().Name}'.")
		{
			Guard.CheckTrue(value.GetType().IsEnum, "value");
			Value = value;
		}

		private InvalidEnumValueException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public object Value { get; private set; }
	}
}
