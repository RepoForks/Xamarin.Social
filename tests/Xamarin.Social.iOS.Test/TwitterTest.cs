using System;
using NUnit.Framework;
using MonoTouch.UIKit;
using MonoTouch.Accounts;
using Xamarin.Social.Services;
using System.Linq;

namespace Xamarin.Social.iOS.Test
{
	[TestFixture]
	public class TwitterTest
	{
		TwitterService CreateService ()
		{
			return new TwitterService () {
				ConsumerKey = "GsileCDN9PqjHNoIKUfGQ",
				ConsumerSecret = "g7gAeVQPJzTC4zwS0ftVPWsv3brVQVgfyR47gD03lk",
			};
		}

		[Test]
		public void Manual_Authenticate ()
		{
			var service = CreateService ();
			var vc = service.GetAuthenticateUI (account => {
				Console.WriteLine ("AUTHENTICATE RESULT = " + account);
				AppDelegate.Shared.RootViewController.DismissModalViewControllerAnimated (true);
			});
			AppDelegate.Shared.RootViewController.PresentViewController (vc, true, null);
		}

		[Test]
		public void Manual_ShareText ()
		{
			var service = CreateService ();
			
			var item = new Item {
				Text = "This is just a test. Don't mind me...",
			};

			var vc = service.GetShareUI (item, result => {
				Console.WriteLine ("SHARE RESULT = " + result);
				item.Dispose ();
				AppDelegate.Shared.RootViewController.DismissModalViewControllerAnimated (true);
			});
			AppDelegate.Shared.RootViewController.PresentViewController (vc, true, null);
		}

		[Test]
		public void Manual_ShareTextLink ()
		{
			var service = CreateService ();
			
			var item = new Item {
				Text = "This is just a test. Don't mind me...",
			};
			item.Links.Add (new Uri ("http://docs.xamarin.com/ios/getting_started/intro_to_mvc_in_ios"));

			var vc = service.GetShareUI (item, result => {
				Console.WriteLine ("SHARE RESULT = " + result);
				item.Dispose ();
				AppDelegate.Shared.RootViewController.DismissModalViewControllerAnimated (true);
			});
			AppDelegate.Shared.RootViewController.PresentViewController (vc, true, null);
		}
		
		[Test]
		public void Manual_ShareImagesTextLinks ()
		{
			var service = CreateService ();
			
			var item = new Item {
				Text = "I wonder if images and links will work?",
			};
			item.Images.Add ("Images/xamarin-logo.png");
			item.Images.Add ("Images/what_does_that_mean_trollcat.jpg");
			item.Links.Add (new Uri ("http://xamarin.com"));
			item.Links.Add (new Uri ("https://twitter.com/xamarinhq"));
			
			var vc = service.GetShareUI (item, result => {
				Console.WriteLine ("SHARE RESULT = " + result);
				item.Dispose ();
				AppDelegate.Shared.RootViewController.DismissModalViewControllerAnimated (true);
			});
			AppDelegate.Shared.RootViewController.PresentViewController (vc, true, null);
		}
		
		[Test]
		public void HomeTimeline ()
		{
			var service = CreateService ();
			
			var account = service.GetAccountsAsync ().Result.First ();
			
			var req = service.CreateRequest ("GET", new Uri ("http://api.twitter.com/1/statuses/home_timeline.xml"), account);

			var content = req.GetResponseAsync ().Result.GetResponseText ();
			
			Assert.That (content.Contains ("statuses"));
		}
	}
}
