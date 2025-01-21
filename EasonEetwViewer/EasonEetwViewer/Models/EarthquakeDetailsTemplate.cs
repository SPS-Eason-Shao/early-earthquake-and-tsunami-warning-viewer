﻿using EasonEetwViewer.HttpRequest.DmdataComponent;
using EasonEetwViewer.HttpRequest.Dto.ApiResponse.Enum;

namespace EasonEetwViewer.Models;
internal record EarthquakeDetailsTemplate
{
    internal EarthquakeDetailsTemplate(string eventId, Intensity? intensity, DateTimeOffset? originTime, Hypocentre? hypocentre, Magnitude? magnitude, string? informationalText, DateTimeOffset? lastUpdated, List<DetailIntensityTemplate> detailDisplay)
    {
        EventId = eventId;
        Intensity = intensity;
        OriginTime = originTime;
        Hypocentre = hypocentre;
        Magnitude = magnitude;
        InformationalText = informationalText;
        LastUpdated = lastUpdated;
        DetailDisplay = detailDisplay;
    }
    internal string? EventId { get; private init; }
    internal Intensity? Intensity { get; private init; }
    internal DateTimeOffset? OriginTime { get; private init; }
    internal DateTimeOffset? LastUpdated { get; private init; }
    internal Hypocentre? Hypocentre { get; private init; }
    internal Magnitude? Magnitude { get; private init; }
    internal string? InformationalText { get; private init; }
    internal List<DetailIntensityTemplate> DetailDisplay { get; private init; }
}
