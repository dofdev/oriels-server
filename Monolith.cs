using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
// using System.Networking.Definitions;

class Monolith {
  class Peer {
    public string id;
    public string endPoint;
    public int data;

    public Peer(string endPoint) {
      this.endPoint = endPoint;
    }
  }

  static void Main(string[] args) {
    Console.WriteLine("oriels server now booting up...");

    // listen for clients on udp port 1234
    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
    socket.Bind(new IPEndPoint(IPAddress.Any, 1234));
    Console.WriteLine("socket bound on port: 1234");

    // peer data is temporary, blockchain+ipfs is forever
    List<Peer> peerList = new List<Peer>();

    while (true) {
      byte[] data = new byte[1024];
      EndPoint sender = new IPEndPoint(IPAddress.Any, 0);
      socket.ReceiveFrom(data, ref sender);

      bool newPeer = true;
      for (int i = 0; i < peerList.Count; i++) {
        Peer peer = peerList[i];
        if (peer.endPoint == sender.ToString()) {
          // update peer data
          peer.data = BitConverter.ToInt32(data, 0);
          // Console.WriteLine("peer data updated: " + peer.data);
          newPeer = false;
          break;
        }
      }
      if (newPeer) {
        peerList.Add(new Peer(sender.ToString()));
        Console.WriteLine("new peer connected");
      }

      // send data to peers
      for (int i = 0; i < peerList.Count; i++)
      {
        Peer peer = peerList[i];
        Console.WriteLine("sending data to peer: " + peer.data);
        data = BitConverter.GetBytes(peer.data);
        socket.SendTo(data, IPEndPoint.Parse(peer.endPoint));
      }


      Thread.Sleep(100); // mainly for debugging
    }
  }
}