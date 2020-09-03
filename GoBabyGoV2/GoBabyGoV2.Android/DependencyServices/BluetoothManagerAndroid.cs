using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using GoBabyGoV2.DependencyServices;
using GoBabyGoV2.Droid.DependencyServices;
using Java.Lang;
using Plugin.CurrentActivity;
using Xamarin.Forms;

[assembly: Dependency(typeof(BluetoothManagerAndroid))]

namespace GoBabyGoV2.Droid.DependencyServices
{
    public class BluetoothManagerAndroid : IBluetoothManager
    {
        public static Activity AppActivity = CrossCurrentActivity.Current.Activity;
        public static Context AppContext = CrossCurrentActivity.Current.AppContext;

        public static Handler BluetoothHandler;

        public static MyBluetoothService
            BluetoothService = new MyBluetoothService(ref AppContext, ref BluetoothHandler);

        public void Connect(string macaddr, bool secureconnection = false)
        {
            if (BluetoothHandler == null) throw new NullPointerException();

            BluetoothService.Connect(BluetoothService.mAdapter.GetRemoteDevice(Encoding.ASCII.GetBytes(macaddr)),
                secureconnection);
        }

        public int GetConnectionState()
        {
            if (BluetoothHandler == null) throw new NullPointerException();
            return BluetoothService.GetState();
        }

        public void Start()
        {
            if (BluetoothHandler == null) throw new NullPointerException();
            BluetoothService.Start();
        }

        public void Stop()
        {
            if (BluetoothHandler == null) throw new NullPointerException();
            BluetoothService.Stop();
        }
    }
}