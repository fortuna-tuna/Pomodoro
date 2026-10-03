using System;
using System.Collections.Generic;
using System.Text;

namespace Pomodoro.Common.Helpers
{
    internal static class NetworkHelper
    {
       public static async Task<int> ReadExactAsync(Stream stream, byte[] buffer, int offset, int count, CancellationToken ct = default)
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
