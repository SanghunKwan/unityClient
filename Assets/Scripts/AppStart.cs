using UnityEngine;
using UnityEngine.SceneManagement;

public class AppStart : MonoBehaviour
{
    private void Awake()
    {
        //�̰����� ���ø����̼� ���� ����.
        NetworkMain._instance.NetStart();
    }

    private void Start()
    {
        SceneManager.LoadScene(1);
    }
}
