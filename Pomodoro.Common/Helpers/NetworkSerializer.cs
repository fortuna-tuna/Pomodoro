using Pomodoro.Common.Enum;
using Pomodoro.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Pomodoro.Common.Helpers
{
    public static class NetworkSerializer
    {
        public static byte[] SerializeMessage<T>(MessageType type, T payloadDto)
        {
            string jsonPayload = JsonSerializer.Serialize(payloadDto);

            var message = new NetworkMessage
            {
                Type = type,
                Payload = jsonPayload
            };

            var jsonMessage = JsonSerializer.Serialize(message);
            return Encoding.UTF8.GetBytes(jsonMessage);
        }

        public static NetworkMessage? DeserializeMessage(byte[] message)
        {
            var stringMessage = Encoding.UTF8.GetString(message);
            return JsonSerializer.Deserialize<NetworkMessage>(stringMessage);
        }

        public static T? DeserializePayload<T>(string payload)
        {
            return JsonSerializer.Deserialize<T>(payload);
        }
    }
}
