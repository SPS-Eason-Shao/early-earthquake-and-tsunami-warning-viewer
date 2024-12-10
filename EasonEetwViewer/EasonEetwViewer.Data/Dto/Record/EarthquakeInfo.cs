﻿using System.Text.Json.Serialization;
using EasonEetwViewer.HttpRequest.Dto.Enum;

namespace EasonEetwViewer.HttpRequest.Dto.Record;

/// <summary>
/// Represents an earthquake in the API call <c>gd.earthquake.list</c>.
/// </summary>
public record EarthquakeInfo
{
    /// <summary>
    /// The property <c>id</c>. The ID of the earthquake.
    /// </summary>
    [JsonPropertyName("id")]
    public required int EarthquakeId { get; init; }
    /// <summary>
    /// The property <c>type</c>. The type of the earthquake.
    /// </summary>
    [JsonPropertyName("type")]
    public required EarthquakeType Type { get; init; }
    /// <summary>
    /// The property <c>eventId</c>. The Event ID of the earthquake.
    /// </summary>
    [JsonPropertyName("eventId")]
    public required string EventId { get; init; }
    /// <summary>
    /// The property <c>originTime</c>. The time at which the earthquake happened.
    /// <c>null</c> when there is only observation report of the earthquake.
    /// </summary>
    [JsonPropertyName("originTime")]
    public DateTime? OriginTime { get; init; }
    /// <summary>
    /// The property <c>arrivalTime</c>. The time at which the earthquake is detected.
    /// </summary>
    [JsonPropertyName("arrivalTime")]
    public required DateTime ArrivalTime { get; init; }
    /// <summary>
    /// The property <c>hypocenter</c>. The hypocentre of the earthquake.
    /// </summary>
    [JsonPropertyName("hypocenter")]
    public required Hypocentre Hypocentre { get; init; }
    /// <summary>
    /// The property <c>magnitude</c>. The magnitude of the earthquake.
    /// </summary>
    [JsonPropertyName("magnitude")]
    public required Magnitude Magnitude { get; init; }
    /// <summary>
    /// The property <c>maxInt</c>. The maximum intensity observed for the earthquake.
    /// <c>null</c> when this is a distant earthquake.
    /// </summary>
    [JsonPropertyName("maxInt")]
    public Intensity? MaxIntensity { get; init; }
    /// <summary>
    /// The property <c>maxLgInt</c>. The maximum LPGM intensity observed for the earthquake.
    /// <c>null</c> when no LPGM observation report is issued.
    /// </summary>
    [JsonPropertyName("maxLgInt")]
    public LgIntensity? MaxLgIntensity { get; init; }
    /// <summary>
    /// The property <c>lgCategory</c>. The LPGM intensity category of the earthquake.
    /// <c>null</c> when no LPGM observation report is issued.
    /// </summary>
    [JsonPropertyName("lgCategory")]
    public LgCategory? LgCategory { get; init; }
}
