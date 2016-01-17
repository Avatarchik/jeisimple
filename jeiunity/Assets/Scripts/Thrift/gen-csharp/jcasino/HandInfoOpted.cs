/**
 * Autogenerated by Thrift Compiler (0.9.2)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Thrift;
using Thrift.Collections;
using System.Runtime.Serialization;
using Thrift.Protocol;
using Thrift.Transport;

namespace jcasino
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class HandInfoOpted : TBase
  {
    private byte[] _playerHand;

    public byte[] PlayerHand
    {
      get
      {
        return _playerHand;
      }
      set
      {
        __isset.playerHand = true;
        this._playerHand = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool playerHand;
    }

    public HandInfoOpted() {
    }

    public void Read (TProtocol iprot)
    {
      TField field;
      iprot.ReadStructBegin();
      while (true)
      {
        field = iprot.ReadFieldBegin();
        if (field.Type == TType.Stop) { 
          break;
        }
        switch (field.ID)
        {
          case 1:
            if (field.Type == TType.String) {
              PlayerHand = iprot.ReadBinary();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          default: 
            TProtocolUtil.Skip(iprot, field.Type);
            break;
        }
        iprot.ReadFieldEnd();
      }
      iprot.ReadStructEnd();
    }

    public void Write(TProtocol oprot) {
      TStruct struc = new TStruct("HandInfoOpted");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (PlayerHand != null && __isset.playerHand) {
        field.Name = "playerHand";
        field.Type = TType.String;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteBinary(PlayerHand);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder __sb = new StringBuilder("HandInfoOpted(");
      bool __first = true;
      if (PlayerHand != null && __isset.playerHand) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("PlayerHand: ");
        __sb.Append(PlayerHand);
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}