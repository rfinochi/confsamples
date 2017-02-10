using UnityEngine;
using System.Collections;

public class AnimEvent_StartCounterUI : MonoBehaviour
{
    public void StartGame()
    {
        MainMenu.Instance.OnAnimationFinish();
    }
}
