using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace UdpClientDemo
{
    class Program
    {
        static UdpClient udpClient;
        static void Main(string[] args)
        {
            udpClient = new UdpClient(50000);       // 当前客户端使用的端口
            udpClient.Connect("127.0.0.1", 61000); // 与服务器建立连接
            Console.WriteLine("客户端已启用......");

            #region 开启线程保持通讯

            var t1 = new Thread(SendMsg);
            t1.Start();
            var t2 = new Thread(ReciveMsg);
            t2.Start();

            #endregion
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        static void SendMsg()
        {
            while (true)
            {
                var msg = Console.ReadLine().ToString();        　　 // 获取控制台字符串
                byte[] sendBytes = Encoding.UTF8.GetBytes(msg); // 将消息编码成字符串数组
                udpClient.Send(sendBytes, sendBytes.Length);       // 发送数据报
            }
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        static void ReciveMsg()
        {
            var remoteIpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 61000); // 远程端点，即发送消息方的端点
            while (true)
            {
                byte[] receiveBytes = udpClient.Receive(ref remoteIpEndPoint); // 接收消息，得到数据报
                string returnData = Encoding.UTF8.GetString(receiveBytes);      // 解析字节数组，得到原消息
                Console.WriteLine($"{remoteIpEndPoint.Address}:{remoteIpEndPoint.Port}，" + returnData.ToString());
            }

        }
    }
}
