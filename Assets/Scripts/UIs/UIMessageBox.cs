using UnityEngine;
using UnityEngine.UI;

public class UIMessageBox : MonoBehaviour
{
    Text _messageText;


    public void InitMessageBox()
    {

    }

    public void SetMessage(in string message, in Color color)
    {
        _messageText.text = message;
        _messageText.color = color;
    }
    public void ClickBackground()
    {

    }
    public void ClickCloseButton()
    {

    }
}
