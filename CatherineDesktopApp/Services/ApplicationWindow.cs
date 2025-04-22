using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatherineDesktopApp.Services
{
    public interface IApplicationWindow
    {
        XamlRoot XamlRoot { get; }
    }

    public class ApplicationWindow : IApplicationWindow
    {

        public XamlRoot XamlRoot
        {
            get
            {
                Window view = ((App)Application.Current).Window;
                return view.Content.XamlRoot;
            }
        }
    }
}
