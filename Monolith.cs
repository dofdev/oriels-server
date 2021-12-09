using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

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
Console.WriteLine("oriels server now booting up...");
int current = 0;

Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
EndPoint sender = new IPEndPoint(IPAddress.Any, 0);
// socket.SendTimeout = 1000;
// socket.ReceiveTimeout = 1000;
socket.Bind(new IPEndPoint(IPAddress.Any, 1234));
Console.WriteLine("socket bound on port: 1234");

// peer data is temporary, blockchain+ipfs is forever
Peer[] peers = new Peer[64];

Thread writeThread = new Thread(Write);
writeThread.Priority = ThreadPriority.BelowNormal;
writeThread.Start();
void Write() {
  bool running = true;
  while (running) {
    for (int i = 0; i < peers.Length; i++) {
      Peer peer = peers[i];
      if (peer != null) {
        for (int j = 0; j < peers.Length; j++) {
          Peer peer2 = peers[j];
          if (peer2 != null && i != j) {
            socket.SendTo(peer2.data, IPEndPoint.Parse(peer.endPoint));
          }
        }
      }
    }

    Thread.Sleep(60);
  }
}

Thread readThread = new Thread(Read);
readThread.Priority = ThreadPriority.BelowNormal;
readThread.Start();
void Read() {
  bool running = true;
  while (running) {
    // clean up peers
    for (int i = 0; i < peers.Length; i++) {
      if (peers[i] != null && peers[i].time + 6 < time) {
        peers[i] = null;
        Console.WriteLine($"--peer | current: {--current}");
      }
    }

    int bufferSize = 1024;
    byte[] data = new byte[bufferSize]; 
    while (socket.Available > 0) {
      int index = 0;
      try {
        // sender = new IPEndPoint(IPAddress.Any, 0);
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
            Console.WriteLine($"++peer | current: {++current}");
            break;
          }
        }
      } catch (Exception e) {
        Console.WriteLine(e.Message);
        peers[index] = null;
        Console.WriteLine($"!!error | current: {--current}");
        break;
      }
    }

    // Thread.Sleep(60);
    Thread.CurrentThread.Priority = ThreadPriority.Lowest;
  }
}

Console.ReadLine();

socket.Close();
Console.WriteLine("shutdown");

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