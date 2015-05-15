using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adas.ArduinoTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var arduino = new SerialPort("COM4", 9600))
            {
                arduino.Open();
                var random = new Random();
                //while (true)
                //{
                //    //var message = arduino.ReadLine();
                //    //Console.WriteLine(message);
                arduino.Write(new [] {Convert.ToByte(180)}, 0, 1);
               // }
                arduino.Close();
            }
        }
    }
}
