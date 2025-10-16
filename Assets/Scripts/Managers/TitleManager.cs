using UnityEngine;

public class TitleManager : MonoBehaviour
{
    static TitleManager _uniqueInstance;

    UILoginWnd _uiLoginWnd;


    public static TitleManager _instance => _uniqueInstance;

    private void Awake()
    {
        _uniqueInstance = this;
    }

    //юс╫ц
    private void Start()
    {
        InitTitle();
    }
    //==

    public void InitTitle()
    {
        string path = "Prefabs/";
        GameObject _uiPrefab = Resources.Load<GameObject>(path + "LoginWindow");

        GameObject go = Instantiate(_uiPrefab);
        _uiLoginWnd = go.GetComponent<UILoginWnd>();
        _uiLoginWnd.OpenWnd();
    }

}
