using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.IO;
using Java.Lang;
using Java.Util;
using String = System.String;

namespace GoBabyGoV2.Droid.DependencyServices
{
    public class MyBluetoothService
    {
        private static object _lock = new object();

        // Debugging
        private static string TAG = "GBGBTService";

        // Name for the SDP record when creating server socket
        private static string NAME_SECURE = "GBGBTSecure";
        private static string NAME_INSECURE = "GBGBTInsecure";

        // Unique UUID for this application
//        private static UUID MY_UUID_SECURE = UUID.FromString("fa87c0d0-afac-11de-8a39-0800200c9a66");
//        private static UUID MY_UUID_INSECURE = UUID.FromString("8ce255c0-200a-11e0-ac64-0800200c9a66");

        private static UUID BLUETOOTH_CLASSIC_UUID = UUID.FromString("00001101-0000-1000-8000-00805F9B34FB");

        // Member fields
        public BluetoothAdapter mAdapter;
        private Handler mHandler;
        private AcceptThread mSecureAcceptThread;
        private AcceptThread mInsecureAcceptThread;
        private ConnectThread mConnectThread;
        private ConnectedThread mConnectedThread;
        private int mState;
        private int mNewState;

        // Constants that indicate the current connection state
        public const int STATE_NONE = 0; // we're doing nothing
        public const int STATE_LISTEN = 1; // now listening for incoming connections
        public const int STATE_CONNECTING = 2; // now initiating an outgoing connection
        public const int STATE_CONNECTED = 3; // now connected to a remote device

        // Message types sent from the BluetoothChatService Handler
        public const int MESSAGE_STATE_CHANGE = 1;
        public const int MESSAGE_READ = 2;
        public const int MESSAGE_WRITE = 3;
        public const int MESSAGE_DEVICE_NAME = 4;
        public const int MESSAGE_TOAST = 5;

        // Key names received from the BluetoothChatService Handler
        public const string DEVICE_NAME = "device_name";
        public const string TOAST = "toast";

        public MyBluetoothService(ref Context context, ref Handler handler)
        {
            mAdapter = BluetoothAdapter.DefaultAdapter;
            mState = STATE_NONE;
            mNewState = mState;
            mHandler = handler;
        }

        /**
        * Update UI title according to the current state of the chat connection
        */
        private void UpdateUserInterfaceTitle()
        {
            lock (_lock)
            {
                mState = GetState();

                Log.Debug(TAG, "updateUserInterfaceTitle() " + mNewState + " -> " + mState);
                mNewState = mState;

                // Give the new state to the Handler so the UI Activity can update
                mHandler.ObtainMessage(MESSAGE_STATE_CHANGE, mNewState, -1).SendToTarget();
            }
        }

        /**
        * Return the current connection state.
        */
        public int GetState()
        {
            lock (_lock) return mState;
        }

        /**
         * Start the chat service. Specifically start AcceptThread to begin a
         * session in listening (server) mode. Called by the Activity onResume()
         */
        public void Start()
        {
            lock (_lock)
            {
                Log.Debug(TAG, "start");

                // Cancel any thread attempting to make a connection
                if (mConnectThread != null)
                {
                    mConnectThread.Cancel();
                    mConnectThread = null;
                }

                // Cancel any thread currently running a connection
                if (mConnectedThread != null)
                {
                    mConnectedThread.Cancel();
                    mConnectedThread = null;
                }

                // Start the thread to listen on a BluetoothServerSocket
                if (mSecureAcceptThread == null)
                {
                    mSecureAcceptThread = new AcceptThread(true, this);
                    mSecureAcceptThread.Start();
                }

                if (mInsecureAcceptThread == null)
                {
                    mInsecureAcceptThread = new AcceptThread(false, this);
                    mInsecureAcceptThread.Start();
                }

                // Update UI title
                UpdateUserInterfaceTitle();
            }
        }

        /**
         * Start the ConnectThread to initiate a connection to a remote device.
         *
         * @param device The BluetoothDevice to connect
         * @param secure Socket Security type - Secure (true) , Insecure (false)
         */
        public void Connect(BluetoothDevice device, bool secure)
        {
            lock (_lock)
            {
                Log.Debug(TAG, "connect to: " + device);

                // Cancel any thread attempting to make a connection
                if (mState == STATE_CONNECTING)
                {
                    if (mConnectThread != null)
                    {
                        mConnectThread.Cancel();
                        mConnectThread = null;
                    }
                }

                // Cancel any thread currently running a connection
                if (mConnectedThread != null)
                {
                    mConnectedThread.Cancel();
                    mConnectedThread = null;
                }

                // Start the thread to connect with the given device
                mConnectThread = new ConnectThread(device, secure, this);
                mConnectThread.Start();

                // Update UI title
                UpdateUserInterfaceTitle();
            }
        }

