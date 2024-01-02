using action_triggers.scripts;
using UnityEngine;
using UnityEngine.Events;

public class ObserverSubject : MonoBehaviour
{
    public UnityEvent<string> observers;
    public UnityEvent<DialogueTrigger> dialogueObservers;
    public UnityEvent<PlayerInfo> playerInfoObservers;
    public UnityEvent<CharacterData> enemyInfoObservers;

    protected void NotifyObservers(PlayerInfo message)
    {
        print("PlayerInfo NotifyObservers");
        playerInfoObservers?.Invoke(message);
    }

    protected void NotifyObservers(WeaponActions message)
    {
        print("WeaponActions NotifyObservers");
        observers?.Invoke(message.ToString());
    }

    protected void NotifyObservers(PlayerActions message)
    {
        print("PlayerActions NotifyObservers");
        observers?.Invoke(message.ToString());
        dialogueObservers?.Invoke((DialogueTrigger)message);
    }

    protected void NotifyObservers(DialogueTrigger message)
    {
        print("DialogueTrigger NotifyObservers");
        dialogueObservers?.Invoke(message);
    }

    protected void NotifyObservers(CharacterData message)
    {
        enemyInfoObservers?.Invoke(message);
    }
}