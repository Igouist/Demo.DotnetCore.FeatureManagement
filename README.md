FeatureManagement & Feature Flag 的測試用專案

> 筆記內容同步發布至 Blog 好讀版：[.Net: 使用 FeatureManagement 套件來實作 Feature Flag 功能切換吧 - 伊果的沒人看筆記本](https://igouist.github.io/post/2024/08/dotnet-feature-flag-and-feature-management/)

---

![Image](https://i.imgur.com/926KRip.png)

在做上一篇 [IOptions](https://igouist.github.io/post/2024/08/dotnet-ioptions) 的筆記時，剛好看到 FeatureManagement 這香東西。<br/>馬上來收錄一篇。順便也簡單整理一下 Feature Flag (= Feature Toggle) 的介紹。

## 認識一下 Feature Flag

> 本部落格秉持著「簡單、友善、我好菜」的精神，按照慣例先簡單介紹一下<br/>
> 已經知道的朋友就可以跳過這個小節，直接前往 #環境準備 囉。

假設我們原本有 Old 邏輯：

```csharp
Old();
```

天庭傳來諭令，要我們改成 New 邏輯。這簡單，我們就把 Old 砍掉，換成 New。非常自然，改完就佈版

```csharp
New();
```

隔天，天庭又傳來諭令，New 需要調整一下，先不要了現在我們又需要把 New 邏輯砍掉，讓 Old 邏輯回來。簡單，但看來我們得再上一版

```csharp
Old();
// New();
```

再隔天，大家可能猜到天庭又要幹嘛了，總之又上了一版

```csharp
// Old();
New();
```

如此往復三萬八千次，工程師終於受不了了：「俺老孫每天在這切換 Old 跟 New，改完還得佈版，每天搞這些就飽了，我滴媽呀，不幹了」

就在老孫關燈走人的那一瞬間，突然靈光一現：等等，**俺加個開關還不行嗎？**

<!--more-->

```csharp
if (NewFunctionEnabled())
{
    New();
}
else
{
    Old();
}
```

現在只要把這個開關放到外面，別寫死在程式碼裡。丟去什麼 appsettings.json、Config、資料庫、ini、東海龍王廟，隨便

總之丟去個不用佈板就能直接修改的地方，不就完事了嗎？

現在程式碼就像盯著旗手的士兵們，旗手舉了藍色，他們就往左跑；旗手舉了紅色，他們就往右跑。老孫只要把設定給改了，就能一鍵切換邏輯，好不自在。

**沒錯，老孫剛剛發現了 Feature Flag**。

有了這個開關，除了開開關關(?)，現在也可以做到更多事情了。

例如新功能想先隨機讓一些使用者踩看看，馬上在這個 `NewFunctionEnabled()` 開關後面加點料，讓它隨機打開、隨機關起來；

想讓合作好的客戶搶先使用，馬上在這個 `NewFunctionEnabled()` 開關後面偷塞一張白名單：<br/>你聽好了舉旗仔，看到這些人就幫我舉紅的旗子，門就會打開

因為我們把開關抽出去了，代表判斷條件和執行邏輯之間被我們降低耦合了，彈性就變高了，我們也就可以針對開關內部的邏輯動手動腳了

一時之間，花果山分部的工程師紛紛效仿老孫，<br/>用這個**香香的 Feature Flag 來從外部動態切換程式碼邏輯**，好不爽快。
<br/>吱聲此起彼落，可謂是：

> 兩條邏輯改又改，改完還要等佈版<br/>
> 有了 Feature Flag，切個開關就下班

至於後來開關越埋越多，系統服務七十二變。工程師老孫被迫跟著唐經理去聽研討會，好好學習怎麼管理這些 Feature Flag。還弄了些儀表板、過期時間等等，那又是另一個故事了…

---

故事說到這裡，雖然例子粗暴一點，但朋朋們應該對 Feature Flag 在幹嘛有點了解了（吧？）

如果想要進一步認識 Feature Flag，這邊也準備了一些推薦閱讀。

例如 Feature Flag 還可以用在哪、分成哪些種類；<br/>
兩個 Flag 共用到同一段 Code 怎辦；<br/>
PM 朋朋們又是怎麼運用 Feature Flag 的……

都丟在下面這團，提供給有興趣的朋朋們：

- [Feature Toggles (aka Feature Flags) - martinFowler](https://martinfowler.com/articles/feature-toggles.html)
- [Feature Toggle 應用常見問題 | Miles' Blog](https://mileschou.me/blog/feature-toggle-faq/)
- [產品上線規劃：Beta Release、Feature Flags 實作經驗與工具分享 | by Anne Hsiao | 3PM LAB 產品三眼怪實驗室 | Medium](https://medium.com/3pm-lab/product-beta-release-planning-and-feature-flags-implementation-1f7673007d23)
- [Introduction to Feature Toggles and Implementation Best Practices - Darren Sim](https://darrensim.com/2018/introduction-to-feature-toggles-and-implementation-best-practices/)

---

> 補充：雖然 Feature Flag 這麼香，但如果開關沒有好好管理，<br/>不小心開到不該開的程式碼邏輯會發生什麼事呢？
>
> 泡杯茶，看看實例吧：[Knightmare: A DevOps Cautionary Tale – Doug Seven](https://dougseven.com/2014/04/17/knightmare-a-devops-cautionary-tale/)

---

> 補充：Feature Toggle 常常跟主幹開發（Trunk-based Development）一起出現？
> 
> 蠻常聽到這個問題的，這邊也簡單筆記一下，沒有採用主幹開發的朋朋可以跳過這小段
> 
> （想認識 Trunk-based Development 的朋朋們，<br/>　可以右轉去 [TrunkBasedDevelopment.com](https://tw.trunkbaseddevelopment.com/)，感謝協助中文化的大大們）
> 
> 回到問題，我的理解是這樣：
> 
> 即使我們採用了主幹開發，持續把變更交付到主幹分支上，把大家的 GAP 降到最低，<br/>但一定還是會遇到需要做很久才做得完的功能
> 
> 我們不想要拉一條 feature 分支，放著漸漸就跟主幹脫節，到時候合併又要大爆炸<br/>
> 而是想要把這些程式碼持續地整合到我們的程式庫，這樣就能及早發現問題和解除衝突<br/>
> 可是如果粗暴地合併進去，開發中的功能就會暴露出來…怎麼辦呢？
> 
> 這種時候，如果有一個開關，來幫助我們關閉尚未完工的功能、打開即將交付的功能。我們就能整合現有的程式碼，也能提早知道合併會不會炸爛其他功能…
> 
> 很方便對吧？ Feature Toggle 就跳出來啦。香。

## 環境準備：Microsoft.FeatureManagement

好，我們前面簡單介紹了一下 Feature Flag，接著就進入實作階段。

要實現 Feature Flag 的方法有很多，例如我之前都是用[上一篇](https://igouist.github.io/post/2024/08/dotnet-ioptions)提到的 IOption 直接從 appsettings.json 把 true/false 讀出來用

而這篇我們要來使用香香的 FeatureManagement 來幫我們實現 Feature Flag 的操作。

首先，我們要先搞到 Microsoft.FeatureManagement.AspNetCore 這個套件：<br/>[NuGet Gallery | Microsoft.FeatureManagement.AspNetCore](https://www.nuget.org/packages/Microsoft.FeatureManagement.AspNetCore)

接著讓我們把它給註冊到我們的 Program.cs：

```csharp
// Program.cs

// 環境準備：Microsoft.FeatureManagement
// 加入 Feature Flag 功能，預設會抓 appsettings.json 中的 "FeatureManagement" 節點
builder.Services.AddFeatureManagement();

// 如果想要自訂 Feature Flag 在 appsettings.json 裡的位置，可以這樣寫
//builder.Services.AddFeatureManagement(builder.Configuration.GetSection("CustomFeatureManagement"));
```

然後為了後續的示範，先開好一組 Controller：
```csharp
// Controllers/DemoController.cs

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
}
```

這樣環境就搞定了，讓我們開始第一組實作吧！

## 實作：使用 IsEnabledAsync 拿到 Feature Flag 的值

假設我們跟上面的老孫一樣：翻寫了一條新版邏輯。<br/>如果開關被打開了，就走新版的路線；關著的話就維持舊版

要達成上面的條件，我們會**需要直接取得某個 Flag 的開關狀態，<br/>這時候就可以使用 `IsEnabledAsync`**

先到 appsettings.json 加上我們的 Feature Flag，先叫做 `NewLogicEnabled`：

```json
// appsettings.json

"FeatureManagement": {
  "NewLogicEnabled": true
}
```

![Image](https://i.imgur.com/FzIALd8.png)

為了後面方便使用，我習慣先建一組簡單的 Const 來管理這些 Feature Flag：

```csharp
// Flags/FeatureFlags.cs

public class FeatureFlags
{
    // 這邊的名稱要和 appsettings.json 裡 FeatureManagement 底下的設定一樣
    public const string NewLogicEnabled = "NewLogicEnabled"; 
}
```

讓我們加上這次測試用的 API，並且使用 IsEnabledAsync 來抓取指定的 Flag 狀態：
```csharp
// Controllers/DemoController.cs

// Feature Flag 可以讓我們從設定檔中讀取特定的開關
// 並且在程式中根據這些開關來決定是否啟用某些功能
[HttpGet("IsEnabledAsync")]
public async Task<string> IsFeatureEnabled()
{
    // Demo 1: 使用 IsEnabledAsync 拿到 Feature Flag 的值 
    var isEnabled = await _featureManager.IsEnabledAsync(FeatureFlags.NewLogicEnabled);
    
    return isEnabled 
        ? "香香的新版邏輯已啟用。"
        : "新版邏輯尚未啟用，繼續使用臭臭的舊版邏輯。";
}
```

![Image](https://i.imgur.com/O1uxn9W.png)

現在讓我們直接把 appsettings.json 裡的 `NewLogicEnabled` 改成 false（只要存檔就好了，不需要重啟服務），並且再呼叫一次 API：

![Image](https://i.imgur.com/WTgcapM.png)

來張動圖看看成果：

![Image](https://i.imgur.com/9elZfT7.gif)

搞定，收工下班。

## 實作：使用 FeatureGate 來啟用和停用 API

除了根據開關決定要執行哪條路線的邏輯以外，我們也常常根據開關決定某些功能是不是要開放

**如果我們需要根據 Feature Flag 來決定是否啟用某支 API，就可以使用 `FeatureGate`**

首先讓我們加上第二組 Flag，這次叫做 `NiceApiEnabled`：

```json
// appsettings.json

"FeatureManagement": {
  "NewLogicEnabled": false,
  "NiceApiEnabled": true
}
```

```csharp
// Flags/FeatureFlags.cs

public class FeatureFlags
{
    public const string NewLogicEnabled = "NewLogicEnabled"; 

      // 新增 NiceApiEnabled
    public const string NiceApiEnabled = "NiceApiEnabled";
}
```

接著就來替我們的 API 掛上 FeatureGate 試試：
```csharp
// Controllers/DemoController.cs

[HttpGet("FeatureGate")]
[FeatureGate(FeatureFlags.NiceApiEnabled)] // 讓 FeatureGate 去檢查我們的 NiceApiEnabled
public string FeatureGate()
{
    // Demo 2: 使用 FeatureGate 來啟用和停用 API
    // 如果 NiceApiEnabled 是關閉的，這個方法不會被執行，會直接回傳 404
    return "這是一個很棒的 API，只有你看得到。";
}
```

`NiceApiEnabled` 是 true 的時候，可以正常呼叫到 API：
![Image](https://i.imgur.com/2el7jVT.png)

而當 `NiceApiEnabled` 是 false 的時候，這支 API 就 不存在了…
![Image](https://i.imgur.com/BV3aomM.png)

## 實作：使用 FeatureFilter 自訂篩選條件、隨機抽取使用者

前面我們示範了怎麼看開關狀態，以及根據開關把 API 給 Ban 了。接著來點場景應用吧。

之前遇過需要隨機抽樣去分流的狀況（把 50% 使用者引導去翻寫好的新站台觀察狀況之類的）

**像這種需要自己寫某些邏輯來決定開關是否開啟的場合，就可以使用 FeatureFilter 來實現**

---

跟前兩個範例一樣，我們需要先去 `appsettings.json` 掛上我們的 Feature Flag。既然是要隨機抽取幸運兒體驗我們的新功能，就叫做 `RandomTrial`

但要注意，當我們要自訂 Flag 啟用的條件（也就是需要跑過某些邏輯才能決定是 true/false）的時候，就不能像前面兩個範例一樣簡單丟個 `"RandomTrial":true` 就搞定

而是要使用 `EnabledFor`，並在裡面標示出我們要呼叫的 FeatureFilter 名稱、需要傳遞的參數，這樣 FeatureManagement 才知道它得依序跑過這些 FeatureFilter

假設我們希望有 50% 的使用者會隨機抽中新功能。我們就可以先在 `EnabledFor` 裡掛一條叫做「Random」的 FeatureFilter，並且丟個 Percentage=50 的參數進去：
```json
// appsettings.json

"FeatureManagement": {
  "NewLogicEnabled": false,
  "NiceApiEnabled": false,
  
  "RandomTrial": {
    "EnabledFor": [
      {
        "Name": "Random",
        "Parameters": {
          "Percentage": 50
        }
      }
    ]
  }
}
```

```csharp
// Flags/FeatureFlags.cs

public class FeatureFlags
{
    public const string NewLogicEnabled = "NewLogicEnabled"; 
    public const string NiceApiEnabled = "NiceApiEnabled";

    // 新增 RandomTrial
    public const string RandomTrial = "RandomTrial";
}
```

接著讓我們實作對應的 FeatureFilter，這邊作法就可以粗暴一點：

- 從 `context.Parameters` 把前面設好的 Percentage 參數拉出來
- 現場骰下去看有沒有過門檻，沒過就掰掰

```csharp
// Filters/RandomFilter.cs

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
```

接著我們要把這個 FeatureFilter 拿去註冊：
```csharp
// Program.cs

builder.Services
    .AddFeatureManagement()
    .AddFeatureFilter<RandomFilter>() // 加入自訂的 Feature Filter. see: Filters/RandomFilter.cs
    ;
```

接著就像前面的兩個範例一樣，直接拿來用就好嚕
```csharp
// Controllers/DemoController.cs

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
```

來張動圖看看成果：

![Image](https://i.imgur.com/9l8N7EE.gif)

## 實作：使用 FeatureFilter 來根據白名單啟用 API

接下來試試另一組場景，假設我們打算先讓十位合作客戶試用新功能，<br/>這時候也可以靠自製 FeatureFilter 來處理。

> 備註：這一節會需要用到依賴注入相關的功能，還不熟的朋友可以參見[依賴注入的筆記](https://igouist.github.io/post/2021/11/newbie-6-dependency-injection/)

首先按照慣例，先加上我們的 Feature Flag<br/>
這次就叫做 `Whitelist`，並且只指定 FeatureFilter 的名稱就好，不需要參數：

```json
// appsettings.json

"FeatureManagement": {
  "NewLogicEnabled": false,
  "NiceApiEnabled": false,
  "RandomTrial": {}, // 好長，省略
  
  "Whitelist": {
    "EnabledFor": [
      {
        "Name": "Whitelist"
      }
    ]
  }
}
```
```csharp
// Flags/FeatureFlags.cs

public class FeatureFlags
{
    public const string NewLogicEnabled = "NewLogicEnabled"; 
    public const string NiceApiEnabled = "NiceApiEnabled";
    public const string RandomTrial = "RandomTrial";

    // 新增 Whitelist
    public const string Whitelist = "Whitelist";
}
```

接著，假裝一下我們在某張資料表裏面有白名單資料：
```csharp
// Repositories/WhitelistRepository.cs

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
        // 小提醒：如果真的連線到資料庫，然後名單的改動頻率很低，可以考慮加個快取
    }
}
```

而且這個 Repo 也有好好註冊到 Program.cs。<br/>
同時，因為後續的示範一定也需要從 Request 拿到使用者的身分<br/>
這邊也註冊了 IHttpContextAccessor 來取得 Request 內容：
```csharp
// Program.cs
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IWhitelistRepository, WhitelistRepository>();
```
> 延伸閱讀：[存取 ASP.NET Core 中的 HttpContext](https://learn.microsoft.com/zh-tw/aspnet/core/fundamentals/http-context?view=aspnetcore-8.0#access-httpcontext-from-custom-components) 的「從自訂元件存取 HttpContext」小節

> 備註：如果 FeatureFilter 依賴對象的生命週期是 Scope 的，會需要調整一下 FeatureManagement 的註冊，請看這一小節最後的補充。

上游的假資料準備好了，接著就讓我們來實作白名單 FeatureFilter 吧：
```csharp
// Filters/WhitelistFilter.cs

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
        // 粗暴地從 Request 的 Header 取出使用者名稱
        // 實作時記得換成真正的使用者資訊來源，例如 JWT Token、拆好的 User 資訊、擲筊的結果，之類的
        var user = _httpContextAccessor.HttpContext?.Request.Headers["User"]; 
        if (string.IsNullOrWhiteSpace(user))
        {
            return false;
        }
        
        // 假裝從資料庫中取得白名單，檢查使用者是否在白名單中
        // 實作時記得換成真正的白名單來源 & 比對邏輯
        var whitelist = await _whitelistRepository.GetWhitelistAsync();
        return whitelist.Any(x => x.Equals(user, StringComparison.OrdinalIgnoreCase)); // 插個不分大小寫，避免我等等 Demo 手殘
    }
}
```

搞定，開開心心來註冊囉：
```csharp
// Program.cs

builder.Services
    .AddFeatureManagement()
    .AddFeatureFilter<RandomFilter>()
    .AddFeatureFilter<WhitelistFilter>(); // 把白名單的 FeatureFilter 掛上去
```

接著捏支 API 來測測看：
```csharp

// Controllers/DemoController.cs

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
```

來張動圖看看成果：

![Image](https://i.imgur.com/r2gIrLw.gif)

搞定，收工。又是美好的一天，準時下班囉

> 補充：由於 FeatureManager 是 Singleton 的，而 AddFeatureFilter 會根據 FeatureManager 的生命週期來註冊 FeatureFilter，因此如果依賴對象的生命週期是 Scope，注入的時候就會發生錯誤。
> 
> 這時候請改用 AddScopedFeatureManagement：
>
> ```csharp
> // Program.cs
> 
> // builder.Services.AddFeatureManagement();
> builder.Services.AddScopedFeatureManagement();
> ```
> 
> 參考：[Unable to inject Scoped/Transient services into FeatureFilter · Issue #15 · microsoft/FeatureManagement-Dotnet](https://github.com/microsoft/FeatureManagement-Dotnet/issues/15)
> 
> 延伸閱讀：[依賴注入的三種生命週期 Transient、Scoped、Singleton](https://igouist.github.io/post/2021/11/newbie-6-dependency-injection/#%E4%BE%9D%E8%B3%B4%E6%B3%A8%E5%85%A5%E7%9A%84%E4%B8%89%E7%A8%AE%E7%94%9F%E5%91%BD%E9%80%B1%E6%9C%9F-transientscopedsingleton)

## 小結

稍微玩了一下 FeatureManagement，並且拿了一些前陣子遇過的場景來實作看看

原本以為只是簡單的去組態抓開關，想不到 FeatureFilter 提供的自訂條件還蠻彈性的，挺香

FeatureGate 無腦噴 404 也挺爽的（警告：請跟呼叫端的朋朋喬好= = 不然很危險，生命危險）

如果有想要搞個 Feature Flag 的場合，推薦可以玩玩看。

那麼，今天的筆記就到這邊。我們下篇見～

## 參考資料

- [如何在 .NET 應用程式中使用功能旗標的教學課程 | Microsoft Learn](https://learn.microsoft.com/zh-tw/azure/azure-app-configuration/use-feature-flags-dotnet-core)
- [[料理佳餚] ASP.NET Core 的 Feature Flags（Feature Toggle） | 軟體主廚的程式料理廚房](https://dotblogs.com.tw/supershowwei/2020/11/02/154134)
- 延伸閱讀（Azure App Configuration）：[使用 App Configuration 的 Feature manager 功能來實做 Feature Toggle/Flag 機制 | 黯雲端記事錄](https://dotblogs.com.tw/anyun/2022/10/16/160158)
