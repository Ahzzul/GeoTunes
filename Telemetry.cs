using System;

namespace GeoTunes
{
    public class Placement
    {
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
        public double heading { get; set; }
        public double pitch { get; set; }
        public double roll { get; set; }
    }

    public class Acceleration
    {
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
    }

    public class Truck
    {
        public string id { get; set; }
        public string make { get; set; }
        public string model { get; set; }
        public double speed { get; set; }
        public double cruiseControlSpeed { get; set; }
        public bool cruiseControlOn { get; set; }
        public double engineRpm { get; set; }
        public double engineRpmMax { get; set; }
        public double fuel { get; set; }
        public double fuelCapacity { get; set; }
        public bool engineOn { get; set; }
        public bool electricOn { get; set; }
        public Placement placement { get; set; }
        public Acceleration acceleration { get; set; }
        // ... alle weiteren Felder aus deinem JSON hinzufügen
    }

    public class Game
    {
        public bool connected { get; set; }
        public string gameName { get; set; }
        public bool paused { get; set; }
        public DateTime time { get; set; }
        public double timeScale { get; set; }
        public string version { get; set; }
        public string telemetryPluginVersion { get; set; }
    }

    public class Trailer
    {
        public bool attached { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public double mass { get; set; }
        public double wear { get; set; }
        public Placement placement { get; set; }
    }

    public class Job
    {
        public int income { get; set; }
        public DateTime deadlineTime { get; set; }
        public DateTime remainingTime { get; set; }
        public string sourceCity { get; set; }
        public string sourceCompany { get; set; }
        public string destinationCity { get; set; }
        public string destinationCompany { get; set; }
    }

    public class Navigation
    {
        public DateTime estimatedTime { get; set; }
        public double estimatedDistance { get; set; }
        public double speedLimit { get; set; }
    }

    public class TelemetryData
    {
        public Game game { get; set; }
        public Truck truck { get; set; }
        public Trailer trailer { get; set; }
        public Job job { get; set; }
        public Navigation navigation { get; set; }
    }

}
