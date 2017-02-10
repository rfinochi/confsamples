using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour
{
    private Transform ThisTransform;
    private MeshRenderer ThisMeshRenderer;
    private BoxCollider ThisCollider;

    public float enemySpeed = 0.01f;
    private Vector3 VectorFront = Vector3.forward;
    private GameObject explosion;

    private bool canMove = true;

    public Transform playerTransform = null;

    void Awake()
    {
        ThisTransform = GetComponent<Transform>();
        ThisMeshRenderer = GetComponent<MeshRenderer>();
        ThisCollider = GetComponent<BoxCollider>();
    }

    void Start ()
    {
        explosion = ThisTransform.GetChild(0).gameObject;
        explosion.SetActive(false);
    }

    void OnEnable()
    {
        ThisCollider.enabled = true;
        ThisMeshRenderer.enabled = true;
        canMove = true;
    } 

    void Update ()
    {
        if (playerTransform == null)
        {
            return;
        }

        if (canMove)
        {
            ThisTransform.LookAt(playerTransform.position);
            ThisTransform.Translate(VectorFront * enemySpeed);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "Laser")
        {
            collision.gameObject.SetActive(false);
        }

        ThisCollider.enabled = false;
        ThisMeshRenderer.enabled = false;
        explosion.SetActive(true);
        canMove = false;

        StartCoroutine(WaitForExplosionEnd());
    }

    private IEnumerator WaitForExplosionEnd()
    {
        yield return new WaitForSeconds(2.0f);
        explosion.SetActive(false);
        gameObject.SetActive(false);
    }
}
