using UnityEngine;
using UnityEngine.UI;

public class TargetCursor : MonoBehaviour
{
    public static TargetCursor Instance;

    [SerializeField]
    private Canvas canvas;

    public Animator cursorAnimate;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        canvas.worldCamera = Camera.main;
        canvas.planeDistance = 2;
    }
}
