using Shelly_UI.Models;

namespace Shelly_UI.Services;

public interface IConfigService
{
    void SaveConfig(ShellyConfig config);
    ShellyConfig LoadConfig();
}
