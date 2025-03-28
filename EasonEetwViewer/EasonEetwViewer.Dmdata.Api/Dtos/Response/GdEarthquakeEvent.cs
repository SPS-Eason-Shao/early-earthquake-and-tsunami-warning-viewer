﻿using System.Text.Json.Serialization;
using EasonEetwViewer.Dmdata.Api.Dtos.Record.GdEarthquake;
using EasonEetwViewer.Dmdata.Api.Dtos.ResponseBase;

namespace EasonEetwViewer.Dmdata.Api.Dtos.Response;
/// <summary>
/// Represents the result of an API call on <c>gd.earthquake.event</c> API.
/// </summary>
public record GdEarthquakeEvent : SuccessBase
{
    /// <summary>
    /// The property <c>event</c>, the earthquake event.
    /// </summary>
    [JsonPropertyName("event")]
    public required EarthquakeInfoWithTelegrams EarthquakeEvent { get; init; }
}
