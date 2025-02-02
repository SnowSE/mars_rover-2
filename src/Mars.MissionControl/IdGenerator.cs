﻿using System.Text;

namespace Mars.MissionControl;

/// <summary>
/// Originally from https://github.com/dotnet/aspnetcore/blob/main/src/Servers/Kestrel/shared/CorrelationIdGenerator.cs
/// The .NET Foundation licenses this file to you under the MIT license.
/// </summary>
internal static class IdGenerator
{
    // Base32 encoding - in ascii sort order for easy text based sorting
    private static readonly char[] chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();

    // Seed the _lastConnectionId for this application instance with
    // the number of 100-nanosecond intervals that have elapsed since 12:00:00 midnight, January 1, 0001
    // for a roughly increasing _lastId over restarts
    private static long _lastId = DateTime.UtcNow.Ticks;

    public static string GetNextId() => GenerateRandomId();// GenerateId(Interlocked.Increment(ref _lastId));


    private static string GenerateRandomId(int length = 16)
    {
        StringBuilder id = new();
        for (int i = 0; i < length; i++)
        {
            id.Append(chars[Random.Shared.Next(chars.Length)]);
        }
        return id.ToString();
    }

    private static string GenerateId(long id)
    {
        return string.Create(13, id, (buffer, value) =>
        {
            char[] encode32Chars = chars;

            buffer[12] = encode32Chars[value & 31];
            buffer[11] = encode32Chars[(value >> 5) & 31];
            buffer[10] = encode32Chars[(value >> 10) & 31];
            buffer[9] = encode32Chars[(value >> 15) & 31];
            buffer[8] = encode32Chars[(value >> 20) & 31];
            buffer[7] = encode32Chars[(value >> 25) & 31];
            buffer[6] = encode32Chars[(value >> 30) & 31];
            buffer[5] = encode32Chars[(value >> 35) & 31];
            buffer[4] = encode32Chars[(value >> 40) & 31];
            buffer[3] = encode32Chars[(value >> 45) & 31];
            buffer[2] = encode32Chars[(value >> 50) & 31];
            buffer[1] = encode32Chars[(value >> 55) & 31];
            buffer[0] = encode32Chars[(value >> 60) & 31];
        });
    }
}
