using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SearchPeople
{
    static class config
    {
        public static string ApiUrl = "https://eastus2.api.cognitive.microsoft.com/face/v1.0";
        public const string NameAzureKey = "Ocp-Apim-Subscription-Key";

        public static string ApiHostName
        {
            get
            {
                var apiHostName = Regex.Replace(ApiUrl, @"^(?:http(?:s)?://)?(?:www(?:[0-9]+)?\.)?",
                    string.Empty, RegexOptions.IgnoreCase).Replace("/", string.Empty);

                return apiHostName;
            }
        }
        public static string APIKey { get; set; } = "";
    }
}
