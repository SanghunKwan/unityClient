using NetworkDefine;
using UnityEngine;
using UnityEngine.UI;
using UtilGameDefine;

public class UILoginWnd : MonoBehaviour
{
    [Header("참조 Obj")]
    [SerializeField] GameObject _connectBtn;
    [SerializeField] GameObject _accountBox;
    [SerializeField] GameObject _joinBox;

    [Header("연결 자료")]
    [SerializeField] InputField _inputID;
    [SerializeField] InputField _inputPW;

    [SerializeField] InputField _inputJID;
    [SerializeField] InputField _inputJPW;
    [SerializeField] InputField _inputJName;


    void SettingSwitch(LoginStep step)
    {
        switch (step)
        {
            case LoginStep.None:
                _connectBtn.SetActive(true);
                _accountBox.SetActive(false);
                _joinBox.SetActive(false);
                break;
            case LoginStep.Connect:
                _connectBtn.SetActive(false);
                _accountBox.SetActive(true);
                break;
            case LoginStep.Login:
                _accountBox.SetActive(false);
                break;
            case LoginStep.Join:
                _joinBox.SetActive(true);
                break;
        }
    }
    public void OpenWnd()
    {
        SettingSwitch(LoginStep.None);
    }

    #region [외부 호출 함수]
    public void ClickConnectButton()
    {
        NetworkMain._instance.Connect();
        SettingSwitch(LoginStep.Connect);
    }
    public void ClickLoginButton()
    {
        SettingSwitch(LoginStep.Login);
    }
    public void ClickJoinButton()
    {
        SettingSwitch(LoginStep.Join);
    }
    public void ClickDuplicationButton()
    {
        Packet_DuplicationId pack_duple;
        pack_duple._id = _inputJID.text;

        byte[] data = ConverterPack.StructureToByteArray(pack_duple);
        Packet pack = ConverterPack.CreatePack((uint)CProtocol.Send.CheckIdDuplication, (uint)data.Length, data);

        NetworkMain._instance.SendQueueIn(pack);
    }
    #endregion [외부 호출 함수]
}
