using UnityEngine;
using System.Collections;

public class PlanetRotation : MonoBehaviour
{
    [SerializeField]
    private float rotateSpeed = 0.2f;

    private Transform ThisTransform;
	
    void Start()
    {
        ThisTransform = GetComponent<Transform>();
    }

	void Update()
    {
        ThisTransform.Rotate(Vector3.up * (Time.deltaTime * rotateSpeed));
    }
}
