using UnityEngine;
using System.Collections;

public class LaserBehavior : MonoBehaviour
{
    private Transform ThisTransform;

    private Vector3 VectorFront = Vector3.forward;
    private Vector3 VectorZero = Vector3.zero;
    private float laserSpeed = 0.05f;

    public Transform playerTransform = null;

	void Awake()
    {
        ThisTransform = GetComponent<Transform>();

        if (Application.isEditor)
        {
            laserSpeed = 0.05f;
        }
        else
        {
            laserSpeed = 0.25f;
        }
    }

    void OnEnable()
    {
        if (playerTransform == null)
        {
            return;
        }

        ThisTransform.position = playerTransform.position;
        ThisTransform.eulerAngles = Camera.main.transform.eulerAngles;
        StartCoroutine(WaitForDisable());
    }

    private IEnumerator WaitForDisable()
    {
        yield return new WaitForSeconds(2.5f);
        gameObject.SetActive(false);
    }

    void Update () {
        ThisTransform.Translate(VectorFront * laserSpeed);
    }

}
