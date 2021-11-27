using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
// using Timer = System.Timers.Timer;
// using System.Networking.Definitions;

class Monolith {
  class Peer {
    public string endPoint;
    public float lastTime;
    
    public byte[] data = new byte[1024];

    public Peer(string endPoint) {
      this.endPoint = endPoint;
    }
  }

  public static long startTime;

  static void Main(string[] args) {
    var watch = System.Diagnostics.Stopwatch.StartNew();
    watch.Start();
    float time = 0;

    Console.WriteLine("oriels server now booting up...");

    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
    // socket.SendTimeout = 1000;
    // socket.ReceiveTimeout = 1000;
    socket.Bind(new IPEndPoint(IPAddress.Any, 1234));
    Console.WriteLine("socket bound on port: 1234");

    // peer data is temporary, blockchain+ipfs is forever
    List<Peer> peerList = new List<Peer>();

    while (true) {
      time = watch.ElapsedMilliseconds / 1000.0f;

      byte[] data = new byte[1024];
      EndPoint sender = new IPEndPoint(IPAddress.Any, 0);
      while (socket.Available > 0) {
        try {
          socket.ReceiveFrom(data, 0, 1024, SocketFlags.None, ref sender);
          bool newPeer = true;
          for (int i = 0; i < peerList.Count; i++) {
            Peer peer = peerList[i];
            if (peer.endPoint == sender.ToString()) {
              data.CopyTo(peer.data, 0);
              peer.lastTime = time;
              
              newPeer = false;
              break;
            }
          }
          if (newPeer) {
            peerList.Add(new Peer(sender.ToString()));
            Console.WriteLine("new peer connected");
          }
        } catch (Exception e) {
          Console.WriteLine("error receiving data" + e.Message);
          // socket.EndReceiveFrom(null, ref sender);
          break;
        }
      }

      for (int i = 0; i < peerList.Count; i++)
      {
        Peer peer = peerList[i];
        // time out
        if (peer.lastTime > time + 6f) {
          peerList.RemoveAt(i);
          Console.WriteLine($"peer{i} timed out");
        }
        
        for (int j = 0; j < peerList.Count; j++)
        {
          Peer peer2 = peerList[j];
          // if (peer.endPoint == peer2.endPoint) {
          //   continue;
          // }
          // send data to peer
          socket.SendTo(peer.data, IPEndPoint.Parse(peer2.endPoint));
        }
      }

      Thread.Sleep(10);
    }
  }
}