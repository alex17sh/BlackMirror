using SchoolManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SchoolTcpServer
{
    class Program
    {
        const int Port = 9700;
        const string IP = "127.0.0.1";
        private static School s_SchoolManager = new School();

        static void Main(string[] args)
        {
            IPAddress localAdd = IPAddress.Parse(IP);
            TcpListener listener = new TcpListener(localAdd, Port);
            listener.Start();

            while (true)
            {
                try
                {
                    //---incoming client connected---
                    TcpClient client = listener.AcceptTcpClient();
                    //---get the incoming data through a network stream---
                    NetworkStream nwStream = client.GetStream();
                    byte[] buffer = new byte[client.ReceiveBufferSize];
                    //---read incoming stream---
                    int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);
                    Stream stream = new MemoryStream(buffer);
                    BinaryReader reader = new BinaryReader(stream);
                    int req = reader.ReadInt32();
                    byte op = reader.ReadByte();
                    int size = reader.ReadInt32();
                    string payload = Encoding.ASCII.GetString(reader.ReadBytes(size));
                    string[] commandArgs = payload.Split('\0');

                    string res = HandleCommand(op, commandArgs);

                    //---write back the text to the client---
                    byte[] bytesToSend = Encoding.ASCII.GetBytes(res);
                    nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                    client.Close();
                }
                catch (Exception e)
                {
                    //LOG ERROR
                    Console.WriteLine("something wrong happened {0}", e);
                }
            }
            
            listener.Stop();

        }

        private static string HandleCommand(byte op, string[] commandArgs)
        {
            switch (op)
            {
                case 0x01:
                    return s_SchoolManager.AddStudent(commandArgs[0], commandArgs[1], int.Parse(commandArgs[2]), int.Parse(commandArgs[3]));                    
                case 0x02:
                    return s_SchoolManager.AddTeacher(commandArgs[0], commandArgs[1], int.Parse(commandArgs[2]));
                case 0x03:
                    return s_SchoolManager.EnterClass(int.Parse(commandArgs[0]), int.Parse(commandArgs[1]));
                case 0x04:
                    return s_SchoolManager.ExitClass(int.Parse(commandArgs[0]), int.Parse(commandArgs[1]));
                case 0x05:
                    return s_SchoolManager.Eating(int.Parse(commandArgs[0]));
                case 0x06:
                    return s_SchoolManager.Chat(int.Parse(commandArgs[0]), int.Parse(commandArgs[1]));
                case 0xA0:
                    return s_SchoolManager.GetStudent();
                case 0xA1:
                    return s_SchoolManager.GetTeachers();
                case 0xA2:
                    return s_SchoolManager.WhoAte();
                case 0xA3:
                    return s_SchoolManager.ClassPresence();
                default:
                    return "-1wrong command";
            }
        }
    }
}
