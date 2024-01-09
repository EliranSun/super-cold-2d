using System.Collections;
using action_triggers.scripts;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

internal enum TriggerActions
{
    ValerySpeedUp,
    ValeryGone,
    ApartmentDecay,
    StreetDecay,
    EarthDecay,
    SunRedGiant,
    StarsSupernova,
    GalaxiesDecay,
    BlackHolesDecay,
    GameOver,
    Whiteness,
    UniverseReverse,
    StarsBorn,
    GalaxiesBorn,
    EarthBorn,
    StreetBorn,
    ApartmentBorn,
    ValeryBorn,
    BulletRevivesRyan
}

internal class SimpleLine
{
    public float PauseTime;
    public string Text;
    public TriggerActions TriggerAction;

    public SimpleLine(string text, TriggerActions triggerAction, float pauseTime = 0)
    {
        Text = text;
        PauseTime = pauseTime;
        TriggerAction = triggerAction;
    }

    public SimpleLine(string text, int pauseTime = 0)
    {
        Text = text;
        PauseTime = pauseTime;
    }
}

// TODO: Maybe this should only be responsible for death sequence
// TODO: Remove - this is replaced by DialogueConfig. Same goes to level manager
public class DialogueController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI narratorText;
    [SerializeField] private GameObject valeryEnemy;
    [SerializeField] private TimeController timeController;
    [SerializeField] private GameObject[] furniture;
    [SerializeField] private Camera mainCamera;

    [FormerlySerializedAs("DeathSequence")] [FormerlySerializedAs("isDeadSequence")] [SerializeField]
    private AudioClip[] deathSequence;

    // TODO: Parse from maleText file
    private readonly SimpleLine[] _deathSequenceDialogue =
    {
        new("As “time” went on with Ryan doing nothing at all, something very peculiar started to occur."),
        new("Ryan’s effect on time started to reverse."),
        new(
            "At first - slowly, little by little, things started to move faster and faster. Almost as if Ryan's point of reference became… infinite.",
            TriggerActions.ValerySpeedUp),
        new("Which would make sense, since he is dead and all that."),
        new("Nonetheless, the effect was not linear. Time shifted more and more. Accelerating."),
        new(
            "His wife was long gone. Did she move on? Arrested? Got out of jail? Remarried, happily with children? Dead?",
            TriggerActions.ValeryGone),
        new(
            "The apartment itself got old. The building. The street. The neighborhood. Everything got old. \n Everything, bounded by the merciless vicious mistress that is lady time.",
            TriggerActions.ApartmentDecay),
        new(
            "It did not stop. It only accelerated. Further and further. Ryan, preserved perfectly - his time dimension completely still. If only his consciousness would be here to observe.",
            TriggerActions.StreetDecay),
        new("To observe the land turn into ashes and waste. The rivers, dry. The sun - explode. The Earth - crumble.",
            TriggerActions.SunRedGiant),
        new(
            "Every death of every planet in the solar system. Every violent explosion, light years away from Ryan. The decay of the entire galaxy.",
            TriggerActions.StarsSupernova),
        new(
            "And it does not stop. Accelerating, more and more. Soulless, like a machine. All the galaxies in the entire universe come to a halt.",
            TriggerActions.GalaxiesDecay),
        new(
            "Blackness. For what seems like the longest of time, although time is progressing at unfathomable speed now. Even the most black of holes are now gone from existence.",
            TriggerActions.BlackHolesDecay),
        new(
            "Time continues, until every single particle, atom, sub-atom and matter itself stop existing. Until every single unit of energy ceased to exist. The great, cold universe itself… stops."),
        new("Time itself stops at this point. Nothing matters anymore. Everything is over. The game itself is over.",
            TriggerActions.GameOver),
        new("..."),
        new("Suddenly, whiteness. Rapid expansion, expanding time itself… in the opposite direction.",
            TriggerActions.Whiteness),
        new("A loop?"),
        new("Energy, matter, sub-atoms, atoms, molecules, particles, chemistry, physics, stars, planets, galaxies…",
            TriggerActions.GalaxiesBorn),
        new("The rapid reverse reversion of time starts to slow down.", TriggerActions.StarsBorn),
        new(
            "Ryan’s own sun emerges. Birthing the earth. Birthing oceans, land, life, creatures, mammals, human beings. Civilizations, cities, neighborhoods, streets, buildings…",
            TriggerActions.EarthBorn),
        new("Ryan’s apartment. Ryan’s wife, Valery, moves in. She shot him, but this time in reverse.",
            TriggerActions.ApartmentBorn),
        new(
            "The bullet comes out of Ryan’s bloody body. Reviving him in the process. The bullet returns to the pistol it came from.",
            TriggerActions.BulletRevivesRyan),
        new(
            "Moments before Valery shots again - Ryan can now move. Completely unaware of the magnificent journey his body just experienced."),
        new("A once in a lifetime experience, that is for sure.")
    };

    private AudioSource _audioSource;

    private bool _isNotified;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void OnNotify(string message)
    {
        if (!_isNotified && message == PlayerActions.Died.ToString())
        {
            StartCoroutine(IsDeadSequence());
            // StartCoroutine(IncreaseEnemySpeed());
            _isNotified = true;
        }
    }

    private IEnumerator IsDeadSequence()
    {
        yield return new WaitForSeconds(10);

        var i = 0;
        while (deathSequence[i])
        {
            TriggerAction(_deathSequenceDialogue[i].TriggerAction);

            timeController.NormalTime();
            _audioSource.clip = deathSequence[i];
            narratorText.text = _deathSequenceDialogue[i].Text;
            _audioSource.Play();
            i++;
            yield return new WaitForSeconds(_audioSource.clip.length + _deathSequenceDialogue[i].PauseTime);
        }
    }

    private void TriggerAction(TriggerActions action)
    {
        switch (action)
        {
            // case TriggerActions.ValerySpeedUp:
            //     valeryEnemy.GetComponent<EnemyMovement>().speed *= 4;
            //     break;
            //
            // case TriggerActions.ValeryGone:
            // {
            //     valeryEnemy.GetComponent<EnemyMovement>().speed = 0;
            //     valeryEnemy.GetComponent<EnemyMovement>().enabled = false;
            //     valeryEnemy.GetComponent<SpriteRenderer>().enabled = false;
            //     break;
            // }

            case TriggerActions.ApartmentDecay:
            {
                foreach (var item in furniture)
                    item.GetComponent<SpriteRenderer>().enabled = false;
                break;
            }

            case TriggerActions.StreetDecay:
                mainCamera.backgroundColor = new Color(0.5377358f, 0.4659695f, 0.3982289f);
                break;

            case TriggerActions.SunRedGiant:
                mainCamera.backgroundColor = new Color(1, 0, 0);
                break;
        }
    }

    // private IEnumerator IncreaseEnemySpeed()
    // {
    //     while (true)
    //     {
    //         valeryEnemy.GetComponent<EnemyMovement>().speed += 1f;
    //         yield return new WaitForSeconds(1);
    //     }
    // }
}