﻿using System.Text.Json.Serialization;

namespace EasonEetwViewer.KyoshinMonitor.Dtos;
/// <summary>
/// Represents an observation point.
/// </summary>
public record ObservationPoint
{
    /// <summary>
    /// The property <c>type</c>, whether it is in <c>K-NET</c> or <c>KiK-net</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public required PointType Type { get; init; }
    /// <summary>
    /// The property <c>code</c>, the unique code of the observation point.
    /// </summary>
    [JsonPropertyName("code")]
    public required string Code { get; init; }
    /// <summary>
    /// The property <c>name</c>, the unique name of the observation point.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }
    /// <summary>
    /// The property <c>region</c>, the region of the observation point.
    /// </summary>
    [JsonPropertyName("region")]
    public required string Region { get; init; }
    /// <summary>
    /// The property <c>isSuspended</c>, whether the observation point is in use or is suspended.
    /// </summary>
    [JsonPropertyName("isSuspended")]
    public required bool IsSuspended { get; init; }
    /// <summary>
    /// The property <c>location</c>, the geographic location of the observation point.
    /// </summary>
    [JsonPropertyName("location")]
    public required GeographicCoordinate Location { get; init; }
    /// <summary>
    /// The property <c>point</c>, the pixel position of the observation point.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("point")]
    public required PixelCoordinate Point { get; init; }
}