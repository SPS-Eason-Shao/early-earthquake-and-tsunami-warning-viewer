﻿using System.Text.Json.Serialization;
using EasonEetwViewer.HttpRequest.Dto.Enum;

namespace EasonEetwViewer.HttpRequest.Dto.ApiPost;

/// <summary>
/// Outlines the necessary information provided in the JSON in the header in <c>socket.start</c> API call.
/// </summary>
public record WebSocketStartPost
{
    /// <summary>
    /// The property <c>classifications</c>. The classifications of telegrams that the WebSocket receives.
    /// </summary>
    [JsonPropertyName("classifications")]
    public required List<ContractClassification> Classifications { get; init; }
    /// <summary>
    /// The property <c>types</c>. The types of telegrams the program receives.
    /// <c>null</c> when receiving all types from the classifications.
    /// </summary>
    [JsonPropertyName("types")]
    public List<string>? Types { get; init; }
    /// <summary>
    /// The property <c>test</c>. Whether the WebSocket receives test telegrams.
    /// </summary>
    [JsonPropertyName("test")]
    public WebSocketTestStatus? TestStatus { get; init; }
    /// <summary>
    /// The property <c>appName</c>. The application name of the WebSocket connection.
    /// <c>null</c> when not indicated.
    /// </summary>
    [JsonPropertyName("appName")]
    public string? AppName { get; init; }
    /// <summary>
    /// The property <c>formatMode</c>. Whether the WebSocket receives JSON serialised data or original data.
    /// </summary>
    [JsonPropertyName("formatMode")]
    public WebSocketFormatMode? FormatMode { get; init; }
}
