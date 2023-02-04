namespace SerilogFromAppBuilder.Models;

public readonly record struct Segment(Coordinate Start, Coordinate End)
{
    public double GetDistanceTo()
    {
        var baseRad = Math.PI * Start.Latitude / 180;
        var targetRad = Math.PI * End.Latitude / 180;
        var theta = Start.Longitude - End.Longitude;
        var thetaRad = Math.PI * theta / 180;

        var dist = (Math.Sin(baseRad) * Math.Sin(targetRad)) + (Math.Cos(baseRad) * Math.Cos(targetRad) * Math.Cos(thetaRad));
        dist = Math.Acos(dist);

        dist = dist * 180 / Math.PI;
        dist = dist * 60 * 1.1515;

        return dist * 1.609344;
    }
}