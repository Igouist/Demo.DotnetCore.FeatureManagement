using Demo.DotnetCore.FeatureManagement.Flags;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;

namespace Demo.DotnetCore.FeatureManagement.Controllers;

/// <summary>
/// 示範 Feature Flag 的使用
/// </summary>
[ApiController]
[Route("[controller]")]
public class DemoController : ControllerBase
{
    private readonly ILogger<DemoController> _logger;
    private readonly IFeatureManager _featureManager;

    public DemoController(
        ILogger<DemoController> logger, 
        IFeatureManager featureManager)
    {
        _logger = logger;
        _featureManager = featureManager;
    }
    
    // Feature Flag 可以讓我們從設定檔中讀取特定的開關，並且在程式中根據這些開關來決定是否啟用某些功能
    // see: https://docs.microsoft.com/zh-tw/azure/azure-app-configuration/use-feature-flags-dotnet-core
    // see: https://learn.microsoft.com/zh-tw/dotnet/architecture/cloud-native/feature-flags

    [HttpGet("IsEnabledAsync")]
    public async Task<string> IsFeatureEnabled()
    {
        // Demo 1: 實作：使用 FeatureManagement 拿到 Feature Flag 的值 
        // IsEnabledAsync 會即時讀取設定檔內容
        // 示範這一段的時候，可以先呼叫一次這個方法，再去修改 appsettings.json 的內容（不用停止服務），明確地看到變化
        var isEnabled = await _featureManager.IsEnabledAsync(FeatureFlags.NewLogicEnabled);
        
        return isEnabled 
            ? "香香的新版邏輯已啟用。"
            : "新版邏輯尚未啟用，繼續使用臭臭的舊版邏輯。";
    }
    
    [HttpGet("FeatureGate")]
    [FeatureGate(FeatureFlags.NiceApiEnabled)]
    public string FeatureGate()
    {
        // Demo 2: 實作：使用 FeatureGate 來啟用和停用 API
        // 如果 FeatureB 是關閉的，這個方法不會被執行，會直接回傳 404
        // 示範這一段的時候，同樣也可以先呼叫一次這個方法，再去修改 appsettings.json
        return "這是一個很棒的 API，只有你看得到。";
    }

    [HttpGet("FeatureFilter/Random/")]
    public async Task<string> FeatureFilterRandom()
    {
        // Demo 3: 實作：使用 IFeatureFilter 來隨機啟用 API - 直接查詢 Feature Flag
        var isEnabled = await _featureManager.IsEnabledAsync(FeatureFlags.RandomTrial);
        return isEnabled 
            ? "恭喜你！你抽中了香香新版邏輯的試用機會！"
            : "什麼事也沒發生，又是臭臭邏輯的一天。";
    }  
    
    [HttpGet("FeatureFilter/Random/FeatureGate")]
    [FeatureGate(FeatureFlags.RandomTrial)]
    public string FeatureFilterRandomWithFeatureGate()
    {
        // Demo 3: 實作：使用 IFeatureFilter 來隨機啟用 API - 透過 FeatureGate 來啟用
        return "恭喜你！你抽中了很棒 API 的試用機會！";
    }
    
    [HttpGet("FeatureFilter/Whitelist/")]
    public async Task<string> FeatureFilterWhitelist(
        [FromHeader(Name = "User")] string user)
    {
        // Demo 4: 實作：使用 IFeatureFilter 來根據白名單啟用 API
        var isEnabled = await _featureManager.IsEnabledAsync(FeatureFlags.Whitelist);
        
        return isEnabled 
            ? $"{user} 先生/小姐，你好。歡迎使用本服務。您是我們第 9999 位 VIP 會員！"
            : $"{user} 先生/小姐，你好。很抱歉，本服務尚未開放給您使用。請加入我們的 VIP 會員，取得優先體驗資格！";
    }
}