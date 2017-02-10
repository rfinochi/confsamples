using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity;
using UnityEngine.VR.WSA.Input;
using System;

public class PlayerManager : MonoBehaviour
{
    //PLAYER SETTING
    [SerializeField]
    private Transform playerPosition;
    [HideInInspector]
    public Vector3 playerPositionWorld;

    //LASER SETTING
    [SerializeField]
    private int maxLasersCount;
    [SerializeField]
    private GameObject laserPrefab;
    public Transform lasersContainer;
    [SerializeField]
    private GameObject[] poolingLasersList = null;
    [SerializeField]
    private AudioSource laserSound;
    [SerializeField]
    private AudioClip laserSoundClip;

    private GestureManager gestureManager;

    public bool canShoot = false;

    private void Start()
    {
        playerPositionWorld = playerPosition.transform.position;

        CreatePoolingLasers();
    }

    private void CreatePoolingLasers()
    {
        poolingLasersList = new GameObject[maxLasersCount];

        for (var i = 0; i < maxLasersCount; i++)
        {
            var laserCreated = Instantiate(laserPrefab) as GameObject;
            laserCreated.name = "Laser";
            laserCreated.transform.SetParent(lasersContainer, true);

            var laserBehavior = laserCreated.GetComponent<LaserBehavior>();
            laserBehavior.playerTransform = playerPosition;

            poolingLasersList[i] = laserCreated;
            laserCreated.SetActive(false);
        }
    }

    private void Update()
    {
        playerPositionWorld = playerPosition.transform.position;

        if (gestureManager == null)
            if (GestureManager.Instance.gestureRecognizer.IsCapturingGestures())
            {
                gestureManager = GestureManager.Instance;
                gestureManager.gestureRecognizer.TappedEvent += GestureRecognizer_TappedEvent;
            }

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            PlayerShoot();
    }

    private void GestureRecognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        PlayerShoot();
    }

    private void PlayerShoot()
    {
        if (canShoot)
            for (var i = 0; i < maxLasersCount; i++)
                if (poolingLasersList[i].activeSelf == false)
                {
                    poolingLasersList[i].SetActive(true);
                    laserSound.PlayOneShot(laserSoundClip);
                    return;
                }
    }
}
