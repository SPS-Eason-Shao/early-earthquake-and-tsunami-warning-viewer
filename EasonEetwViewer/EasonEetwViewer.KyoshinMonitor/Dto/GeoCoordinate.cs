﻿using System.Text.Json.Serialization;

namespace EasonEetwViewer.KyoshinMonitor.Dto;
/// <summary>
/// Represents a physical position.
/// </summary>
public record GeoCoordinate
{
    /// <summary>
    /// The property <c>latitude</c>, the latitude of the position.
    /// </summary>
    [JsonPropertyName("latitude")]
    public required double Latitude { get; init; }
    /// <summary>
    /// The property <c>longitude</c>, the longitude of the position.
    /// </summary>
    [JsonPropertyName("longitude")]
    public required double Longitude { get; init; }
}
