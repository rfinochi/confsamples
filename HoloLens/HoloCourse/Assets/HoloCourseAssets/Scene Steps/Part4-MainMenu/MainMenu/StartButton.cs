using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartButton : MonoBehaviour
{
    [SerializeField]
    private Image backgroundButton;

    [SerializeField]
    private Color normalColor;

    [SerializeField]
    private Color overColor;

    public void OnGazeEnter()
    {
        backgroundButton.color = overColor;
    }

    public void OnGazeLeave()
    {
        backgroundButton.color = normalColor;
    }

    public void OnSelect()
    {
        transform.parent.GetComponent<MainMenu>().InitGame();
    }
}
