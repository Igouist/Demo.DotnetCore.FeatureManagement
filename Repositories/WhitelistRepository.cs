namespace Demo.DotnetCore.FeatureManagement.Repositories;

public interface IWhitelistRepository
{
    Task<IEnumerable<string>> GetWhitelistAsync();
}

public class WhitelistRepository : IWhitelistRepository
{
    public async Task<IEnumerable<string>> GetWhitelistAsync()
    {
        // 假裝從資料庫中取得白名單 :)
        return new List<string>
        {
            "Apple",
            "Banana",
            "Cat"
        };
        // ps: 如果真的連線到資料庫，然後名單的改動頻率很低，可以考慮加上快取
    }
}