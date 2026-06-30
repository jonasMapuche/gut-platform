using Android.Bluetooth;
using Android.Content;
using Gut.Interfaces;
using Gut.Models;
using Gut.Platforms.Android.Broadcasts;
using Java.Util;
using System.Text;

namespace Gut.Platforms.Android.Services
{
    public class BluetoothService : IBluetoothService
    {
        public List<Message> Receiver { get; set; }
        private BluetoothSocket socket;
        private static readonly UUID MY_UUID = UUID.FromString("00001101-0000-1000-8000-00805F9B34FB");

        BluetoothAdapter _bluetoothAdapter;
        public BluetoothService()
        {
            this.Receiver = new List<Message>();
        }

        public void SetUp()
        {
            try
            {
                BluetoothManager bluetoothManager = (BluetoothManager)Platform.AppContext.GetSystemService(Context.BluetoothService);
                this._bluetoothAdapter = bluetoothManager.Adapter;
                if (bluetoothManager.Adapter != null && !bluetoothManager.Adapter.IsEnabled)
                {
                    Intent enableBtIntent = new Intent(BluetoothAdapter.ActionRequestEnable);
                    enableBtIntent.SetFlags(ActivityFlags.NewTask);
                    Platform.AppContext.StartActivity(enableBtIntent);
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public void Scan()
        {
            try
            {
                BluetoothBroadcast receiver = new BluetoothBroadcast();
                this.Receiver = receiver.Receiver;
                IntentFilter filter = new IntentFilter(BluetoothDevice.ActionFound);
                Platform.AppContext.RegisterReceiver(receiver, filter);
                this._bluetoothAdapter.StartDiscovery();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public void Connect(string address, string file_path)
        {
            this._bluetoothAdapter.CancelDiscovery();
            BluetoothDevice device = this._bluetoothAdapter.GetRemoteDevice(address);
            try
            {
                socket = device.CreateRfcommSocketToServiceRecord(MY_UUID);
                socket.Connect();
            }
            catch (Java.IO.IOException e)
            {
                try
                {
                    Java.Lang.Reflect.Method createRfcommSocketMethod = device.Class.GetMethod("createRfcommSocket", new Java.Lang.Class[] { Java.Lang.Integer.Type });
                    Java.Lang.Object? resultSocket = createRfcommSocketMethod.Invoke(device, new Java.Lang.Object[] { int.Parse("1") });
                    socket = (BluetoothSocket)resultSocket;
                    socket.Connect();
                    SendOne(file_path);
                    SendTwo(file_path);
                }
                catch (Exception ex)
                {
                    socket.Close();
                    throw new NotImplementedException(ex.Message);
                }
            }
        }

        private async void SendOne(string file_path)
        {
            try
            {
                string file_name = Path.GetFileName(file_path);

                FileStream fs = new FileStream(file_path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
                MemoryStream ms = new MemoryStream();
                await fs.CopyToAsync(ms);
                ms.Position = 0;
                byte[] file_data = ms.ToArray();

                Stream outputStream = socket.OutputStream;

                UnicodeEncoding encoding = new UnicodeEncoding(true, false);
                byte[] encoded_name = encoding.GetBytes(file_name + "\0");

                int header_length = 3 + encoded_name.Length;
                byte[] name_header = new byte[header_length];
                name_header[0] = 0x01;
                name_header[1] = (byte)((encoded_name.Length + 3) >> 8);
                name_header[2] = (byte)(encoded_name.Length + 3);
                Array.Copy(encoded_name, 0, name_header, 3, encoded_name.Length);

                byte[] length_header = new byte[5];
                length_header[0] = 0xC3;
                byte[] size_bytes = BitConverter.GetBytes((uint)file_data.Length);
                if (BitConverter.IsLittleEndian) Array.Reverse(size_bytes);
                Array.Copy(size_bytes, 0, length_header, 1, 4);

                int packet_size = 3 + name_header.Length + length_header.Length;
                byte[] put_packet = new byte[packet_size];
                put_packet[0] = 0x82;
                put_packet[1] = (byte)(packet_size >> 8);
                put_packet[2] = (byte)packet_size;

                int offset = 3;
                Array.Copy(name_header, 0, put_packet, offset, name_header.Length);
                offset += name_header.Length;
                Array.Copy(length_header, 0, put_packet, offset, length_header.Length);

                outputStream.Write(put_packet, 0, put_packet.Length);
                outputStream.Flush();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        private async void SendTwo(string file_path)
        {
            try
            {
                byte[] connect_packet = new byte[7];
                connect_packet[0] = 0x80;
                connect_packet[1] = 0x00;
                connect_packet[2] = 0x07;
                connect_packet[3] = 0x10;
                connect_packet[4] = 0x00;
                connect_packet[5] = 0x20;
                connect_packet[6] = 0x00;
                Stream outputStream = socket.OutputStream;
                outputStream.WriteAsync(connect_packet, 0, connect_packet.Length);
                outputStream.FlushAsync();

                string file_name = Path.GetFileName(file_path);

                FileStream fs = new FileStream(file_path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
                MemoryStream memory_stream = new MemoryStream();
                await fs.CopyToAsync(memory_stream);
                memory_stream.Position = 0;
                byte[] file_data = memory_stream.ToArray();

                MemoryStream ms = new MemoryStream();
                ms.WriteByte(0x02);
                byte[] nameBytes = Encoding.BigEndianUnicode.GetBytes(file_name);
                ms.WriteByte(0x01);

                ushort nameLen = (ushort)(nameBytes.Length + 2 + 3);
                ms.WriteByte((byte)(nameLen >> 8));
                ms.WriteByte((byte)nameLen);
                ms.Write(nameBytes, 0, nameBytes.Length);
                ms.WriteByte(0x00);
                ms.WriteByte(0x00);

                byte[] typeBytes = Encoding.ASCII.GetBytes("image/jpeg");
                ms.WriteByte(0x42);
                ushort typeLen = (ushort)(typeBytes.Length + 1 + 3);
                ms.WriteByte((byte)(typeLen >> 8));
                ms.WriteByte((byte)typeLen);
                ms.Write(typeBytes, 0, typeBytes.Length);
                ms.WriteByte(0x00);

                ms.WriteByte(0xC3);
                byte[] fileLenBytes = BitConverter.GetBytes(file_data.Length);
                if (BitConverter.IsLittleEndian) Array.Reverse(fileLenBytes);
                ms.Write(fileLenBytes, 0, fileLenBytes.Length);

                ms.WriteByte(0x49);
                ushort bodyLen = (ushort)(file_data.Length + 3);
                ms.WriteByte((byte)(bodyLen >> 8));
                ms.WriteByte((byte)bodyLen);
                ms.Write(file_data, 0, file_data.Length);

                byte[] packetBytes = ms.ToArray();
                ushort totalSize = (ushort)(packetBytes.Length + 3);

                packetBytes[1] = (byte)(totalSize >> 8);
                packetBytes[2] = (byte)totalSize;

                outputStream.WriteAsync(packetBytes, 0, packetBytes.Length);
                outputStream.FlushAsync();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }
    }
}
