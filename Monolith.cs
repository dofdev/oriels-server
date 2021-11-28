using System;
using System.Net;
using System.Net.Sockets;

class Monolith {
  class Peer {
    public string endPoint;
    public byte[] data;

    public Peer(string endPoint, byte[] data) {
      this.endPoint = endPoint;
      this.data = data;
    }
  }

  static void Main(string[] args) {
    Console.WriteLine("oriels server now booting up...");

    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
    // socket.SendTimeout = 1000;
    // socket.ReceiveTimeout = 1000;
    socket.Bind(new IPEndPoint(IPAddress.Any, 1234));
    Console.WriteLine("socket bound on port: 1234");

    // peer data is temporary, blockchain+ipfs is forever
    Peer[] peers = new Peer[64];

    while (true) {
      int bufferSize = 1024;
      byte[] data = new byte[bufferSize]; 
      EndPoint sender = new IPEndPoint(IPAddress.Any, 0);
      while (socket.Available > 0) {
        int index = 0;
        try {
          socket.ReceiveFrom(data, 0, bufferSize, SocketFlags.None, ref sender);
          for (int i = 0; i < peers.Length; i++) {
            if (peers[i] != null) {
              if (peers[i].endPoint == sender.ToString()) {
                index = i;
                // data.CopyTo(peers[i].data, 0);
                peers[i].data = data;
                break;
              }
            } else {
              peers[i] = new Peer(sender.ToString(), data);
              Console.WriteLine("new peer connected");
              break;
            }
          }
        } catch (Exception e) {
          Console.WriteLine(e.Message);
          peers[index] = null;
          break;
        }
      }

      for (int i = 0; i < peers.Length; i++)
      {
        Peer peer = peers[i];
        if (peer != null) {
          for (int j = 0; j < peers.Length; j++)
          {
            Peer peer2 = peers[j];

            if (peer2 != null && peer2 != peer) {
              socket.SendTo(peer.data, IPEndPoint.Parse(peer2.endPoint));
            }
          }
        }
      }
    }
  }
}