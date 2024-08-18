namespace Demo.DotnetCore.FeatureManagement.Flags;

public class FeatureFlags
{
    // 這邊的名稱要和 appsettings.json 裡 FeatureManagement 底下的設定一樣
    public const string NewLogicEnabled = "NewLogicEnabled"; 
    public const string NiceApiEnabled = "NiceApiEnabled";
    public const string RandomTrial = "RandomTrial";
    public const string Whitelist = "Whitelist";
}