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
    #endregion [패킷구조체]

    #region [Client프로토콜]
    public class CProtocol
    {
        public enum Send
        {
            End
        }
        public enum Receive
        {
        }
    }
    #endregion [Client프로토콜]
}
