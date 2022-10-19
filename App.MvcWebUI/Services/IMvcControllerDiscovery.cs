using App.MvcWebUI.Models;

namespace App.MvcWebUI.Services
{
    public interface IMvcControllerDiscovery
    {
        IEnumerable<MvcControllerInfo> GetControllers();
    }
}
