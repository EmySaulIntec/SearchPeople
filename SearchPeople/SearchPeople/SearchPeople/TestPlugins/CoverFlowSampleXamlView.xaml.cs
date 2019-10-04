using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SearchPeople.TestPlugins
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CoverFlowSampleXamlView : ContentPage
    {
        public CoverFlowSampleXamlView()
        {
            InitializeComponent();
        }
    }
}