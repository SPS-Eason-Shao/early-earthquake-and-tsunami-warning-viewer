﻿using System.Text.Json.Serialization;

namespace EasonEetwViewer.HttpRequest.Dto.JsonTelegram;

public record JsonSchemaHead
{
    [JsonPropertyName("_originalId")]
    public required string OriginalId { get; init; }

    [JsonPropertyName("_schema")]
    public required JsonSchemaVersionInfo Schema { get; init; }
    [JsonPropertyName("type")]
    public required string Type { get; init; }
    [JsonPropertyName("title")]
    public required string Title { get; init; }
    [JsonPropertyName("status")]
    public required TelegramStatus Status { get; init; }
    [JsonPropertyName("infoType")]
    public required TelegramType InfoType { get; init; }
    [JsonPropertyName("editorialOffice")]
    public required string EditorialOffice { get; init; }
    [JsonPropertyName("publishingOffice")]
    public required List<string> PublishingOffice { get; init; }
    [JsonPropertyName("pressDateTime")]
    public required DateTime PressDateTime { get; init; }
    [JsonPropertyName("reportDateTime")]
    public required DateTime ReportDateTime { get; init; }
    [JsonPropertyName("targetDateTime")]
    public required DateTime? TargetDateTime { get; init; }
    [JsonPropertyName("targetDateTimeDubious")]
    public string? TargetDateTimeDubious { get; init; }
    [JsonPropertyName("targetDuration")]
    public string? TargetDuration { get; init; }
    [JsonPropertyName("validDateTime")]
    public DateTime? ValidDateTime { get; init; }
    [JsonPropertyName("eventId")]
    public required string? EventId { get; init; }
    [JsonPropertyName("serialNo")]
    public required string? SerialNo { get; init; }
    [JsonPropertyName("infoKind")]
    public required string InfoKind { get; init; }
    [JsonPropertyName("infoKindVersion")]
    public required string InfoKindVersion { get; init; }
    [JsonPropertyName("headline")]
    public required string? Headline { get; init; }
}
