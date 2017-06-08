using Android.App;
using Android.Widget;
using Android.OS;
using SALLab07;

namespace Lab07
{
    [Activity(Label = "Lab 07", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        EditText EmailText;
        EditText PasswordText;
        Button ValidateButton;
        TextView MessageText;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            EmailText = FindViewById<EditText>(Resource.Id.EmailText);
            PasswordText = FindViewById<EditText>(Resource.Id.PasswordText);
            ValidateButton = FindViewById<Button>(Resource.Id.ValidateButton);
            MessageText = FindViewById<TextView>(Resource.Id.MessageText);

            ValidateButton.Click += ValidateButton_Click;
        }

        private async void ValidateButton_Click(object sender, System.EventArgs e)
        {
            var serviceClient = new ServiceClient();
            var studentEmail = EmailText.Text;
            var password = PasswordText.Text;
            var myDevice = Android.Provider.Settings.Secure.GetString(ContentResolver, Android.Provider.Settings.Secure.AndroidId);
            var result = await serviceClient.ValidateAsync(studentEmail, password, myDevice);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                var builder = new Notification.Builder(this)
                    .SetContentTitle("Validación de la actividad")
                    .SetContentText($"{result.Status}\n{result.Fullname}\n{result.Token}")
                    .SetSmallIcon(Resource.Drawable.Icon);
                builder.SetCategory(Notification.CategoryMessage);
                var notification = builder.Build();
                var manager = GetSystemService(NotificationService) as NotificationManager;
                manager.Notify(0, notification);
            }
            else
            {
                //var builder = new AlertDialog.Builder(this);
                //var alert = builder.Create();
                //alert.SetTitle("Resultado de la verificación");
                //alert.SetIcon(Resource.Drawable.Icon);
                //alert.SetMessage($"{result.Status}\n{result.Fullname}\n{result.Token}");
                //alert.SetButton("Ok", (s, ev) => { });
                //alert.Show();
                MessageText.Text = $"{result.Status}\n{result.Fullname}\n{result.Token}";
            }
        }
    }
}