        /**
         * Start the ConnectedThread to begin managing a Bluetooth connection
         *
         * @param socket The BluetoothSocket on which the connection was made
         * @param device The BluetoothDevice that has been connected
         */
        public void Connected(BluetoothSocket socket, BluetoothDevice device, string socketType)
        {
            lock (_lock)
            {
                Log.Debug(TAG, "connected, Socket Type:" + socketType);

                // Cancel the thread that completed the connection
                if (mConnectThread != null)
                {
                    mConnectThread.Cancel();
                    mConnectThread = null;
                }

                // Cancel any thread currently running a connection
                if (mConnectedThread != null)
                {
                    mConnectedThread.Cancel();
                    mConnectedThread = null;
                }

                // Cancel the accept thread because we only want to connect to one device
                if (mSecureAcceptThread != null)
                {
                    mSecureAcceptThread.Cancel();
                    mSecureAcceptThread = null;
                }

                if (mInsecureAcceptThread != null)
                {
                    mInsecureAcceptThread.Cancel();
                    mInsecureAcceptThread = null;
                }

                // Start the thread to manage the connection and perform transmissions
                mConnectedThread = new ConnectedThread(socket, socketType, this);
                mConnectedThread.Start();

                // Send the name of the connected device back to the UI Activity
                Message msg = mHandler.ObtainMessage(MESSAGE_DEVICE_NAME);
                Bundle bundle = new Bundle();

                bundle.PutString(DEVICE_NAME, device.Name);
                msg.Data = bundle;

                mHandler.SendMessage(msg);

                // Update UI title
                UpdateUserInterfaceTitle();
            }
        }

        /**
         * Stop all threads
         */
        public void Stop()
        {
            lock (_lock)
            {
                Log.Debug(TAG, "stop");

                if (mConnectThread != null)
                {
                    mConnectThread.Cancel();
                    mConnectThread = null;
                }

                if (mConnectedThread != null)
                {
                    mConnectedThread.Cancel();
                    mConnectedThread = null;
                }

                if (mSecureAcceptThread != null)
                {
                    mSecureAcceptThread.Cancel();
                    mSecureAcceptThread = null;
                }

                if (mInsecureAcceptThread != null)
                {
                    mInsecureAcceptThread.Cancel();
                    mInsecureAcceptThread = null;
                }

                mState = STATE_NONE;

                // Update UI title
                UpdateUserInterfaceTitle();
            }
        }

        /**
         * Write to the ConnectedThread in an unsynchronized manner
         *
         * @param out The bytes to write
         * @see ConnectedThread#write(byte[])
         */
        public void Write(byte[] bytes)
        {
            // Create temporary object
            ConnectedThread r;

            // Synchronize a copy of the ConnectedThread
            lock (_lock)
            {
                if (mState != STATE_CONNECTED) return;
                r = mConnectedThread;
            }

            // Perform the write unsynchronized
            r.Write(bytes);
        }

        /**
         * Indicate that the connection attempt failed and notify the UI Activity.
         */
        private void ConnectionFailed()
        {
            // Send a failure message back to the Activity
            Message msg = mHandler.ObtainMessage(MESSAGE_TOAST);
            Bundle bundle = new Bundle();

            bundle.PutString(TOAST, "Unable to connect device");
            msg.Data = bundle;

            mHandler.SendMessage(msg);

            mState = STATE_NONE;

            // Update UI title
            UpdateUserInterfaceTitle();

            // Start the service over to restart listening mode
            this.Start();
        }

        /**
         * Indicate that the connection was lost and notify the UI Activity.
         */
        private void ConnectionLost()
        {
            // Send a failure message back to the Activity
            Message msg = mHandler.ObtainMessage(MESSAGE_TOAST);
            Bundle bundle = new Bundle();

            bundle.PutString(TOAST, "Device connection was lost");
            msg.Data = bundle;

            mHandler.SendMessage(msg);

            mState = STATE_NONE;

            // Update UI title
            UpdateUserInterfaceTitle();

            // Start the service over to restart listening mode
            this.Start();
        }


