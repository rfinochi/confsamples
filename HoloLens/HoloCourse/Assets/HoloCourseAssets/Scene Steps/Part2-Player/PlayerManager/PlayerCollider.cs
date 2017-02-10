using UnityEngine;
using System.Collections;

public class PlayerCollider : MonoBehaviour
{
    [SerializeField]
    private Animator playerDamageFX;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name.IndexOf("Enemy") != -1)
            playerDamageFX.SetTrigger("Hit");
    }
}
