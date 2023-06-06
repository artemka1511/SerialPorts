using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPorts
{
    class MySerialPort : SerialPort
    {
        private const int DataSize = 54;
        private readonly byte[] _bufer = new byte[DataSize];
        private int _stepIndex;
        private bool _startRead;
        private bool _virtualPort;

        public MySerialPort(string port, bool virtualPort) : base()
        {
            PortName = port;
            BaudRate = 115200;
            DataBits = 8;
            StopBits = StopBits.Two;
            Parity = Parity.None;
            ReadTimeout = 1000;
            _virtualPort = virtualPort;

            //DataReceived += SerialPort_DataReceived;
        }

        //void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        //{
        //    var port = (SerialPort)sender;
        //    try
        //    {
        //        byte[] bytes = new byte[256];
        //        var stream = port.BaseStream;

        //        int i;

        //        while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
        //        {
        //            string data = Encoding.ASCII.GetString(bytes, 0, i);
        //            Console.WriteLine($"Получено сообщение в {PortName}: {data}");
        //            MessageHandler(data);
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}

        //void MessageHandler(string data)
        //{
        //    switch (data)
        //    {
        //        case "log version"
        //        default:
        //            break;
        //    }
        //}


    }
}
