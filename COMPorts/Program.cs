using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace COMPorts
{
    class Program
    {
        public static SerialPort VirtualSerialPort { get; set; }
        public static SerialPort GPSCOMPort { get; set; }
        public static async Task Main(string[] args)
        {
            
            while (true)
            {
                string virtualPortValue = System.Configuration.ConfigurationManager.AppSettings["VirtualCOMPort"];

                string gpsPortValue = System.Configuration.ConfigurationManager.AppSettings["GpsCOMPort"];

                if (!CheckPort(virtualPortValue)) { ShowWarningMessage(virtualPortValue); }
                else if (!CheckPort(gpsPortValue)) { ShowWarningMessage(virtualPortValue); }
                else
                {
                    CreateAndOpenComPort(virtualPortValue, true);
                    CreateAndOpenComPort(gpsPortValue, false);
                }

                Console.ReadLine();
            }
        }

        public static void CreateAndOpenComPort(string portName, bool virtualPort)
        {
            if(virtualPort)
            {
                VirtualSerialPort = new MySerialPort(portName, virtualPort);

                VirtualSerialPort.DataReceived += VirtualSerialPort_DataReceived;

                VirtualSerialPort.Open();

                Console.WriteLine($"Связь с виртуальным COM портом {portName} успешно установлена");
            }
            else
            {
                GPSCOMPort = new MySerialPort(portName, virtualPort);

                GPSCOMPort.Open();

                Console.WriteLine($"Связь с GPS COM портом {portName} успешно установлена");
            }
        }

        public static void VirtualSerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var port = (SerialPort)sender;
            try
            {
                byte[] bytes = new byte[256];
                var stream = port.BaseStream;

                int i;

                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    string data = Encoding.ASCII.GetString(bytes, 0, i);
                    Console.WriteLine($"Получено сообщение в {port.PortName}: {data}");
                    MessageHandler(data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void MessageHandler(string data)
        {
            switch (data.Replace("\r\n", string.Empty))
            {
                case "log version":
                    VirtualSerialPort.Write("Hello world!");
                    break;
                default:
                    GPSCOMPort.Write(data);
                    break;
            }
        }

        public static void ShowWarningMessage(string portName)
        {
            Console.WriteLine($"Ошибка: порт {portName} отсутствует!");
            Console.ReadLine();
        }

        public static bool CheckPort(string portName)
        {
            string[] ports = SerialPort.GetPortNames();

            if(ports.Contains(portName)) { return true; }
            else { return false; }
        }
    }
}
