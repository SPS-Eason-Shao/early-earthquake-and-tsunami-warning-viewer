﻿using CommunityToolkit.Mvvm.ComponentModel;
using EasonEetwViewer.Models;
using EasonEetwViewer.ViewModels.ViewModelBases;
using Mapsui;
using Mapsui.Limiting;
using Mapsui.Projections;

namespace EasonEetwViewer.ViewModels;
internal partial class MapViewModelBase : PageViewModelBase
{
    [ObservableProperty]
    private Map _map = new();

    // Adapted from https://mapsui.com/samples/ - Navigation - Keep within Extent
    internal MapViewModelBase(UserOptions options) : base(options)
    {
        //MRect bounds = GetLimitsOfJapan();
        MRect view = GetMainLimitsOfJapan();
        Map.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());
        //Map.Navigator.Limiter = new ViewportLimiterKeepWithinExtent();
        Map.Navigator.RotationLock = true;
        //Map.Navigator.OverridePanBounds = bounds;
        //Map.Navigator.ZoomToBox(view);
        Map.Home = n => n.ZoomToBox(view);
    }

    private static MRect GetLimitsOfJapan()
    {
        (double minX, double minY) = SphericalMercator.FromLonLat(122, 20);
        (double maxX, double maxY) = SphericalMercator.FromLonLat(154, 46);
        return new MRect(minX, minY, maxX, maxY);
    }

    private static MRect GetMainLimitsOfJapan()
    {
        (double minX, double minY) = SphericalMercator.FromLonLat(122, 27);
        (double maxX, double maxY) = SphericalMercator.FromLonLat(154, 46);
        return new MRect(minX, minY, maxX, maxY);
    }
}