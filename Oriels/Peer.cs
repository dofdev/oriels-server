// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace Oriels
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct Peer : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_1_12_0(); }
  public static Peer GetRootAsPeer(ByteBuffer _bb) { return GetRootAsPeer(_bb, new Peer()); }
  public static Peer GetRootAsPeer(ByteBuffer _bb, Peer obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public Peer __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public Oriels.Pose? Head { get { int o = __p.__offset(4); return o != 0 ? (Oriels.Pose?)(new Oriels.Pose()).__assign(o + __p.bb_pos, __p.bb) : null; } }
  public Oriels.Pose? LHand { get { int o = __p.__offset(6); return o != 0 ? (Oriels.Pose?)(new Oriels.Pose()).__assign(o + __p.bb_pos, __p.bb) : null; } }
  public Oriels.Pose? RHand { get { int o = __p.__offset(8); return o != 0 ? (Oriels.Pose?)(new Oriels.Pose()).__assign(o + __p.bb_pos, __p.bb) : null; } }
  public Oriels.Vec3? Cursor { get { int o = __p.__offset(10); return o != 0 ? (Oriels.Vec3?)(new Oriels.Vec3()).__assign(o + __p.bb_pos, __p.bb) : null; } }

  public static void StartPeer(FlatBufferBuilder builder) { builder.StartTable(4); }
  public static void AddHead(FlatBufferBuilder builder, Offset<Oriels.Pose> headOffset) { builder.AddStruct(0, headOffset.Value, 0); }
  public static void AddLHand(FlatBufferBuilder builder, Offset<Oriels.Pose> lHandOffset) { builder.AddStruct(1, lHandOffset.Value, 0); }
  public static void AddRHand(FlatBufferBuilder builder, Offset<Oriels.Pose> rHandOffset) { builder.AddStruct(2, rHandOffset.Value, 0); }
  public static void AddCursor(FlatBufferBuilder builder, Offset<Oriels.Vec3> cursorOffset) { builder.AddStruct(3, cursorOffset.Value, 0); }
  public static Offset<Oriels.Peer> EndPeer(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<Oriels.Peer>(o);
  }
};


}
