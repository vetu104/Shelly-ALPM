using System.Collections.Generic;
using System.Threading.Tasks;
using PackageManager.Aur.Models;

namespace PackageManager.Aur;

public interface IAurPackageManager
{
    void Intialize(bool root = false);

    Task<List<AurPackageDto>> GetInstalledPackages();
    Task<List<AurPackageDto>> SearchPackages(string query);
    
    Task<List<AurPackageDto>> GetPackagesNeedingUpdate(string packageName);
}