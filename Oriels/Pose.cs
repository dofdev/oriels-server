// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace Oriels
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct Pose : IFlatbufferObject
{
  private Struct __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public void __init(int _i, ByteBuffer _bb) { __p = new Struct(_i, _bb); }
  public Pose __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public Oriels.Vec3 Pos { get { return (new Oriels.Vec3()).__assign(__p.bb_pos + 0, __p.bb); } }
  public Oriels.Quat Rot { get { return (new Oriels.Quat()).__assign(__p.bb_pos + 12, __p.bb); } }

  public static Offset<Oriels.Pose> CreatePose(FlatBufferBuilder builder, float pos_X, float pos_Y, float pos_Z, float rot_X, float rot_Y, float rot_Z, float rot_W) {
    builder.Prep(4, 28);
    builder.Prep(4, 16);
    builder.PutFloat(rot_W);
    builder.PutFloat(rot_Z);
    builder.PutFloat(rot_Y);
    builder.PutFloat(rot_X);
    builder.Prep(4, 12);
    builder.PutFloat(pos_Z);
    builder.PutFloat(pos_Y);
    builder.PutFloat(pos_X);
    return new Offset<Oriels.Pose>(builder.Offset);
  }
};


}
