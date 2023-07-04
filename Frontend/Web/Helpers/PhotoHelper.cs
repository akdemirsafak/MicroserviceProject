using Microsoft.Extensions.Options;
using Web.Services;

namespace Web.Helpers;

public class PhotoHelper
{
    private readonly ServiceApiSettings _serviceApiSettings;

    public PhotoHelper(IOptions<ServiceApiSettings> settings)
    {
        _serviceApiSettings = settings.Value;
    }

    public string GetPhotoStockUrl(string photoUrl)
    {
        return $"{_serviceApiSettings.PhotoStockUri}/photos/{photoUrl}";
    }
}
