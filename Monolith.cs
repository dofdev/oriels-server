using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

class Monolith {
  class Peer {
    public string endPoint;
    public byte[] data;
    public long time;

    public Peer(string endPoint, byte[] data, long time) {
      this.endPoint = endPoint;
      this.data = data;
      this.time = time;
    }
  }

  static void Main(string[] args) {
    Console.WriteLine("oriels server now booting up...");
    long time = 0;
    Thread clock = new Thread(Clock);
    clock.Start();
    void Clock() {
      bool running = true;
      while (running) {
        time++;
        Thread.Sleep(1000);
      }
    }

    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
    EndPoint sender = new IPEndPoint(IPAddress.Any, 0);
    // socket.SendTimeout = 1000;
    // socket.ReceiveTimeout = 1000;
    socket.Bind(new IPEndPoint(IPAddress.Any, 1234));
    Console.WriteLine("socket bound on port: 1234");

    // peer data is temporary, blockchain+ipfs is forever
    Peer[] peers = new Peer[64];

    bool running = true;
    while (running) {
      // clean up peers
      for (int i = 0; i < peers.Length; i++) {
        if (peers[i] != null && peers[i].time + 6 < time) {
          peers[i] = null;
          Console.WriteLine("peer timed out");
        }
      }

      int bufferSize = 1024;
      byte[] data = new byte[bufferSize]; 
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
                peers[i].time = time;
                break;
              }
            } else {
              peers[i] = new Peer(sender.ToString(), data, time);
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

      Thread.Sleep(60);
    }

    socket.Close();
  }
}