using UnityEngine;

internal class Line
{
    private static int _nextId;
    public readonly AudioClip Clip;
    public readonly float GoToNextIn;
    public readonly string Text;

    public Line(string text, AudioClip clip)
    {
        Text = text;
        Clip = clip;
    }

    public Line(string text, AudioClip clip, float goToNextIn = 0)
    {
        Text = text;
        Clip = clip;
        GoToNextIn = goToNextIn;
    }
}


public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private TimeController timeController;
    [SerializeField] private GameObject weapon;
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject manInGreen;

    [SerializeField] private AudioSource bgMusic;
    private int _afraidToCrossTheRoadLineIndex;
    private bool _afraidToCrossTheRoadSeen;
    private int _afraidToGetLostLineIndex;
    private bool _afraidToGetLostSeen;

    private AudioSource _audioSource;
    private bool _isDead;
    private bool _isEnemyDead;
    private bool _isPlayerMoved;
    private int _levelCompleteLineIndex;
    private int _levelFailedLineIndex;
    private Line[] _lines;
    private int _manInGreenAppearsLineIndex;
    private int _manInGreenDisappearsLineIndex;
    private int _nextLevelIndex;
    private int _notHisHouseLineIndex;
    private bool _notHisHouseSeen;
    private int _respawnLineIndex;
    private int _ryanControlsTimeLineIndex;

    private bool IsTimeSlowed
    {
        get
        {
            if (timeController != null) return timeController.isTimeSlowed;
            return false;
        }
    }

    // private void Update()
    // {
    //     _audioSource.pitch = IsTimeSlowed ? 0.6f : 1;
    //
    //     if (Input.GetKeyDown(KeyCode.R))
    //     {
    //         _lineIndex = GetSceneLineIndex("Restart");
    //         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //         return;
    //     }
    //
    //     if (player && player.gameObject.activeSelf == false && !_isDead)
    //     {
    //         _isDead = true;
    //         _lineIndex = GetSceneLineIndex("Died");
    //         StartCoroutine(ReadNextLine(true));
    //         _audioSource.pitch = 1;
    //         return;
    //     }
    //
    //     if (enemy && enemy.gameObject.activeSelf == false && !_isEnemyDead)
    //     {
    //         _isEnemyDead = true;
    //         _lineIndex = GetSceneLineIndex("EnemyDied");
    //         StartCoroutine(ReadNextLine(true));
    //     }
    //
    //     if (!_isPlayerMoved && _lineIndex == _ryanControlsTimeLineIndex && (Input.GetKeyDown(KeyCode.A) ||
    //                                                                         Input.GetKeyDown(KeyCode.D) ||
    //                                                                         Input.GetKeyDown(KeyCode.W) ||
    //                                                                         Input.GetKeyDown(KeyCode.S)))
    //     {
    //         StartCoroutine(ReadNextLine());
    //         _isPlayerMoved = true;
    //     }
    //
    //     if (_levelCompleteLineIndex != 0 && _lineIndex == _levelCompleteLineIndex + 1)
    //         NextScene();
    //
    //     if (_lineIndex == _manInGreenAppearsLineIndex && manInGreen) manInGreen.SetActive(true);
    //     if (_lineIndex == _manInGreenDisappearsLineIndex && manInGreen) manInGreen.SetActive(false);
    // }
}