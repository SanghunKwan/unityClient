using System.Runtime.InteropServices;

namespace NetworkDefine
{
    #region [패킷구조체]
    [StructLayout(LayoutKind.Sequential)]
    public struct Packet
    {
        [MarshalAs(UnmanagedType.U4)]
        public uint _protocol;
        [MarshalAs(UnmanagedType.U4)]
        public uint _totalSize;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1016)]
        public byte[] _data;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct Packet_UserData
    {
        [MarshalAs(UnmanagedType.U8)]
        public ulong _uuid;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string _id;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 30)]
        public string _pw;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string _name;
        [MarshalAs(UnmanagedType.U4)]
        public uint _clearStage;
        [MarshalAs(UnmanagedType.U8)]
        public ulong _gold;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct Packet_DuplicationId
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string _id;
    }
    #endregion [패킷구조체]

    #region [Client프로토콜]
    public class CProtocol
    {
        public enum Send
        {
            Join                            = 200,
            Login,
            CheckIdDuplication,


            End
        }
        public enum Receive
        {
            Connect_Success                 = 200,
            Join_Success,
            Join_Failed,

            Login_Success,
            Login_Failed,

            Duplication_true,
            Duplication_false,
        }
    }
    #endregion [Client프로토콜]
}
