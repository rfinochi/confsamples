using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance;

    private Animator ThisAnimator;

    [SerializeField]
    private PlayerManager playerManager;
    [SerializeField]
    private EnemyManager[] enemyManagerList = new EnemyManager[3];

    private bool startGame = false;

    private void Awake()
    {
        Instance = this;

        ThisAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        ThisAnimator.enabled = false;

        playerManager.canShoot = false;

        for (var i = 0; i < enemyManagerList.Length; i++)
            enemyManagerList[i].canSpawn = false;
    }
	
    public void InitGame()
    {
        if (startGame)
            return;

        startGame = true;

        ThisAnimator.enabled = true;
    }

    public void OnAnimationFinish()
    {
        playerManager.canShoot = true;
            
        for (var i = 0; i < enemyManagerList.Length; i++)
            enemyManagerList[i].canSpawn = true;

        TargetCursor.Instance.cursorAnimate.SetTrigger("Init");
        gameObject.SetActive(false); 
    }

}
