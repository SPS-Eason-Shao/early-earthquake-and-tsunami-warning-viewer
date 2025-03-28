using System.Text.Json.Serialization;

namespace EasonEetwViewer.Dmdata.Api.Dtos.ResponseBase;

/// <summary>
/// Outlines the model of a list response with next pooling options.
/// Abstract and cannot be instantiated.
/// </summary>
/// <typeparam name="T">The type of item in the list.</typeparam>
public abstract record PoolingBase<T> : TokenBase<T>
{
    /// <summary>
    /// The <c>nextPooling</c> property. The pooling token that should be specified in the next API call.
    /// </summary>
    /// <remarks>
    /// <see langword="null"/> when the current call is the final call.
    /// </remarks>
    [JsonPropertyName("nextPooling")]
    public string? NextPooling { get; init; }
    /// <summary>
    /// The <c>nextPoolingInterval</c> property. The time in milliseconds that the program should wait until the next API call.
    /// </summary>
    /// <remarks>
    /// <see langword="null"/> when the current call is the final call.
    /// </remarks>
    [JsonPropertyName("nextPoolingInterval")]
    public int? NextPoolingInterval { get; init; }
}