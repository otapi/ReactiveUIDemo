﻿using ReactiveUI;
using ReactiveUIDemo.Services;
using ReactiveUIDemo.ViewModel;
using Splat;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using ReactiveUI.XamForms;
using ReactiveUIDemo.Views;
using System.ComponentModel;

namespace ReactiveUIDemo
{
    public class AppBootsrapper : ReactiveObject, IScreen
    {
        public RoutingState Router { get; protected set; }

        public AppBootsrapper()
        {
            Router = new RoutingState();

            ///You much register This as IScreen to represent your app's main screen
            Locator.CurrentMutable.RegisterConstant(this, typeof(IScreen));

            //We register the service in the locator
            Locator.CurrentMutable.Register(() => new LoginService(), typeof(ILogin));

            //Register the views 
            Locator.CurrentMutable.Register(() => new LoginPage(), typeof(IViewFor<LoginViewModel>));
            Locator.CurrentMutable.Register(() => new ItemsPage(), typeof(IViewFor<ItemsViewModel>));

            // Enable Framework Logging
            #if DEBUG
            Locator.CurrentMutable.RegisterConstant(new LoggingService { Level = LogLevel.Debug }, typeof(ILogger));
            #endif

            this
                .Router
                .NavigateAndReset
                .Execute(new LoginViewModel(Locator.Current.GetService<ILogin>()))
                .Subscribe();
            
        }

        public Page CreateMainPage()
        {
            // NB: This returns the opening page that the platform-specific
            // boilerplate code will look for. It will know to find us because
            // we've registered our AppBootstrappScreen.
            return new ReactiveUI.XamForms.RoutedViewHost();
        }

        public class LoggingService : ILogger
        {
            public LogLevel Level { get; set; }
                        
            public void Write([System.ComponentModel.Localizable(false)] string message, LogLevel logLevel)
            {
                if (logLevel >= Level)
                    System.Diagnostics.Debug.WriteLine(message);
            }

            public void Write([System.ComponentModel.Localizable(false)] string message, [System.ComponentModel.Localizable(false)] Type type, LogLevel logLevel)
            {
                if (logLevel >= Level)
                    System.Diagnostics.Debug.WriteLine(message);
            }

            public void Write(Exception exception, [System.ComponentModel.Localizable(false)] string message, [System.ComponentModel.Localizable(false)] Type type, LogLevel logLevel)
            {
                if (logLevel >= Level)
                    System.Diagnostics.Debug.WriteLine(message);
            }

            public void Write(Exception exception, [System.ComponentModel.Localizable(false)] string message, LogLevel logLevel)
            {
                if (logLevel >= Level)
                    System.Diagnostics.Debug.WriteLine(message);
            }
        }
    }
}
