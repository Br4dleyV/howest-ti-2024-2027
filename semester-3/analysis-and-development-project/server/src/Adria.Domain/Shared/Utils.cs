using System.Security.Cryptography;
using Adria.Domain.Models;

namespace Adria.Domain.Shared;

public static class Utils
{
    public static double GetDistanceToLocation(Coordinates startLocation, Coordinates endLocation)
    {
        const double earthRadiusKm = 6371.0;

        double lat1 = DegreesToRadians(startLocation.Latitude);
        double lon1 = DegreesToRadians(startLocation.Longitude);
        double lat2 = DegreesToRadians(endLocation.Latitude);
        double lon2 = DegreesToRadians(endLocation.Longitude);

        double dLat = lat2 - lat1;
        double dLon = lon2 - lon1;

        double a = Math.Pow(Math.Sin(dLat / 2), 2) +
                   Math.Cos(lat1) * Math.Cos(lat2) *
                   Math.Pow(Math.Sin(dLon / 2), 2);

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return earthRadiusKm * c;
    }

    private static double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }

    public static int RandomInt(int min, int max)
    {
        var buffer = new byte[4];
        RandomNumberGenerator.Fill(buffer);
        uint randUInt32 = BitConverter.ToUInt32(buffer);
        return min + (int)(randUInt32 / (double)uint.MaxValue * (max - min));
    }
    
    public static double RandomDouble(int min, int max)
    {
        var buffer = new byte[8];
        RandomNumberGenerator.Fill(buffer);
        ulong randUInt64 = BitConverter.ToUInt64(buffer);
        return min + (randUInt64 / (double)ulong.MaxValue) * (max - min);
    }
}