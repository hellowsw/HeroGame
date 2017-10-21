BitAnd = LuaCustom.BitOp32.And
BitOr = LuaCustom.BitOp32.Or
BitNot = LuaCustom.BitOp32.Not
BitLShift = LuaCustom.BitOp32.LShift
BitRShift = LuaCustom.BitOp32.RShift
GameObject = UnityEngine.GameObject
PlayerPrefs = UnityEngine.PlayerPrefs
AccountServer = UnityDLL.AccountServer
GameServer = UnityDLL.GameServer
LYBGlobalConsts = Network.DataDefs.LYBGlobalConsts
BitOp32 = LuaCustom.BitOp32
LYBStreamIn = Network.Net.LYBStreamIn
LYBStreamOut = Network.Net.LYBStreamOut
TBinaryData = Network.Net.TBinaryData
SPMPS = Network.DataDefs.NetMsgDef.SPMPS
SCMPS = Network.DataDefs.NetMsgDef.SCMPS
Input = UnityEngine.Input
Screen = UnityEngine.Screen
GameConst = GameCommon.GameConst
ObjectManager = Manager.ObjectManager
CommonLib = GameCommon.CommonLib
InputState = GameCommon.InputState
DOTweenUtil = UnityDLL.GameLogic.Common.DOTweenUtil

_load_sprite = function(path, name)
    return ResourceManager.LoadSpriteFromResources(path, name)
end

_load_resource = function(path, name)
    return ResourceManager.LoadResourceFromLocal(path, name)
end

_instantiate = function(path, name)
    return ResourceManager.InstantiateFromResources(path, name)
end