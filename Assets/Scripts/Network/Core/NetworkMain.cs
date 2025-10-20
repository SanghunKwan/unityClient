using NetworkDefine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class NetworkMain : TSingleton<NetworkMain>
{
    const int _serverPort = 666;
    const string _serverIP = "127.0.0.1";

    TcpClient _serverTcp;
    //Socket _serverSocket;
    Queue<Packet> _sendQueue;
    Queue<Packet> _receiveQueue;

    public bool _isQuit { get; set; }

    protected override void Init()
    {
        base.Init();
        _sendQueue = new Queue<Packet>();
        _receiveQueue = new Queue<Packet>();
        _isQuit = false;
    }

    private void Update()
    {
        //if (_serverSocket != null && _serverSocket.Poll(0, SelectMode.SelectRead))
        if (_serverTcp != null && _serverTcp.Connected)
        {
            NetworkStream stream = _serverTcp.GetStream();
            int receiveLength;
            byte[] buffer = new byte[1024];

            if (stream != null && stream.DataAvailable && (receiveLength = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                Packet pack = (Packet)ConverterPack.ByteArrayToStructure(buffer, typeof(Packet), receiveLength);

                _receiveQueue.Enqueue(pack);
            }


            //int receivelength = _serverTcp.Client.Receive(buffer);
            ////int receivelength = _serverSocket.Receive(buffer);
            //if (receivelength > 0)
            //{
            //    Packet pack = (Packet)ConverterPack.ByteArrayToStructure(buffer, typeof(Packet), receivelength);

            //    _receiveQueue.Enqueue(pack);
            //}

        }
    }

    public void NetStart()
    {
        StartCoroutine(SendLoop());
        StartCoroutine(ReceiveLoop());
        //StartCoroutine(ReceiveLoopTcpClient());
    }

    public void Connect()
    {
        try
        {
            //_serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //_serverSocket.Connect(_serverIP, _serverPort);
            IPEndPoint clientAddr = new IPEndPoint(IPAddress.Parse(_serverIP), 999);
            IPEndPoint serverAddr = new IPEndPoint(IPAddress.Parse(_serverIP), _serverPort);

            _serverTcp = new TcpClient(clientAddr);
            _serverTcp.Connect(serverAddr);
        }
        catch (System.Exception ex)
        {
            Debug.LogFormat("오류 : {0}", ex.Message);
        }
    }

    #region [코루틴]
    IEnumerator SendLoop()
    {
        while (!_isQuit)
        {
            if (_sendQueue.Count > 0)
            {
                Packet pack = _sendQueue.Dequeue();
                byte[] data = ConverterPack.StructureToByteArray(pack);

                _serverTcp.Client.Send(data);
                //_serverSocket.Send(data);
            }
            yield return null;
        }
    }
    IEnumerator ReceiveLoop()
    {
        while (!_isQuit)
        {
            if (_receiveQueue.Count > 0)
            {
                Packet pack = _receiveQueue.Dequeue();

                switch ((CProtocol.Receive)pack._protocol)
                {
                    case CProtocol.Receive.Connect_Success:
                        Debug.Log("연결 성공");
                        break;
                    case CProtocol.Receive.Join_Success:
                        Debug.Log("가입 성공");
                        break;
                    case CProtocol.Receive.Join_Failed:
                        Debug.Log("가입 실패");
                        break;
                    case CProtocol.Receive.Login_Success:
                        TitleManager._instance.LoginSuccess();
                        break;
                    case CProtocol.Receive.Login_Failed:
                        Debug.Log("로그인 실패");
                        break;
                    case CProtocol.Receive.Duplication_True:
                        TitleManager._instance.OnDuplicationCheck(false);
                        break;
                    case CProtocol.Receive.Duplication_False:
                        TitleManager._instance.OnDuplicationCheck(true);
                        break;
                }

            }
            yield return null;
        }
    }
    IEnumerator ReceiveLoopTcpClient()
    {
        while (!_isQuit)
        {
            if (_serverTcp != null && _serverTcp.Connected)
            {
                byte[] buffer = new byte[1024];

                int receivelength = _serverTcp.Client.Receive(buffer);
                Debug.Log("리시브");
                //int receivelength = _serverSocket.Receive(buffer);
                if (receivelength > 0)
                {
                    Packet pack = (Packet)ConverterPack.ByteArrayToStructure(buffer, typeof(Packet), receivelength);

                    _receiveQueue.Enqueue(pack);
                }

            }
            yield return null;
        }
    }
    #endregion [코루틴]

    public void SendQueueIn(Packet packet)
    {
        _sendQueue.Enqueue(packet);
    }
}
