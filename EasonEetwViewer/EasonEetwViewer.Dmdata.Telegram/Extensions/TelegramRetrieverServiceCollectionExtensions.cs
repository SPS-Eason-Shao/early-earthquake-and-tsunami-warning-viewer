﻿using EasonEetwViewer.Dmdata.Authentication.Abstractions;
using EasonEetwViewer.Dmdata.Telegram.Abstractions;
using EasonEetwViewer.Dmdata.Telegram.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EasonEetwViewer.Dmdata.Telegram.Extensions;
/// <summary>
/// Provides extension methods for <see cref="IServiceCollection"/> to add telegram retriever.
/// </summary>
public static class TelegramRetrieverServiceCollectionExtensions
{
    /// <summary>
    /// Injects a <see cref="ITelegramRetriever"/> with the given base URI.
    /// </summary>
    /// <param name="services">The instance of <see cref="IServiceCollection"/> for the service to be injected.</param>
    /// <param name="baseUri">The base URI for telegrams.</param>
    /// <returns>The <see cref="IServiceCollection"/> where the service is injected, for chained calls.</returns>
    public static IServiceCollection AddTelegramRetriever(this IServiceCollection services, string baseUri)
        => services
            .AddSingleton<ITelegramParser, TelegramParser>()
            .AddSingleton<ITelegramRetriever>(sp
            => new TelegramRetriever(
                    baseUri,
                    sp.GetRequiredService<ITelegramParser>(),
                    sp.GetRequiredService<ILogger<TelegramRetriever>>(),
                    sp.GetRequiredService<IAuthenticationHelper>()));
}
