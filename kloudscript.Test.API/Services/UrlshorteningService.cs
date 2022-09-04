using kloudscript.Test.API.Entity;
using kloudscript.Test.API.Utility;
using Microsoft.Extensions.Caching.Memory;

namespace kloudscript.Test.API.Services
{
    public interface IUrlshorteningService
    {
        Task<string> GenerateShortUrl(string? url, int slideExpiry, int absExpiry);
        Task<string> RedirectToUrl(string shortUrl);
    }
    public class UrlshorteningService : IUrlshorteningService
    {
        private const string urlCacheKey = "UrlStorage";

        IMemeoryConfigService memeoryConfig;
        public UrlshorteningService(IMemeoryConfigService _memeoryConfig)
        {
            memeoryConfig = _memeoryConfig;
        }
        public async Task<string> GenerateShortUrl(string? url,int slideExpiry,int absExpiry)
        {
            string shortenUrl = string.Empty;
            await Task.Run(async () =>
                    {
                        shortenUrl = CreateShortenUrl.GetURL();
                        Dictionary<string, string>? urlList = await memeoryConfig.GetObjectFromMemory(urlCacheKey) as Dictionary<string, string>;
                        if (urlList == null)
                        {
                            urlList = new Dictionary<string, string>();
                        }
                        else
                        {
                            var urlData = urlList.Where(x => x.Key == url).FirstOrDefault();
                            if (string.IsNullOrEmpty(urlData.Value) == false)
                            {
                                urlList.Remove(urlData.Key);
                            }
                        }
                        urlList.Add(url, shortenUrl);
                        await memeoryConfig.SetObjectInMemroy(urlCacheKey, urlList, slideExpiry, absExpiry);
                    });
            return shortenUrl;
        }

        public async Task<string> RedirectToUrl(string shortUrl)
        {
            string returnUrl = string.Empty;
            await Task.Run(async () =>
            {
                Dictionary<string, string>? urlList =await  memeoryConfig.GetObjectFromMemory(urlCacheKey) as Dictionary<string, string>;
                if (urlList != null)
                {
                    var urlKey = urlList.Where(x => x.Value == shortUrl).FirstOrDefault(); 
                    returnUrl = urlKey.Key; 
                }
                else
                {
                    returnUrl = "Url not found";
                }
            });
            return returnUrl;
        }
    }
}

