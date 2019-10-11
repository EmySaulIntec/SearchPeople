using System.Text.RegularExpressions;

namespace SearchPeople
{
    static class Config
    {
        public static string ApiUrl = "https://recognitionservice.cognitiveservices.azure.com/face/v1.0";
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
        public static string APIKey { get; set; } = "d0d158ee6f634157b5d19abd74096864";
    }
}
