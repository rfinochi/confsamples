using UnityEngine;
using UnityEngine.UI;

public class CockpitHud : MonoBehaviour
{
    private Canvas ThisCanvas;

    [SerializeField]
    private RectTransform hudImage;
	
    void Awake()
    {
        ThisCanvas = GetComponent<Canvas>();
    }

	void Start () {
        if (ThisCanvas.worldCamera == null)
        {
            ThisCanvas.worldCamera = Camera.main;
        }

        if (!Application.isEditor)
        {
            hudImage.localScale = new Vector3(0.5f, 1.0f, 1.0f);
        }
	}
}
