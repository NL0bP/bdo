using System;
using BDCommon.Network;

namespace BDCommon.Structures.Player
{
   [Serializable, ProtoBuf.ProtoContract]
   public class Player
    {
         [ProtoBuf.ProtoMember(1)]
         public int AccountId;
         [ProtoBuf.ProtoMember(2)]
         public int PlayerId;
         [ProtoBuf.ProtoMember(3)]
         public CharacterData CharacterData;
         [ProtoBuf.ProtoMember(4)]
         public int Level;

         public IConnection Connection { get; set; }
    }
}
