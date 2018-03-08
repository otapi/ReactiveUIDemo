﻿using ReactiveUIDemo.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace ReactiveUIDemo
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();

            //MainPage = new Views.MainMasterDetailPage();
            var bootstrapper = new AppBootsrapper();
            MainPage = bootstrapper.CreateMainPage();
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
