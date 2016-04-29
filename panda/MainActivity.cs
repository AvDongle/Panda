using Android.App;
using Android.Widget;
using Android.OS;
using Android.Graphics;	
using System;
using Com.Getpebble.Android.Kit.Util;
using Com.Getpebble.Android.Kit;
using System.Text;
using Android.Content.PM;
using Android.Content;

namespace panda
{
	[Activity (Label = "Curve Panda", MainLauncher = true, Icon = "@mipmap/icon", ScreenOrientation = ScreenOrientation.Portrait)]
	public class MainActivity : Activity
	{
		int count = 1;
		Random rand = new Random();
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			//RequestedOrientation = ScreenOrientation.Portrait;
			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button> (Resource.Id.myButton);
			button.Text = "Click me!";
			PebbleDictionary dict = new PebbleDictionary ();
			button.Click += delegate {
				button.Text = string.Format ("{0} clicks!", count++);
				sendAlertToPebble(button.Text);
				RandomBG();
			};
		}
			
		protected override void OnResume()
		{
			base.OnResume ();
			StringBuilder builder = new StringBuilder ();
			builder.Append ("Pebble info\n\n");
			Boolean isConnected = PebbleKit.IsWatchConnected (this);
			builder.Append("Watch Connected: " +(isConnected ? "True" : "False"));

			// What is the firmware version?
			PebbleKit.FirmwareVersionInfo info = PebbleKit.GetWatchFWVersion(this);
			builder.Append("\nFirmware version: ");
			builder.Append(info.Major).Append(".");
			builder.Append(info.Minor).Append("\n");

			// Is AppMesage supported?
			Boolean appMessageSupported = PebbleKit.AreAppMessagesSupported(this);
			builder.Append("AppMessage supported: " + (appMessageSupported ? "true" : "false"));

			TextView textView = (TextView)FindViewById(Resource.Id.textView1);
			textView.Text = builder.ToString();
			//Toast.MakeText (this, "Pebble " + (isConnected ? "is" : "is not") + " connected!", ToastLength.Long).Show ();
		}

		private void sendAlertToPebble(System.String msgBody) 
		{
			var i = new Intent("com.getpebble.action.SEND_NOTIFICATION");
			string notificationData = @"[{""title"":""Test Message"",""body"":"""+msgBody+@"""}]";
			i.PutExtra("messageType", "PEBBLE_ALERT");
			i.PutExtra("sender", "FSHBT");
			i.PutExtra("notificationData", notificationData);
			//Log.Debug(TAG, "About to send a modal alert to Pebble: " + notificationData);
			SendBroadcast(i);
		}

		private void RandomBG()
		{
			RelativeLayout test = FindViewById<RelativeLayout>(Resource.Id.PhoneNumberText);			
			test.SetBackgroundColor(Color.Argb(255,
				rand.Next(0,255),
				rand.Next(0, 255),
				rand.Next(0, 255)));		
		}
	}
}