        public class AcceptThread : Thread
        {
            // The local server socket
            private BluetoothServerSocket mmServerSocket;

            private MyBluetoothService parentBluetoothService;

//            private BluetoothAdapter mAdapter;
            private string mSocketType;

            public AcceptThread(bool secure, MyBluetoothService bluetoothService)
            {
                BluetoothServerSocket tmp = null;
                mSocketType = secure ? "Secure" : "Insecure";
                parentBluetoothService = bluetoothService;

                // Create a new listening server socket
                try
                {
                    if (secure)
                    {
                        tmp = parentBluetoothService.mAdapter
                            .ListenUsingRfcommWithServiceRecord(NAME_SECURE, BLUETOOTH_CLASSIC_UUID);
//                        tmp = parentBluetoothService.mAdapter.ListenUsingRfcommWithServiceRecord(NAME_SECURE, MY_UUID_SECURE);
                    }
                    else
                    {
                        tmp = parentBluetoothService.mAdapter
                            .ListenUsingInsecureRfcommWithServiceRecord(NAME_INSECURE, BLUETOOTH_CLASSIC_UUID);
//                        tmp = parentBluetoothService.mAdapter.ListenUsingInsecureRfcommWithServiceRecord(NAME_INSECURE, MY_UUID_INSECURE);
                    }
                }
                catch (IOException e)
                {
                    Log.Error(TAG, "Socket Type: " + mSocketType + "listen() failed", e);
                }

                mmServerSocket = tmp;
                parentBluetoothService.mState = STATE_LISTEN;
            }

            public override void Run()
            {
                Log.Debug(TAG, "Socket Type: " + mSocketType +
                               "BEGIN mAcceptThread" + this);

                Name = ("AcceptThread" + mSocketType);

                BluetoothSocket socket = null;

                // Listen to the server socket if we're not connected
                while (parentBluetoothService.mState != STATE_CONNECTED)
                {
                    try
                    {
                        // This is a blocking call and will only return on a
                        // successful connection or an exception
                        socket = mmServerSocket.Accept();
                    }
                    catch (IOException e)
                    {
                        Log.Error(TAG, "Socket Type: " + mSocketType + "accept() failed", e);
                        break;
                    }

                    // If a connection was accepted
                    if (socket != null)
                    {
                        lock (_lock)
                        {
                            switch (parentBluetoothService.mState)
                            {
                                case STATE_LISTEN:
                                case STATE_CONNECTING:
                                    // Situation normal. Start the connected thread.
                                    parentBluetoothService.Connected(socket, socket.RemoteDevice, mSocketType);
                                    break;

                                case STATE_NONE:
                                case STATE_CONNECTED:
                                    // Either not ready or already connected. Terminate new socket.
                                    try
                                    {
                                        socket.Close();
                                    }
                                    catch (IOException e)
                                    {
                                        Log.Error(TAG, "Could not close unwanted socket", e);
                                    }

                                    break;
                            }
                        }
                    }
                }

                Log.Info(TAG, "END mAcceptThread, socket Type: " + mSocketType);
            }

            public void Cancel()
            {
                Log.Debug(TAG, "Socket Type" + mSocketType + "cancel " + this);
                try
                {
                    mmServerSocket.Close();
                }
                catch (IOException e)
                {
                    Log.Error(TAG, "Socket Type" + mSocketType + "close() of server failed", e);
                }
            }
        }


        /**
     * This thread runs while attempting to make an outgoing connection
     * with a device. It runs straight through; the connection either
     * succeeds or fails.
     */
        private class ConnectThread : Thread
        {
            private MyBluetoothService parentBluetoothService;
            private BluetoothSocket mmSocket;
            private BluetoothDevice mmDevice;
            private string mSocketType;

            public ConnectThread(BluetoothDevice device, bool secure, MyBluetoothService bluetoothService)
            {
                mmDevice = device;
                BluetoothSocket tmp = null;
                mSocketType = secure ? "Secure" : "Insecure";
                parentBluetoothService = bluetoothService;

                // Get a BluetoothSocket for a connection with the
                // given BluetoothDevice
                try
                {
                    if (secure)
                    {
                        tmp = device.CreateRfcommSocketToServiceRecord(BLUETOOTH_CLASSIC_UUID);
//                    tmp = device.CreateRfcommSocketToServiceRecord(MY_UUID_SECURE);
                    }
                    else
                    {
                        tmp = device.CreateInsecureRfcommSocketToServiceRecord(BLUETOOTH_CLASSIC_UUID);
//                    tmp = device.CreateInsecureRfcommSocketToServiceRecord(MY_UUID_INSECURE);
                    }
                }
                catch (IOException e)
                {
                    Log.Error(TAG, "Socket Type: " + mSocketType + "create() failed", e);
                }

                mmSocket = tmp;
                parentBluetoothService.mState = STATE_CONNECTING;
            }

