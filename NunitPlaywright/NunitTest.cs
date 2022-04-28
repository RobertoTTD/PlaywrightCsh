using System.Threading.Tasks;
using Microsoft.Playwright;
//using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace NunitPlaywright;

public class NunitTest// : PageTest
{
    private string url = "https://dora-test.explorelearning.cloud/";// "https://localhost:62527/";// TestContext.Parameters["WelcomeUrl"];
    private static string AccountTextSelector => "id=i0116";
    private static string PasswordTextSelector => "id=i0118";
    private static string NextButtonSelector => "id=idSIButton9";
    private static string CentreSelector => "a:has-text(\"Aberdeen\")";// "id=centres_header";
    private IBrowser? browser;
    private IBrowserContext? context;
    private IPage? page ;


    [SetUp]
    public async Task SetupAsync()
    {
        await Init();
        await EnsurePageIsOpenAsync();
        await Login();
    }

    [Test]
    public async Task AberdeenCentreExists()
    {  
        await ClickCentresAsync();
        var centres = page.Locator("text=Aberdeen");
        await centres.ClickAsync();
        var text = centres.InnerTextAsync().Result;
        Assert.That(text.Contains("Aberdeen"));
    }

    private async Task Init()
    {
        //var playwright = await Playwright.CreateAsync();
        browser = await (await Playwright.CreateAsync()).Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false
        });
        context = await browser.NewContextAsync();
        page = await context.NewPageAsync();
    }

    public async Task EnsurePageIsOpenAsync()
    {
        await page.GotoAsync(url);
        await page.WaitForLoadStateAsync();
    }

    public async Task Login()
    {
        await EnterAccountAsync();
        await page.ClickAsync(NextButtonSelector);
        await EnterPasswordAsync();
        await page.ClickAsync(NextButtonSelector);
        await page.ClickAsync(NextButtonSelector);
        await page.WaitForLoadStateAsync();
    }

    public async Task EnterAccountAsync()
    {
        // This step will fail unless the user is excluded from 2FA
        await page.FillAsync(AccountTextSelector, "talkthinkdotest5@explorelearning.co.uk");
    }

    public async Task EnterPasswordAsync()
    {
        await page.FillAsync(PasswordTextSelector, "TTDT3st1ng!");
    }

    public async Task ClickCentresAsync()
    {
        //await Page.ClickAsync(CentreSelector);
        //await (await this._page).HoverAsync(CentreSelector);
        await page.ClickAsync(CentreSelector);
        ////var bar = (await this._page).Locator(HeaderSelector);
        //var centres = await (await this._page).QuerySelectorAsync(CentreSelector);
        //await centres.ClickAsync();
    }
}