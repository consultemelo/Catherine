using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatherineDesktopApp.Shared
{
    public class Constants
    {
        public const int
            ModelShortCodeMaxLength = 15,
            ModelLongCodeMaxLength = 30,
            ModelShortNameMaxLength = 50,
            ModelLongNameMaxLength = 100,
            ModelShortDescriptionMaxLength = 255,
            ModelLongDescriptionMaxLength = 2048;

        public const string
            AppEventLogSource = "CatherineDesktopApp",
            AppEventLogName = "Application",
            AppDataSubFolderName = $"CatherineDesktopApp",
            AppSeedUserName = "application_seed";

        public const int
            PriestFirstNamesMinLengthAttribute = 2,
            PriestFirstNamesMaxLengthAttribute = 500,
            PriestLastNamesMinLengthAttribute = 2,
            PriestLastNamesMaxLengthAttribute = 500;

        public const int
            UserFirstNameMinLengthAttribute = 3,
            UserFirstNameMaxLengthAttribute = 90,
            UserUserNameMinLengthAttribute = 3,
            UserUserNameMaxLengthAttribute = 16,
            UserLastNameMinLengthAttribute = 3,
            UserLastNameMaxLengthAttribute = 90,
            UserPasswordMinLengthAttribute = 3,
            UserPasswordMaxLengthAttribute = 32;
    }

    public class ResourceFiles
    {
        public const string
            Controls = "Controls",
            ErrorMessages = "ErrorMessages",
            ManifestStrings = "ManifestStrings",
            EnumDisplays = "EnumDisplays";
    }
}