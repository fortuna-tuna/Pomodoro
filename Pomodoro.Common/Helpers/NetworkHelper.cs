using Pomodoro.Common.Enum;
using Pomodoro.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;


namespace Pomodoro.Common.Helpers
{
    public static class NetworkHelper
    {
        public static async Task WriteExactAsync<T>(Stream stream, MessageType type, T payload)
        {
            var message = NetworkSerializer.SerializeMessage(type, payload);
            byte[] length = BitConverter.GetBytes(message.Length);

            await stream.WriteAsync(length, 0, length.Length);
            await stream.WriteAsync(message, 0, message.Length);
        }
        public static async Task<byte[]> ReadDataAsync(Stream stream, CancellationToken token)
        {
            var lengthBuffer = new byte[4];
            int bytesRead = await ReadExactAsync(stream, lengthBuffer, 0, 4, token);
            if (bytesRead < 4) return null;

            var messageLength = BitConverter.ToInt32(lengthBuffer, 0);
            var buf = new byte[messageLength];
            await ReadExactAsync(stream, buf, 0, messageLength, token);

            return buf;
        }
       private static async Task<int> ReadExactAsync(Stream stream, byte[] buffer, int offset, int count, CancellationToken ct = default)
       {
                int totalBytesRead = 0;
                while (totalBytesRead < count)
                {
                    int bytesRead = await stream.ReadAsync(buffer, offset + totalBytesRead, count - totalBytesRead, ct);
                    if (bytesRead == 0) return totalBytesRead; 
                    totalBytesRead += bytesRead;
                }
                return totalBytesRead;
       }
    }
}
