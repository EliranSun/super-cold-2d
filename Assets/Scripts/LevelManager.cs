using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private static int _lineIndex;
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private TextMeshProUGUI narratorText;
    [SerializeField] private GameObject player;
    [SerializeField] private TimeController timeController;
    [SerializeField] private GameObject weapon;
    [SerializeField] private Camera mainCamera;
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

    private void Awake()
    {
        if (DateTime.Now.Hour is >= 6 and < 12)
            mainCamera.backgroundColor = new Color(0.8f, 0.8f, 0.8f);
        else if (DateTime.Now.Hour is >= 12 and < 18)
            mainCamera.backgroundColor = new Color(1, 1, 1);
        else if (DateTime.Now.Hour is >= 18 and < 22)
            mainCamera.backgroundColor = new Color(0.5f, 0.5f, 0.5f);
        else if (DateTime.Now.Hour >= 22 && DateTime.Now.Hour < 6)
            mainCamera.backgroundColor = new Color(0.3f, 0.3f, 0.3f);

        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Level 0":
                _ryanControlsTimeLineIndex = 1;
                _notHisHouseLineIndex = 3;
                _afraidToCrossTheRoadLineIndex = 5;
                _afraidToGetLostLineIndex = 6;

                _lines = new[]
                {
                    new Line("Poor Ryan. He is not in control of his life. \nBut he can control time. Uncontrollably.",
                        clips[0]),
                    new("When he moves an inch, times slows down almost to a halt.", clips[1], 0.5f),
                    new("It is indeed a terrible fate. A fate Ryan had submitted to.", clips[2]),
                    new("This was not his house and Ryan knew it perfectly well.", clips[3], 0.5f),
                    new("Soon enough he is going to wish that was the case", clips[4]),
                    new("Ryan was too afraid to cross the road. Even chickens are not afraid of that", clips[5]),
                    new(
                        "Ryan was too afraid to get lost. And besides, his wife is waiting for him. She has something very important to tell him.",
                        clips[6])
                };
                break;

            case "Level 1":
                _respawnLineIndex = 0;
                _levelFailedLineIndex = 2;
                _levelCompleteLineIndex = 3;

                _lines = new[]
                {
                    new Line(
                        "His wife, Valery, hated this. \nShe tried to kill him, because she was too afraid of divorce.",
                        clips[0], 1),
                    new("Ryan was a pacifist though.\nAll He could do is run away from her, or act in self defense.",
                        clips[1]),
                    new(
                        "And then he died. \nBut he also had the ability to reverse time, by simply hitting the R button, whatever that means.",
                        clips[2]),
                    new("He killed her, but not murdered - and this was an important distinction.", clips[5], 1),
                    new(
                        "The problem with Ryan though, is that he never tried to see things from a different perspective",
                        clips[3], 1)
                };
                break;

            case "Level 2":
                _respawnLineIndex = 0;
                _levelCompleteLineIndex = 4;
                _levelFailedLineIndex = 5;

                _lines = new[]
                {
                    new Line(
                        "For example, seeing things from the now dead Valery's perspective would entail an ENTIRELY different picture.",
                        clips[0], 1),
                    new(
                        "Imagine what it would be like, living as her.\n Seeing Ryan, your husband, moving at the speed of light.",
                        clips[1], 1),
                    new("Unable to stop, unable to speak. \nUnable to do anything, but move.", clips[2], 1),
                    new("That would drive you crazy, to the point of wanting to kill him.\nObviously.", clips[3]),
                    new("What else can you do, but kill him, in this wretched world he enforced upon you?",
                        clips[4]),
                    new(
                        "Inevitable. And the real tragedy of it all was that she had to experience her death over and over again.\n Whenever he pressed R",
                        clips[5])
                };
                break;

            case "Level 3":
            {
                _respawnLineIndex = 0;
                _levelCompleteLineIndex = 4;
                _levelFailedLineIndex = 3;

                _lines = new[]
                {
                    new Line(
                        "It is unthinkable, of course, but she could try and understand how he felt, before going to that gun.",
                        clips[0], 1),
                    new(
                        "Thinking really hard, what was the cause of all this. Perhaps then, she would have been able to see",
                        clips[1], 1),
                    new("See the world, as he does. Feel the world as he felt.", clips[2]),
                    new(
                        "But that is, of course, fantasy world. In reality, she died... That is, until he reversed time by pressing R",
                        clips[3]),
                    new("Would she still kill him, even then?", clips[4])
                };
                break;
            }

            case "Level 4":
            {
                _respawnLineIndex = 0;
                _levelCompleteLineIndex = 1;
                _levelFailedLineIndex = 2;

                _lines = new[]
                {
                    new Line(
                        "Back in reality - Ryan was afraid. A cop found him quickly - and how could Ryan explain?",
                        clips[0]),
                    new("He killed him too. And now he was a cop murderer.", clips[1]),
                    new("The cop killed him. But he could not possibly know that death for Ryan is only temporary...",
                        clips[2])
                };

                break;
            }

            case "Level 5":
            {
                _respawnLineIndex = 0;
                _levelCompleteLineIndex = 1;
                _levelFailedLineIndex = 0;

                _lines = new[]
                {
                    new Line("It didn't take long for them to find the cop killer. Now he was in real trouble",
                        clips[0]),
                    new("Of course, there was just no way that he can win this... But he had time on his side.",
                        clips[1])
                };

                break;
            }

            case "Level 6":
            {
                _manInGreenAppearsLineIndex = 3;
                _manInGreenDisappearsLineIndex = 7;
                _lines = new[]
                {
                    new Line("After the showdown, Ryan was on the hide. \nHe was thinking about two things.", clips[0],
                        0.3f),
                    new(
                        "One: if he is not giving up on life just yet, he better fight back. No more pacifism bullshit.",
                        clips[1], 1),
                    new(
                        "Two: Valery. He was thinking about his wife. His dead wife. Could he have done things different?",
                        clips[2], 1),
                    new("It was at that moment that he appeared, out of thin air.", clips[3], 1),
                    new("Ryan was in shock. Who are you? He asked. The man in green replied: I am your salvation",
                        clips[4],
                        1),
                    new(
                        "Ryan noticed that this man is not being affected by his time manipulation. Perhaps he was indeed his path to redemption?",
                        clips[5], 1),
                    new("The man in green said: There is a tower up north. If you'll manage to get there, " +
                        "and climb all the way to the top - you'll find there a key.\n " +
                        "This key, is the solution to your problem. You'll know what to do with it once you get it.",
                        clips[6], 1),
                    new("Good luck - the man in green concluded. And then he disappeared, just as he appeared.",
                        clips[7], 1),
                    new("Ryan had no other choice.", clips[8])
                };

                break;
            }

            case "Level 7":
            {
                _respawnLineIndex = 0;
                _levelCompleteLineIndex = 4;
                _levelFailedLineIndex = 3;

                _lines = new[]
                {
                    new Line(
                        "Didn't take long for Ryan to find the tower. The entrance floor had one clerk, and one security guard near the elevator.",
                        clips[1], 1),
                    new("Ryan's plan was simple - punch the security guy, and take his gun.", clips[2]),
                    new("Simple.", clips[3]),
                    new("Ryan advanced to the next floor, without any hesitation.", clips[4])
                };

                break;
            }
        }

        if (SceneManager.GetActiveScene().name == "Level 1" && _lineIndex == _respawnLineIndex)
            bgMusic.time = 20;

        StartCoroutine(ReadNextLine());
    }

    private void Update()
    {
        _audioSource.pitch = IsTimeSlowed ? 0.6f : 1;

        if (Input.GetKeyDown(KeyCode.R))
        {
            _lineIndex = GetSceneLineIndex("Restart");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }

        if (player && player.gameObject.activeSelf == false && !_isDead)
        {
            _isDead = true;
            _lineIndex = GetSceneLineIndex("Died");
            StartCoroutine(ReadNextLine(true));
            _audioSource.pitch = 1;
            return;
        }

        if (enemy && enemy.gameObject.activeSelf == false && !_isEnemyDead)
        {
            _isEnemyDead = true;
            _lineIndex = GetSceneLineIndex("EnemyDied");
            StartCoroutine(ReadNextLine(true));
        }

        if (!_isPlayerMoved && _lineIndex == _ryanControlsTimeLineIndex && (Input.GetKeyDown(KeyCode.A) ||
                                                                            Input.GetKeyDown(KeyCode.D) ||
                                                                            Input.GetKeyDown(KeyCode.W) ||
                                                                            Input.GetKeyDown(KeyCode.S)))
        {
            StartCoroutine(ReadNextLine());
            _isPlayerMoved = true;
        }

        if (_levelCompleteLineIndex != 0 && _lineIndex == _levelCompleteLineIndex + 1)
            NextScene();

        if (_lineIndex == _manInGreenAppearsLineIndex && manInGreen) manInGreen.SetActive(true);
        if (_lineIndex == _manInGreenDisappearsLineIndex && manInGreen) manInGreen.SetActive(false);
    }

    private void NextScene()
    {
        _lineIndex = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    private int GetSceneLineIndex(string scenario)
    {
        switch (scenario)
        {
            case "Restart": return _respawnLineIndex;
            case "Died": return _levelFailedLineIndex;
            case "EnemyDied": return _levelCompleteLineIndex;

            default:
                throw new ArgumentOutOfRangeException(nameof(scenario), scenario, null);
        }
    }

    private IEnumerator ReadNextLine(bool force = false)
    {
        if (_audioSource.isPlaying && !force) yield return new WaitForSeconds(_audioSource.clip.length);

        narratorText.text = _lines[_lineIndex].Text;
        _audioSource.clip = _lines[_lineIndex].Clip;
        _audioSource.Play();

        // TODO: Bad code
        if (_lineIndex == _respawnLineIndex && SceneManager.GetActiveScene().name == "Level 1")
            enemy.GetComponent<EnemyMovement>().target = weapon.transform;

        _lineIndex++;

        if (_lineIndex == _respawnLineIndex && SceneManager.GetActiveScene().name != "Level 1")
            _lineIndex++;

        if (_lines[_lineIndex - 1].GoToNextIn > 0)
        {
            // TODO: Instead of trying to sync audio and text, simply trigger the next line WHEN
            // TODO: the current audio clip ends + the delay from GoToNextIn
            // FIXME: Without this, lines currently can cut one another if time is being slowed too much
            // TODO: And in general, this entire class need to be split:
            // Line logic should not be here + I think it can be refactored to a class holding 
            // all the information the logic needs:
            // line text, action at the end of the line, delay, what lines to go to next, etc.
            // like the node approach in Yarn Spinner
            if (_audioSource.isPlaying && !force) yield return new WaitForSeconds(_audioSource.clip.length);
            yield return new WaitForSeconds(_lines[_lineIndex - 1].GoToNextIn);

            StartCoroutine(ReadNextLine());
        }
    }

    public void AfraidToCrossTheRoad()
    {
        if (_afraidToCrossTheRoadSeen) return;

        _lineIndex = _afraidToCrossTheRoadLineIndex;
        _afraidToCrossTheRoadSeen = true;
        StopAllCoroutines();
        StartCoroutine(ReadNextLine(true));
    }

    public void AfraidToGetLost()
    {
        if (_afraidToGetLostSeen) return;

        _lineIndex = _afraidToGetLostLineIndex;
        _afraidToGetLostSeen = true;
        StopAllCoroutines();
        StartCoroutine(ReadNextLine(true));
    }

    public void NotHisHouse()
    {
        if (_notHisHouseSeen) return;

        _lineIndex = _notHisHouseLineIndex;
        _notHisHouseSeen = true;
        StopAllCoroutines();
        StartCoroutine(ReadNextLine(true));
    }
}