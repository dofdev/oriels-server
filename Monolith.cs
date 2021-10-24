using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Monolith
{
  // make a github repo for this

  static void Main(string[] args)
  {
    Console.WriteLine("starting up...");

    // listen for clients on udp port 1234
    UdpClient listener = new UdpClient(1234);
    Console.WriteLine("now listening...");

    // loop forever
    while (true)
    {
      // receive a message
      IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
      byte[] message = listener.Receive(ref sender);


      // print the message
      Console.WriteLine("received message: " + Encoding.ASCII.GetString(message));
    }
  }
}