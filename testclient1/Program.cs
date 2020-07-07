using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace testclient1
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpClient tcpClient = new TcpClient();            
            tcpClient.Connect("10.158.64.53",44000);
            NetworkStream networkStream = tcpClient.GetStream();
            string texto, conf;
            while (true)
            {
                if (networkStream.CanWrite && networkStream.CanRead)
                {
                    Console.WriteLine("enviar texto");
                    // Does a simple write.
                    texto = Console.ReadLine();
                    texto = @"<CitiCommand Id=""1"" Type=""PtzControl"" ><PtzControl CameraId=""1"" CameraName=""ALL"" GotoPreset=""1""/></CitiCommand>";
                    Byte[] sendBytes = Encoding.ASCII.GetBytes(texto);
                    networkStream.Write(sendBytes, 0, sendBytes.Length);

                    Console.WriteLine("ver mensaje");
                    // Does a simple write.
                    conf = Console.ReadLine();
                    if (conf == "si")
                    {
                        // Reads the NetworkStream into a byte buffer.
                        byte[] bytes = new byte[tcpClient.ReceiveBufferSize];
                        networkStream.Read(bytes, 0, (int)tcpClient.ReceiveBufferSize);

                        // Returns the data received from the host to the console.
                        string returndata = Encoding.ASCII.GetString(bytes);

                        Console.WriteLine("This is what the host returned to you: " + returndata);
                    }
                    else
                    {
                        return;
                    }
                }
                else if (!networkStream.CanRead)
                {
                    Console.WriteLine("You can not write data to this stream");
                    tcpClient.Close();
                }
                else if (!networkStream.CanWrite)
                {
                    Console.WriteLine("You can not read data from this stream");
                    tcpClient.Close();
                }
            }
        }
    }
}
