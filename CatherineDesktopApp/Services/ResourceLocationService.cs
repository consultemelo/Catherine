using CatherineDesktopApp.Shared.Exceptions;
using CatherineDesktopApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Windows.ApplicationModel.Resources;

namespace CatherineDesktopApp.Services
{
    public interface IResourceLocationService
    {
        string GetErrorMessageString(string key);
        string GetControlString(string key);
        string GetEnumDisplayString(string key);
    }

    public class ResourceLocationService : IResourceLocationService
    {
        private readonly ResourceLoader _errorMessageLoader;
        private readonly ResourceLoader _controlLoader;
        private readonly ResourceLoader _enumDisplayLoader;

        public ResourceLocationService()
        {
            _errorMessageLoader = ResourceLoader.GetForViewIndependentUse(ResourceFiles.ErrorMessages);
            _controlLoader = ResourceLoader.GetForViewIndependentUse(ResourceFiles.Controls);
            _enumDisplayLoader = ResourceLoader.GetForViewIndependentUse(ResourceFiles.EnumDisplays);
        }

        public string GetErrorMessageString(string key)
        {
            return GetMapValueForKey(key, ResourceFiles.ErrorMessages);
        }

        public string GetControlString(string key)
        {
            return GetMapValueForKey(key, ResourceFiles.Controls);
        }

        public string GetEnumDisplayString(string key)
        {
            return GetMapValueForKey(key, ResourceFiles.EnumDisplays);
        }

        private string GetMapValueForKey(string key, string map)
        {
            if (key == null || key.Length < 1)
                throw new ArgumentNullException(nameof(key), $"Key:{key}");

            string result;

            switch (map)
            {
                case ResourceFiles.Controls:
                    result = _controlLoader.GetString(key);
                    break;
                case ResourceFiles.ErrorMessages:
                    result = _errorMessageLoader.GetString(key);
                    break;
                case ResourceFiles.EnumDisplays:
                    result = _enumDisplayLoader.GetString(key);
                    break;
                default:
                    throw new Exception("ResourceMapNotFound");
            }

            if (result == null || result == string.Empty)
                throw new ResourceKeyNotFoundException($"Key:{map}/{key}");

            return result;
        }
    }

}