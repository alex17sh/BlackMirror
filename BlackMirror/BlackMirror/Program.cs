using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BlackMirror
{
    class Program
    {
        const int Port = 9700;
        const string IP = "127.0.0.1";

        static void Main(string[] args)
        {
            //---data to send to the server---
            string textToSend = DateTime.Now.ToString();

            //---create a TCPClient object at the IP and port no.---
            TcpClient client = new TcpClient(IP, Port);
            NetworkStream nwStream = client.GetStream();

            int req = 2000;
            byte op = 0xA3;
            string payload = "1\05";
            byte[] payloadBytes = ASCIIEncoding.ASCII.GetBytes(payload);
            int size = payloadBytes.Length;

            byte[] reqBytes = BitConverter.GetBytes(req);
            byte[] sizeBytes = BitConverter.GetBytes(size);




            byte[] bytesToSend = new byte[reqBytes.Length + 1 + sizeBytes.Length + payloadBytes.Length];

            reqBytes.CopyTo(bytesToSend, 0);
            bytesToSend[reqBytes.Length] = op;
            sizeBytes.CopyTo(bytesToSend, reqBytes.Length + 1);
            payloadBytes.CopyTo(bytesToSend, reqBytes.Length + 1 + sizeBytes.Length);

            //---send the text---
            Console.WriteLine("Sending : " + textToSend);
            nwStream.Write(bytesToSend, 0, bytesToSend.Length);

            //---read back the text---
            byte[] bytesToRead = new byte[client.ReceiveBufferSize];
            int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
            Console.WriteLine("Received : " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
            Console.ReadLine();
            client.Close();
        }

    }
}
