using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
// using System.Networking.Definitions;

class Monolith
{
  // server side data example
  int example = 0;

  class Peer
  {
    public string id;
    public IPEndPoint endPoint;
    public int data;
  }

  static void Main(string[] args)
  {
    Console.WriteLine("starting up...");

    // listen for clients on udp port 1234
    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Udp);
    socket.Bind(new IPEndPoint(IPAddress.Any, 1234));
    Console.WriteLine("now listening...");

    // peer data is temporary, blockchain+ipfs is forever
    peerList = new List<Peer>();

    // loop forever
    while (true)
    {
      byte[] data;
      // receive a message
      IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);

      data = udp.ReceiveAsync(ref sender, );

      bool newPeer = true;
      for (int i = 0; i < peerList.Count; i++)
      {
        Peer peer = peerList[i];
        if (peer.endPoint == sender)
        {
          // update peer data
          peer.data = BitConverter.ToInt32(data, 0);
          newPeer = false;
        }
      }
      if (newPeer)
      {
        // add peer to list
        peerList.Add(new Peer() { id = Encoding.ASCII.GetBytes(data), endPoint = sender, data = 0 });
      }


      // realtime data example
      // 

      // print the message
      Console.WriteLine("received message: " + Encoding.ASCII.GetString(data));

      // send a message back to the client
      Console.Write("Enter a message: ");
      string message = Console.ReadLine();
      data = Encoding.ASCII.GetBytes(message);
      udp.Send(data, data.Length, sender);
    }
  }
}