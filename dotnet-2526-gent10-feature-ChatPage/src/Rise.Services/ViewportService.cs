// Services/ViewportService.cs
using Microsoft.JSInterop;

namespace Rise.Services;

/// <summary>
/// Service voor het detecteren van viewport grootte (mobile/tablet/desktop)
/// </summary>
public interface IViewportService
{
    Task<ViewportSize> GetViewportSizeAsync();
    Task<bool> IsMobileAsync();
    Task<int> GetWidthAsync();
    Task<int> GetHeightAsync();
}

public class ViewportService : IViewportService
{
    private readonly IJSRuntime _jsRuntime;

    public ViewportService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<ViewportSize> GetViewportSizeAsync()
    {
        var width = await GetWidthAsync();
        
        return width switch
        {
            < 768 => ViewportSize.Mobile,
            < 1024 => ViewportSize.Tablet,
            _ => ViewportSize.Desktop
        };
    }

    public async Task<bool> IsMobileAsync()
    {
        var size = await GetViewportSizeAsync();
        return size == ViewportSize.Mobile;
    }

    public async Task<int> GetWidthAsync()
    {
        return await _jsRuntime.InvokeAsync<int>("getViewportWidth");
    }

    public async Task<int> GetHeightAsync()
    {
        return await _jsRuntime.InvokeAsync<int>("getViewportHeight");
    }
}

public enum ViewportSize
{
    Mobile,
    Tablet,
    Desktop
}