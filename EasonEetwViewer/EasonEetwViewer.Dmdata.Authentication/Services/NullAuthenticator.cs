﻿using System.Net.Http.Headers;
using EasonEetwViewer.Dmdata.Authentication.Abstractions;
using EasonEetwViewer.Dmdata.Authentication.Exceptions;

namespace EasonEetwViewer.Dmdata.Authentication.Services;
/// <summary>
/// Represents an authenticator without authentication.
/// </summary>
internal sealed class NullAuthenticator : IAuthenticator
{
    /// <inheritdoc/>
    /// <exception cref="NullAuthenticatiorException">This class does not support this operation.</exception>
    public Task<AuthenticationHeaderValue> GetAuthenticationHeaderAsync()
        => throw new NullAuthenticatiorException();
    /// <inheritdoc/>
    public override string? ToString()
        => null;
    /// <summary>
    /// Creates a new instance of <see cref="NullAuthenticator"/>.
    /// </summary>
    private NullAuthenticator() { }
    /// <summary>
    /// The shared instance of <see cref="NullAuthenticator"/>.
    /// </summary>
    public static IAuthenticator Instance { get; } = new NullAuthenticator();
}
