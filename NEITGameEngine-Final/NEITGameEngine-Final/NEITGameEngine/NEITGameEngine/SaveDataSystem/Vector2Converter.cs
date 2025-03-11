using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Xna.Framework;

namespace NEITGameEngine.SaveDataSystem
{
    public class Vector2Converter:JsonConverter<Vector2>
    {
        public override Vector2 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Expected StartObject Token");
            }

            float x = 0;
            float y = 0;

            while (reader.Read()) { 
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    break;
                }
                string propertyName = reader.GetString();

                reader.Read();

                if (propertyName == "X")
                {
                    x = reader.GetSingle();
                }
                else if (propertyName == "Y")
                {
                    y = reader.GetSingle();
                }
            }
            return new Vector2(x, y);
        }

        public override void Write(Utf8JsonWriter writer, Vector2 value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber("X", value.X);
            writer.WriteNumber("Y", value.Y);
            writer.WriteEndObject();
        }
    }
}
