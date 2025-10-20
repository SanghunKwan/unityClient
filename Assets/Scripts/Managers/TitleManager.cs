using UnityEngine;

public class TitleManager : MonoBehaviour
{
    static TitleManager _uniqueInstance;

    const string path = "Prefabs/";
    UILoginWnd _uiLoginWnd;
    UIMessageBox _uiMessageBox;


    public static TitleManager _instance => _uniqueInstance;

    private void Awake()
    {
        _uniqueInstance = this;
    }

    //юс╫ц
    private void Start()
    {
        InitTitle();
        InitMessageBox();
    }
    //==

    public void InitTitle()
    {
        GameObject _uiPrefab = Resources.Load<GameObject>(path + "LoginWindow");

        GameObject go = Instantiate(_uiPrefab);
        _uiLoginWnd = go.GetComponent<UILoginWnd>();
        _uiLoginWnd.OpenWnd();
    }

    public void OnDuplicationCheck(bool isOn)
    {
        _uiLoginWnd.OnDuplicationCheck(isOn);
    }

    public void InitMessageBox()
    {
        GameObject _uiPrefab = Resources.Load<GameObject>(path + "MessageBox");

        GameObject go = Instantiate(_uiPrefab, _uiLoginWnd.transform);
        _uiMessageBox = go.GetComponent<UIMessageBox>();
        _uiMessageBox.InitMessageBox();
    }

    public void LoginSuccess()
    {
        _uiLoginWnd.Login();
    }
    public void PrintMessage(in string message)
    {

    }
}
