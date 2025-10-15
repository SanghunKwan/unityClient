using NetworkDefine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class NetworkMain : TSingleton<NetworkMain>
{
    const int _serverPort = 789;
    const string _serverIP = "127.0.0.1";

    Socket _serverSocket;
    Queue<Packet> _sendQueue;
    Queue<Packet> _receiveQueue;

    public bool _isQuit { get; set; }

    protected override void Init()
    {
        base.Init();
        _sendQueue = new Queue<Packet>();
        _receiveQueue = new Queue<Packet>();
        _isQuit = false;

        StartCoroutine(SendLoop());
        StartCoroutine(ReceiveLoop());
    }

    private void Update()
    {
        if (_serverSocket != null && _serverSocket.Poll(0, SelectMode.SelectRead))
        {
            byte[] buffer = new byte[1024];
            int receiveLength = _serverSocket.Receive(buffer);

            if (receiveLength > 0)
            {
                Packet pack = (Packet)ConverterPack.ByteArrayToStructure(buffer, typeof(Packet), receiveLength);

                _receiveQueue.Enqueue(pack);
            }

        }

    }

    public void Connect()
    {
        try
        {
            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _serverSocket.Connect(_serverIP, _serverPort);
        }
        catch (System.Exception ex)
        {
            Debug.LogFormat("坷幅 : {0}", ex.Message);
        }
    }

    #region [内风凭]
    IEnumerator SendLoop()
    {
        while (!_isQuit)
        {
            if (_sendQueue.Count > 0)
            {
                Packet pack = _sendQueue.Dequeue();
                byte[] data = ConverterPack.StructureToByteArray(pack);

                _serverSocket.Send(data);
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

                switch((CProtocol.Receive)pack._protocol)
                {
                    case CProtocol.Receive:

                        break;

                }

            }
            yield return null;
        }
    }
    #endregion [内风凭]
}
