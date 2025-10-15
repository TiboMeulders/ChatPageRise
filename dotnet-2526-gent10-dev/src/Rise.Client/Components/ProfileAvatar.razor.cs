using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Rise.Client.Components;

public partial class ProfileAvatar : ComponentBase
{
    [Parameter] public string? FirstName { get; set; }
    [Parameter] public string? LastName { get; set; }
    [Parameter] public string? ImageUrl { get; set; }
    [Parameter] public string Size { get; set; } = "xl"; // sm, md, lg, xl, 2xl
    [Parameter] public string Class { get; set; } = string.Empty;
    [Parameter] public string? Title { get; set; }
    [Parameter] public EventCallback OnClick { get; set; }

    protected string CssSize => Size switch
    {
        "sm" => "w-10 h-10",
        "md" => "w-12 h-12",
        "lg" => "w-16 h-16",
        "2xl" => "w-24 h-24",
        _ => "w-24 h-24"
    };

    protected string CssText => (Size switch
    {
        "sm" => "text-sm",
        "md" => "text-base",
        "lg" => "text-xl",
        "2xl" => "text-3xl",
        _ => "text-2xl"
    }) + " font-bold text-white";

    protected string CssBase => string.IsNullOrWhiteSpace(ImageUrl)
        ? "bg-gray-300"
        : string.Empty;

    protected async Task HandleClick()
    {
        if (OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync();
        }
    }

    protected static string GetInitials(string? firstName, string? lastName)
    {
        var f = (firstName ?? string.Empty).Trim();
        var l = (lastName ?? string.Empty).Trim();

        char c1 = f.Length > 0 ? char.ToUpperInvariant(f[0]) : '\0';
        char c2 = l.Length > 0 ? char.ToUpperInvariant(l[0]) : '\0';

        if (c1 == '\0' && c2 == '\0') return "?";
        if (c2 == '\0') return c1.ToString();
        if (c1 == '\0') return c2.ToString();
        return new string(new[] { c1, c2 });
    }
}

