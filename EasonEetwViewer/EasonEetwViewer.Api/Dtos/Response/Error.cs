﻿using System.Text.Json.Serialization;
using EasonEetwViewer.Api.Dtos.Enum;
using EasonEetwViewer.Api.Dtos.Record;
using EasonEetwViewer.Api.Dtos.ResponseBase;

namespace EasonEetwViewer.Api.Dtos.Response;

/// <summary>
/// Represents an Error HTTP response.
/// </summary>
public record Error : ApiBase
{
    /// <summary>
    /// The <c>status</c> property. Always set to <see cref="ResponseStatus.Error"/>.
    /// </summary>
    [JsonPropertyName("status")]
    public override ResponseStatus ResponseStatus { get; } = ResponseStatus.Error;
    /// <summary>
    /// The <c>error</c> property. An object representing the error returned by the API Call.
    /// </summary>
    [JsonPropertyName("error")]
    public required ErrorDetails ErrorDetails { get; init; }
}