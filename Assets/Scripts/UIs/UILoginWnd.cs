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

    [SerializeField] Text _duplicationIdText;
    [SerializeField] Text _duplicationPwText;
    [SerializeField] Text _duplicationNameText;


    readonly Color[] _duplicationColor = new Color[3] { Color.gray, Color.cyan, Color.magenta };
    readonly string[] _duplicationMessage = new string[2] { "사용할 수 있는 아이디입니다.", "이미 존재하는 아이디입니다." };
    readonly string[] _duplicationErrorMessage = new string[6] { "중복 확인을 해야 합니다.", "1글자 이상, 10글자 미만으로 입력해야 합니다.", "영어와 숫자만 사용해야 합니다.",
                                                                 string.Empty, "1글자 이상, 45글자 미만으로 입력해야 합니다." ,
                                                                "1글자 이상, 20글자 미만으로 입력해야 합니다."};
    bool _readyToCheckDuplication;
    bool[] _usableJInput = new bool[3];

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
                _joinBox.SetActive(false);
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
        _inputJID.onValueChanged.AddListener((_) => OnRequireDuplication());
        _inputJPW.onValueChanged.AddListener((pw) => OnPasswordInputChanged(pw));
        _inputJName.onValueChanged.AddListener((userName) => OnNameInputChange(userName));
    }
    void OnRequireDuplication()
    {
        _usableJInput[0] = false;
        bool isValid = IsIdValid(_inputJID.text, out uint error);
        _duplicationIdText.color = _duplicationColor[isValid ? 0 : 2];
        _readyToCheckDuplication = isValid;
        _duplicationIdText.text = _duplicationErrorMessage[error];
    }
    void OnPasswordInputChanged(in string pw)
    {
        bool isValid = IsPwValid(pw, out uint error);
        _usableJInput[1] = isValid;
        _duplicationPwText.text = isValid ? string.Empty : _duplicationErrorMessage[error];
    }
    void OnNameInputChange(in string name)
    {
        bool isValid = IsNameValid(name, out uint error);
        _usableJInput[2] = isValid;
        _duplicationNameText.text = isValid ? string.Empty : _duplicationErrorMessage[error];
    }
    public void OnDuplicationCheck(bool isOn)
    {
        _usableJInput[0] = isOn;
        if (isOn)
        {
            _duplicationIdText.text = _duplicationMessage[0];
            _duplicationIdText.color = _duplicationColor[1];
        }
        else
        {
            _duplicationIdText.text = _duplicationMessage[1];
            _duplicationIdText.color = _duplicationColor[2];
        }
    }
    public bool IsIdValid(in string userID, out uint error)
    {
        if (userID.Length <= 0 || userID.Length > 10)
        {
            error = 1;
            return false;
        }

        for (int i = 0; i < userID.Length; i++)
        {
            if (!char.IsLetterOrDigit(userID[i]))
            {
                error = 2;
                return false;
            }
        }

        error = 0;
        return true;
    }
    public bool IsPwValid(in string userPw, out uint error)
    {
        if (userPw.Length <= 0 || userPw.Length > 45)
        {
            error = 4;
            return false;
        }

        error = 0;
        return true;
    }
    public bool IsNameValid(in string userName, out uint error)
    {
        if (userName.Length <= 0 || userName.Length > 20)
        {
            error = 5;
            return false;
        }

        error = 0;
        return true;
    }

    
    public void Login()
    {
        SettingSwitch(LoginStep.Login);
        Debug.Log("로그인 성공");
    }

    #region [외부 호출 함수]
    public void ClickConnectButton()
    {
        NetworkMain._instance.Connect();
        SettingSwitch(LoginStep.Connect);
    }
    public void ClickLoginButton()
    {
        Packet_Login packetLogin;
        packetLogin._id = _inputID.text;
        packetLogin._pw = _inputPW.text;

        byte[] data = ConverterPack.StructureToByteArray(packetLogin);
        Packet pack = ConverterPack.CreatePack((uint)CProtocol.Send.Login, (uint)data.Length, data);

        NetworkMain._instance.SendQueueIn(pack);
    }
    public void ClickJoinButton()
    {
        SettingSwitch(LoginStep.Join);
    }
    public void ClickDuplicationButton()
    {
        if (!_readyToCheckDuplication) return;

        Packet_DuplicationId pack_duple;
        pack_duple._id = _inputJID.text;

        byte[] data = ConverterPack.StructureToByteArray(pack_duple);
        Packet pack = ConverterPack.CreatePack((uint)CProtocol.Send.CheckIdDuplication, (uint)data.Length, data);

        NetworkMain._instance.SendQueueIn(pack);
    }
    public void ClickInputIdField()
    {
        if (_inputJID.text != string.Empty) return;
        OnRequireDuplication();
    }
    public void ClickInputPwField()
    {
        OnPasswordInputChanged(_inputJPW.text);
    }
    public void ClickInputNameField()
    {
        OnNameInputChange(_inputJName.text);
    }
    public void ClickJoinQueryButton()
    {
        if (_usableJInput[0] && _usableJInput[1] && _usableJInput[2])
        {
            Packet_UserData userData;
            userData._id = _inputJID.text;
            userData._name = _inputJName.text;
            userData._pw = _inputJPW.text;

            byte[] data = ConverterPack.StructureToByteArray(userData);
            Packet pack = ConverterPack.CreatePack((uint)CProtocol.Send.Join, (uint)data.Length, data);

            NetworkMain._instance.SendQueueIn(pack);

            SettingSwitch(LoginStep.Connect);
        }
        else
        {

        }
    }

    #endregion [외부 호출 함수]


}
