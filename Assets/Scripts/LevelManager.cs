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

    public int Id { get; } = _nextId++;
}


public class LevelManager : MonoBehaviour
{
    // scene 1
    private const int RyanControlsTimeLineIndex = 1;
    private const int EnemyTargetsWeaponLineIndex = 2;
    private const int RespawnLineIndex = 4;

    private const int KilledHerLineIndex = 5;

    // scene 2
    private const int LivingAsValeryLineIndex = 8;

    private const int KilledHimLineIndex = 11;

    // scene 3
    private const int ValeryRespawnLineIndex = 12;
    private const int ValeryDiesAgainLineIndex = 16;

    private const int ValeryKillsRyanLineIndex = 17;

    // scene 4
    private const int CopFoundRyanLineIndex = 18;
    private const int CopKillsRyanLineIndex = 20;
    private const int RyanKillsCopLineIndex = 19;

    // scene 6
    private const int ManInGreenAppearsLineIndex = 27;
    private const int ManInGreenDisappearsLineIndex = 31;
    private const int SceneSixEndLineIndex = 32;

    // scene 7
    private const int TheTowerLineIndex = 32;
    private const int ThePlanLineIndex = 33;
    private const int DeadLineIndex = 34;
    private const int RyanKillsSecurityGuardLineIndex = 35;

    [SerializeField] private AudioClip[] clips;
    [SerializeField] private TextMeshProUGUI narratorText;
    [SerializeField] private GameObject player;
    [SerializeField] private TimeController timeController;
    [SerializeField] private GameObject weapon;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private int lineIndex;
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject manInGreen;

    private AudioSource _audioSource;
    private bool _isDead;
    private bool _isEnemyDead;
    private bool _isPlayerMoved;
    private Line[] _lines;

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
        _lines = new[]
        {
            new Line(
                "Poor Ryan. He is not in control of his life. \nBut he can control time. Uncontrollably. (todo: what is your name)",
                clips[0]),
            new(
                "When he moves an inch, times slows down almost to a halt. \nIt is indeed a terrible fate. " +
                "\nMoving, after all, is an essential part of life.\nNothing can exist without moving.",
                clips[1], 2),
            new("His wife, Valery, hated this. \nShe tried to kill him, because she was too afraid of divorce.",
                clips[2], 1),
            new("Ryan was a pacifist though.\nAll He could do is run away from her, or act in self defense.", clips[3]),
            new(
                "And then he died. \nBut he also had the ability to reverse time, by simply hitting the R button, whatever that means.",
                clips[4]),
            new("He killed her, but not murdered - and this was an important distinction.", clips[5], 1),
            new("The problem with Ryan though, is that he never tried to see things from a different perspective",
                clips[6], 1),
            new(
                "For example, seeing things from the now dead Valery's perspective would entail an ENTIRELY different picture.",
                clips[7], 1),
            new(
                "Imagine what it would be like, living as her.\n Seeing Ryan, your husband, moving at the speed of light.",
                clips[8], 1),
            new("Unable to stop, unable to speak. \nUnable to do anything, but move.", clips[9], 1),
            new("That would drive you crazy, to the point of wanting to kill him.\nObviously.", clips[10]),
            new("What else can you do, but kill him, in this wretched world he enforced upon you?", clips[11]),
            new(
                "Inevitable. And the real tragedy of it all was that she had to experience her death over and over again.\n Whenever he pressed R",
                clips[12]),
            new("It is unthinkable, of course, but she could try and understand how he felt, before going to that gun.",
                clips[13], 1),
            new("Thinking really hard, what was the cause of all this. Perhaps then, she would have been able to see",
                clips[14], 1),
            new("See the world, as he does. Feel the world as he felt.", clips[15]),
            new(
                "But that is, of course, fantasy world. In reality, she died... That is, until he reversed time by pressing R",
                clips[16]),
            new("Would she still kill him, even then?", clips[17]),
            new(
                "Back in reality - Ryan was afraid. A cop found him quickly - and how could Ryan explain?",
                clips[18]),
            new("He killed him too. And now he was a cop murderer.", clips[19]),
            new("The cop killed him. But he could not possibly know that death for Ryan is only temporary...",
                clips[20]),
            new("It didn't take long for them to find the cop killer. Now he was in real trouble", clips[21]),
            new("Of course, there was just no way that he can win this... But he had time on his side.", clips[22]),
            new("After the showdown, Ryan was on the hide. \nHe was thinking about two things.", clips[23], 0.3f),
            new("One: if he is not giving up on life just yet, he better fight back. No more pacifism bullshit.",
                clips[24], 1),
            new("Two: Valery. He was thinking about his wife. His dead wife. Could he have done things different?",
                clips[25], 1),
            new("It was at that moment that he appeared, out of thin air.", clips[26], 1),
            new("Ryan was in shock. Who are you? He asked. The man in green replied: I am your salvation", clips[27],
                1),
            new(
                "Ryan noticed that this man is not being affected by his time manipulation. Perhaps he was indeed his path to redemption?",
                clips[28], 1),
            new("The man in green said: There is a tower up north. If you'll manage to get there, " +
                "and climb all the way to the top - you'll find there a key.\n " +
                "This key, is the solution to your problem. You'll know what to do with it once you get it.",
                clips[29], 1),
            new("Good luck - the man in green concluded. And then he disappeared, just as he appeared.", clips[30], 1),
            new("Ryan had no other choice.", clips[31]),
            new(
                "Didn't take long for Ryan to find the tower. The entrance floor had one clerk, and one security guard near the elevator.",
                clips[32], 1),
            new("Ryan's plan was simple - punch the security guy, and take his gun.", clips[33]),
            new("Simple.", clips[34]),
            new("Ryan advanced to the next floor, without any hesitation.", clips[35])
        };

