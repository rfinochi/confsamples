using UnityEngine;
using System.Collections;

public class CameraMoveAndRotation : MonoBehaviour {

    [SerializeField]
    private Transform ThisTransform;

    public float minY = -45.0f;
    public float maxY = 45.0f;

    public float sensX = 100.0f;
    public float sensY = 100.0f;

    float rotationY = 0.0f;
    float rotationX = 0.0f;

    private float zoomSpeed = 2.0f;

    private bool canCameraRotate = false;

    [SerializeField]
    public float moveSpeed = 2.0f;

    void Awake()
    {
        ThisTransform = GetComponent<Transform>();
    }

	void Update () {
        if (Input.GetMouseButtonDown(1))
        {
            canCameraRotate = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            canCameraRotate = false;
        }

        RotateCamera();
        //ZoomCamera();
        MoveCamera();
    }

    private void RotateCamera()
    {
        if (canCameraRotate)
        {
            rotationX += Input.GetAxis("Mouse X") * sensX * Time.deltaTime;
            rotationY += Input.GetAxis("Mouse Y") * sensY * Time.deltaTime;
            rotationY = Mathf.Clamp(rotationY, minY, maxY);
            ThisTransform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);

        }
    }

    private void ZoomCamera()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        transform.Translate(0, scroll * zoomSpeed, scroll * zoomSpeed, Space.World);
    }

    private void MoveCamera()
    {
        if (Input.GetKey(KeyCode.W))
        {
            ThisTransform.Translate((Vector3.forward * Time.deltaTime) * moveSpeed);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            ThisTransform.Translate((Vector3.back * Time.deltaTime) * moveSpeed);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            ThisTransform.Translate((Vector3.left * Time.deltaTime) * moveSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            ThisTransform.Translate((Vector3.right * Time.deltaTime) * moveSpeed);
        }
    }
}
