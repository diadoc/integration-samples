using System.Text.Json;
using Diadoc.Api;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SampleWebApp.Oidc.Pages;

[Authorize]
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IDiadocApi _diadocApi;

    public IndexModel(ILogger<IndexModel> logger, IDiadocApi diadocApi)
    {
        _logger = logger;
        _diadocApi = diadocApi;
    }

    public async Task OnGet()
    {
        ViewData["UserName"] = User.FindFirst("name")!.Value;
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        var getBoxResponse = await _diadocApi.GetMyOrganizationsAsync(accessToken);
        ViewData["DiadocRequest"] = $"/GetMyOrganizations";
        ViewData["DiadocResponse"] = JsonSerializer.Serialize(getBoxResponse,
            new JsonSerializerOptions(JsonSerializerOptions.Default) { WriteIndented = true });
    }
}