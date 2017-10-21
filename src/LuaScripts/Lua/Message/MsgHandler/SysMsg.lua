--local BitAnd = BitOp32.And
--local BitOr = BitOp32.Or
--local BitNot = BitOp32.Not
--local BitLShift = BitOp32.LShift
--local BitRShift = BitOp32.RShift
--local LYBGlobalConsts = LYBGlobalConsts
local NpcData = NpcData
local NetMsg = NetMsg
local GetWriteStream = GameServer.GetMsgWriteStream

NetMsg.SAChDataMsg =
{
    Read = function(self, stream)
        self.data_ = LocalPlayerData.Read(stream)
        return self
    end
}

-- C++ RPC
NetMsg.SAClientRPCOPMsg = 
{
    Read = function(self, stream)
        self.type_ = stream:ReadShort()
        self.rpc_stream_ = stream:ReadRpcStreamAlign4B()
        return self
    end
}




NetMsg.SQClientRPCOPMsg =
{
    Write = function(self)
		local stream = GetWriteStream()
       stream:WriteByte(SPMPS.EPRO_SYSTEM_MESSAGE)
      stream:WriteByte(SCMPS.EPRO_CLIENT_REQUEST_RPCOP)

        stream:WriteShort(self.type_)
        MsgWriteBuffer(stream, self.rpc_stream_, self.rpc_stream_.Size)
    
        return stream
    end
}

NetMsg.SABackMsg = 
{
    Read = function(self, stream)
        self.type_ = stream:ReadUShort()
        return self
    end
}
