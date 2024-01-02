using action_triggers.scripts;
using UnityEngine;
using UnityEngine.Events;

public class CharacterDataObserverSubject : MonoBehaviour
{
    public UnityEvent<CharacterData> characterData;

    protected void Notify(CharacterData message)
    {
        characterData?.Invoke(message);
    }
}