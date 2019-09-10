using ReactiveUI;
using ReactiveUI.XamForms;
using ReactiveUIDemo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReactiveUIDemo.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ReactiveContentPage<LoginViewModel>
	{
		public LoginPage ()
		{
			InitializeComponent ();

            // TODO: Disposal of bindings? as per https://reactiveui.net/docs/handbook/data-binding/xamarin-forms
        }

        
    }
}