using ReactiveUI;
using ReactiveUIDemo.Services;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ReactiveUIDemo.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private string _UserName;
        public string UserName
        {
            get => _UserName;
            //Notify when property user name changes
            set => this.RaiseAndSetIfChanged(ref _UserName, value);
        }

        private string _Password;
        public string Password
        {
            get => _Password;
            set => this.RaiseAndSetIfChanged(ref _Password, value);
        }

        /// <summary>
        /// This is an Oaph Observable propperty helper, 
        /// Which is used to determine whether a subsequent action
        /// Could be performed or not depending on its value
        /// This condition is calculated every time its value changes.
        /// </summary>
        ObservableAsPropertyHelper<bool> _ValidLogin;
        public bool ValidLogin
        {
            get { return _ValidLogin?.Value ?? false; }
        }
        
        
        public ReactiveCommand<Unit, Unit> PerformLogin { get; private set; }
        
        public LoginViewModel(ILogin login, IScreen hostScreen = null) : base(hostScreen)
        {
            PerformLogin = ReactiveCommand.CreateFromObservable(() => Observable.StartAsync(async () =>
            {
                var lg = await login.Login(_UserName, _Password);
                if (lg && ValidLogin)
                {
                    HostScreen.Router
                            .Navigate
                            .Execute(new ItemsViewModel())
                            .Subscribe();
                }
            }));

            this.WhenAnyValue(x => x.UserName, x => x.Password,
                (email, password) =>
                (
                    ///Validate the password
                    !string.IsNullOrEmpty(password) && password.Length > 5
                )
                &&
                (
                    ///Validate teh email.
                    !string.IsNullOrEmpty(email)
                            &&
                     Regex.Matches(email, "^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$").Count == 1
                ))
                .ToProperty(this, v => v.ValidLogin, out _ValidLogin);

 
           
        }

    }
}
