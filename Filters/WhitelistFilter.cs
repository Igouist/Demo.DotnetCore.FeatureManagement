using System.Collections.Immutable;
using Demo.DotnetCore.FeatureManagement.Repositories;
using Microsoft.FeatureManagement;

namespace Demo.DotnetCore.FeatureManagement.Filters;

/// <summary>
/// 白名單啟用 API 的 Filter
/// </summary>
[FilterAlias("Whitelist")]
public class WhitelistFilter : IFeatureFilter
{
    private readonly IWhitelistRepository _whitelistRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WhitelistFilter(
        IWhitelistRepository whitelistRepository, 
        IHttpContextAccessor httpContextAccessor)
    {
        _whitelistRepository = whitelistRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
    {
        // 實作時記得換成真正的使用者資訊來源，例如 JWT Token or 拆好的 User 資訊
        var user = _httpContextAccessor.HttpContext?.Request.Headers["User"]; 
        if (string.IsNullOrWhiteSpace(user))
        {
            return false;
        }
        
        // 檢查使用者是否在白名單中，這邊假裝從資料庫中取得白名單
        // 實作時記得換成真正的白名單來源 & 比對邏輯
        var whitelist = await _whitelistRepository.GetWhitelistAsync();
        return whitelist.Any(x => x.Equals(user, StringComparison.OrdinalIgnoreCase)); // 不分大小寫
    }
}