using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;

namespace SearchPeople.TestPlugins.ViewModels
{

    public class Item
    {
        public ImageSource Image { get; set; }
    }
    public class CoverFlowSampleXamlViewPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        public ObservableCollection<Item> Items { get; set; } = new ObservableCollection<Item>();

        public CoverFlowSampleXamlViewPageViewModel()
        {
            Items.Add(new Item()
            {
                Image = "Bolls.png"
            });


            Items.Add(new Item()
            {
                Image = "Bolls.png"
            });


            Items.Add(new Item()
            {
                Image = "Bolls.png"
            });


            Items.Add(new Item()
            {
                Image = "Bolls.png"
            });


            Items.Add(new Item()
            {
                Image = "Bolls.png"
            });
        }
    }
}
