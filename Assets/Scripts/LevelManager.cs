using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

internal class Line
{
    public AudioClip Clip;
    public bool RequireCondition;
    public string Text;

    public Line(string text, AudioClip clip, bool requireCondition = false)
    {
        Text = text;
        RequireCondition = requireCondition;
        Clip = clip;
    }
}


public class LevelManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI narratorText;
    [SerializeField] private AudioClip[] clips;

    private AudioSource _audioSource;
    private bool _isPlayerMoved;
    private int _lineIndex;

    private Line[] _lines;
    private bool _shouldAdvanceLine = true;

    private void Start()
    {
        _lines = new[]
        {
            new Line(
                "Poor Ryan. He is not in control of his life. \nBut he can control time. Uncontrollably. (todo: what is your name)",
                clips[0]),
            new(
                "When he moves an inch, times slows down almost to a halt. \nIt is indeed a terrible fate. \nMoving, after all, is an essential part of life.\nNothing can exist without moving.",
                clips[1]),
            new("His wife, Valery, hated this. \nShe tried to kill him, because she was too afraid of divorce.",
                clips[2])
        };

        _audioSource = GetComponent<AudioSource>();
        ReadNextLine();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        if (!_isPlayerMoved && (Input.GetKeyDown(KeyCode.A) ||
                                Input.GetKeyDown(KeyCode.D) ||
                                Input.GetKeyDown(KeyCode.W) ||
                                Input.GetKeyDown(KeyCode.S)))
            _isPlayerMoved = true;
    }

    private void ReadNextLine()
    {
        if (_lines[_lineIndex].RequireCondition) return;

        narratorText.text = _lines[_lineIndex].Text;
        _audioSource.clip = _lines[_lineIndex].Clip;
        _audioSource.Play();
    }
}