        StartCoroutine(ReadNextLine());
    }

    private void Update()
    {
        if (IsTimeSlowed) _audioSource.pitch = 0.5f;
        else _audioSource.pitch = 1;

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            lineIndex = GetSceneLineIndex("Restart");
            return;
        }

        if (player && player.gameObject.activeSelf == false && !_isDead)
        {
            _isDead = true;
            lineIndex = GetSceneLineIndex("Died");
            StartCoroutine(ReadNextLine(true));
            _audioSource.pitch = 1;
            return;
        }

        if (enemy && enemy.gameObject.activeSelf == false && !_isEnemyDead)
        {
            _isEnemyDead = true;
            lineIndex = GetSceneLineIndex("EnemyDied");
            StartCoroutine(ReadNextLine());
        }


        if (lineIndex == LivingAsValeryLineIndex && SceneManager.GetActiveScene().name == "Level 1")
            NextScene();

        if (lineIndex is ValeryDiesAgainLineIndex or ValeryKillsRyanLineIndex &&
            SceneManager.GetActiveScene().name == "Level 3")
            NextScene();

        if (lineIndex is RyanKillsCopLineIndex &&
            SceneManager.GetActiveScene().name == "Level 4")
            NextScene();

        if (!_isPlayerMoved && lineIndex == RyanControlsTimeLineIndex && (Input.GetKeyDown(KeyCode.A) ||
                                                                          Input.GetKeyDown(KeyCode.D) ||
                                                                          Input.GetKeyDown(KeyCode.W) ||
                                                                          Input.GetKeyDown(KeyCode.S)))
        {
            StartCoroutine(ReadNextLine());
            _isPlayerMoved = true;
        }

        if (lineIndex == ManInGreenAppearsLineIndex) manInGreen.SetActive(true);
        if (lineIndex == ManInGreenDisappearsLineIndex) manInGreen.SetActive(false);
        if (lineIndex == SceneSixEndLineIndex) NextScene();
    }

    private void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    private int GetSceneLineIndex(string scenario)
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Level 1":
            {
                switch (scenario)
                {
                    case "Restart": return EnemyTargetsWeaponLineIndex;
                    case "Died": return RespawnLineIndex;
                    case "EnemyDied": return KilledHerLineIndex;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(scenario), scenario, null);
                }
            }

            case "Level 2":
                switch (scenario)
                {
                    case "Restart": return LivingAsValeryLineIndex;
                    case "Died": return ValeryRespawnLineIndex;
                    case "EnemyDied": return KilledHimLineIndex;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(scenario), scenario, null);
                }

            case "Level 3":
                switch (scenario)
                {
                    case "Restart": return ValeryRespawnLineIndex;
                    case "Died": return ValeryDiesAgainLineIndex;
                    case "EnemyDied": return ValeryKillsRyanLineIndex;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(scenario), scenario, null);
                }

            case "Level 4":
                switch (scenario)
                {
                    case "Restart": return CopFoundRyanLineIndex;
                    case "Died": return CopKillsRyanLineIndex;
                    case "EnemyDied": return RyanKillsCopLineIndex;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(scenario), scenario, null);
                }

            case "Level 5":
                switch (scenario)
                {
                    case "Restart": return CopFoundRyanLineIndex;
                    case "Died": return CopKillsRyanLineIndex;
                    case "EnemyDied": return RyanKillsCopLineIndex;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(scenario), scenario, null);
                }

            case "Level 7":
                switch (scenario)
                {
                    case "Restart": return TheTowerLineIndex;
                    case "Died": return DeadLineIndex;
                    case "EnemyDied": return RyanKillsSecurityGuardLineIndex;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(scenario), scenario, null);
                }

            default:
                throw new ArgumentOutOfRangeException(nameof(scenario), scenario, null);
        }
    }

    private IEnumerator ReadNextLine()
    {
        narratorText.text = _lines[lineIndex].Text;
        _audioSource.clip = _lines[lineIndex].Clip;
        _audioSource.Play();
        lineIndex++;

        if (lineIndex == RespawnLineIndex) lineIndex++;

        if (_lines[lineIndex - 1].GoToNextIn > 0)
        {
            // TODO: Instead of trying to sync audio and text, simply trigger the next line WHEN
            // TODO: the current audio clip ends + the delay from GoToNextIn
            // FIXME: Without this, lines currently can cut one another if time is being slowed too much
            // TODO: And in general, this entire class need to be split:
            // Line logic should not be here + I think it can be refactored to a class holding 
            // all the information the logic needs:
            // line text, action at the end of the line, delay, what lines to go to next, etc.
            // like the node approach in Yarn Spinner
            if (_audioSource.isPlaying) yield return new WaitForSeconds(_audioSource.clip.length);
            yield return new WaitForSeconds(_lines[lineIndex - 1].GoToNextIn);

            StartCoroutine(ReadNextLine());
        }
    }

    private IEnumerator ReadNextLine(bool force)
    {
        if (_audioSource.isPlaying && !force) yield return new WaitForSeconds(_audioSource.clip.length);
        if (lineIndex >= _lines.Length)
        {
            narratorText.text = "";
            yield break;
        }

        narratorText.text = _lines[lineIndex].Text;
        _audioSource.clip = _lines[lineIndex].Clip;
        _audioSource.Play();

        if (lineIndex == EnemyTargetsWeaponLineIndex)
            enemy.GetComponent<EnemyMovement>().target = weapon.transform;

        lineIndex++;
        if (lineIndex == RespawnLineIndex) lineIndex++;

        if (_lines[lineIndex - 1].GoToNextIn > 0)
        {
            if (_audioSource.isPlaying) yield return new WaitForSeconds(_audioSource.clip.length);
            yield return new WaitForSeconds(_lines[lineIndex - 1].GoToNextIn);
            if (_audioSource.isPlaying)
                // there's another scenario playing
                yield break;

            StartCoroutine(ReadNextLine());
        }
    }
}