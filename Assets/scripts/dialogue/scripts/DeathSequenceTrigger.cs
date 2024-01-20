using action_triggers.scripts;
using observer.scripts;
using UnityEngine;

public class DeathSequenceTrigger : PlayerActionsObserverSubject
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject enemy;
    [SerializeField] private GameObject pistol;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private TimeController timeController;
    
    [SerializeField] private GameObject deathSequenceStartDialogue;
    [SerializeField] private GameObject deathSequenceEndDialogue;
    [SerializeField] private GameObject currentSceneDialogue;
    [SerializeField] private int triggerAfterSeconds = 20;
    private Animator _playerAnimator;
    private static readonly int IsDead = Animator.StringToHash("IsDead");

    private void Awake()
    {
        _playerAnimator = player.GetComponent<Animator>();

        if (IsDeathSequenceEnded())
            return;
        
        if (GetIsSeenUniverseDeathSequence())
        {
            PositionPistolOnEnemy();
            SetPlayerDeathAnimation();
            ActivateEndSequenceEndDialogue();
            
            Notify(PlayerActions.SeenUniverseDeathSequence);
        }
    }

    public void OnNotify(PlayerActions trigger)
    {
        if (trigger == PlayerActions.Died)
        {
            if (GetIsSeenUniverseDeathSequence())
                return;
            
            Invoke(nameof(ActivateDeathSequenceStartDialogue), triggerAfterSeconds);
        }
        
    }

    public void OnNotify(DialogueAction trigger)
    {
        if (trigger == DialogueAction.RevivePlayer)
            SpawnBulletOutOfPlayerBodyTowardsEnemy();
        
        if (trigger == DialogueAction.PlayerCanMove)
            _playerAnimator.SetBool(IsDead, false);

        if (trigger == DialogueAction.DeathSequenceEnd)
        {
            ActivateSceneDialogue();
            PlayerPreferences.SetPlayerPrefValue(PlayerPrefsKeys.DeathSequenceEnded, true);
        }
    }

    private void SetPlayerDeathAnimation()
    {
        var playerAnimator = player.GetComponent<Animator>();
        playerAnimator.SetBool(IsDead, true);
    }

    private void ActivateSceneDialogue()
    {
        currentSceneDialogue.SetActive(false);
        deathSequenceStartDialogue.SetActive(false);
        currentSceneDialogue.SetActive(true);
    }
    
    private void ActivateDeathSequenceStartDialogue()
    {
        currentSceneDialogue.SetActive(false);
        deathSequenceStartDialogue.SetActive(true);
    }

    private void ActivateEndSequenceEndDialogue()
    {
        currentSceneDialogue.SetActive(false);
        deathSequenceEndDialogue.SetActive(true);
    }

    void SpawnBulletOutOfPlayerBodyTowardsEnemy()
    {
        timeController.SuperSlowTime();
        
        var position = player.transform.position;
        position.y += 2f;
        position.x += 2f;
        
        var bullet = Instantiate(bulletPrefab, position, Quaternion.Euler(0f, 0f, -45));
        bullet.gameObject.tag = "Untagged";
    }
    
    void PositionPistolOnEnemy()
    {
        pistol.gameObject.transform.parent = enemy.transform;
        var newPistolPosition = enemy.transform.position;
        newPistolPosition.y -= 1f;
        newPistolPosition.x -= 1f;
        pistol.gameObject.transform.position = newPistolPosition;
        
        // enemy.GetComponent<CollectWeapon>()
    }
    
    bool GetIsSeenUniverseDeathSequence()
    {
        return PlayerPreferences.GetPlayerPrefValue(PlayerPrefsKeys.SeenUniverseDeathSequence);
    }

    bool IsDeathSequenceEnded()
    {
        return PlayerPreferences.GetPlayerPrefValue(PlayerPrefsKeys.DeathSequenceEnded);
    }
}