            public override void Run()
            {
                Log.Info(TAG, "BEGIN mConnectThread SocketType:" + mSocketType);
                Name = ("ConnectThread" + mSocketType);

                // Always cancel discovery because it will slow down a connection
                parentBluetoothService.mAdapter.CancelDiscovery();

                // Make a connection to the BluetoothSocket
                try
                {
                    // This is a blocking call and will only return on a
                    // successful connection or an exception
                    mmSocket.Connect();
                }
                catch (IOException e)
                {
                    // Close the socket
                    try
                    {
                        mmSocket.Close();
                    }
                    catch (IOException e2)
                    {
                        Log.Error(TAG, "unable to close() " + mSocketType +
                                       " socket during connection failure", e2);
                    }

                    parentBluetoothService.ConnectionFailed();
                    return;
                }

                // Reset the ConnectThread because we're done
                lock (_lock)
                {
                    parentBluetoothService.mConnectThread = null;
                }

                // Start the connected thread
                parentBluetoothService.Connected(mmSocket, mmDevice, mSocketType);
            }

            public void Cancel()
            {
                try
                {
                    mmSocket.Close();
                }
                catch (IOException e)
                {
                    Log.Error(TAG, "close() of connect " + mSocketType + " socket failed", e);
                }
            }
        }

        /**
         * This thread runs during a connection with a remote device.
         * It handles all incoming and outgoing transmissions.
         */
        private class ConnectedThread : Thread
        {
            private MyBluetoothService parentBluetoothService;
            private BluetoothSocket mmSocket;
            private System.IO.Stream mmInStream;
            private System.IO.Stream mmOutStream;

            public ConnectedThread(BluetoothSocket socket, string socketType, MyBluetoothService bluetoothService)
            {
                parentBluetoothService = bluetoothService;
                Log.Debug(TAG, "create ConnectedThread: " + socketType);
                mmSocket = socket;

                System.IO.Stream tmpIn = null;
                System.IO.Stream tmpOut = null;

                // Get the BluetoothSocket input and output streams
                try
                {
                    tmpIn = socket.InputStream;
                    tmpOut = socket.OutputStream;
                }
                catch (IOException e)
                {
                    Log.Error(TAG, "temp sockets not created", e);
                }

                mmInStream = tmpIn;
                mmOutStream = tmpOut;
                parentBluetoothService.mState = STATE_CONNECTED;
            }

            public override void Run()
            {
                Log.Info(TAG, "BEGIN mConnectedThread");
                byte[] buffer = new byte[1024];
                int bytes;

                // Keep listening to the InputStream while connected
                while (parentBluetoothService.mState == STATE_CONNECTED)
                {
                    try
                    {
                        // Read from the InputStream
                        bytes = mmInStream.Read(buffer);

                        // Send the obtained bytes to the UI Activity
                        parentBluetoothService.mHandler.ObtainMessage(MESSAGE_READ, bytes, -1, buffer).SendToTarget();
                    }
                    catch (IOException e)
                    {
                        Log.Error(TAG, "disconnected", e);
                        parentBluetoothService.ConnectionLost();
                        break;
                    }
                }
            }

            /**
             * Write to the connected OutStream.
             *
             * @param buffer The bytes to write
             */
            public void Write(byte[] buffer)
            {
                try
                {
                    mmOutStream.Write(buffer);

                    // Share the sent message back to the UI Activity
                    parentBluetoothService.mHandler.ObtainMessage(MESSAGE_WRITE, -1, -1, buffer).SendToTarget();
                }
                catch (IOException e)
                {
                    Log.Error(TAG, "Exception during write", e);
                }
            }

            public void Cancel()
            {
                try
                {
                    mmSocket.Close();
                }
                catch (IOException e)
                {
                    Log.Error(TAG, "close() of connect socket failed", e);
                }
            }
        }
    }
}