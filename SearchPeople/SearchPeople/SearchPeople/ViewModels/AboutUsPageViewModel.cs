using Prism.Navigation;

namespace SearchPeople.ViewModels
{
    public class AboutUsPageViewModel : IInitialize
    {
        public static string AboutUsText { get; set; } = "\nApp hecha por Emy Saul y Gabriel.\nSearch" +
            " People es una app que permite buscar la cara de una o varias personas dentro de " +
            "una carpeta que contenga fotografías";
        public void Initialize(INavigationParameters parameters)
        {
        }
    }
}
