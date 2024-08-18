using Microsoft.FeatureManagement;

namespace Demo.DotnetCore.FeatureManagement.Filters;

/// <summary>
/// 隨機啟用 API 的 Filter
/// </summary>
/// <see ref="https://learn.microsoft.com/zh-tw/azure/azure-app-configuration/howto-feature-filters-aspnet-core"/>
[FilterAlias("Random")] // 設定 Filter 的別名，讓 appsettings.json 可以使用這個別名
public class RandomFilter : IFeatureFilter
{
    public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
    {
        var percentage = context.Parameters.GetValue<int>("Percentage");
        var seed = Guid.NewGuid().GetHashCode();
        var randomNumber = new Random(seed).Next(100);
        
        var isEnabled = randomNumber <= percentage;
        return Task.FromResult(isEnabled);
    }
}