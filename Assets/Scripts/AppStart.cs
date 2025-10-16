using UnityEngine;
using UnityEngine.SceneManagement;

public class AppStart : MonoBehaviour
{
    private void Awake()
    {
        //이곳에서 어플리케이션 최초 실행.
        NetworkMain._instance.NetStart();
    }

    private void Start()
    {
        SceneManager.LoadScene(1);
    }
}
