using Newtonsoft.Json;
using System;

namespace LibraryShared.Converters
{
	public class DecimalToIntConverter : JsonConverter<decimal>
	{
		public override decimal ReadJson(JsonReader reader, Type objectType, decimal existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Float || reader.TokenType == JsonToken.Integer)
			{
				return Convert.ToDecimal(reader.Value);
			}
			throw new JsonSerializationException("Expected float or integer value.");
		}

		public override void WriteJson(JsonWriter writer, decimal value, JsonSerializer serializer)
		{
			writer.WriteValue(Convert.ToInt64(value));
		}
	}
}
