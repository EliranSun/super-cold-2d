using action_triggers.scripts;
using UnityEngine;
using UnityEngine.Events;

public class ObserverSubject : MonoBehaviour
{
    public UnityEvent<string> observers;
    public UnityEvent<DialogueTrigger> dialogueObservers;
    public UnityEvent<PlayerInfo> playerInfoObservers;

    protected void NotifyObservers(PlayerInfo message)
    {
        print($"NOTIFY OBSERVERS {message}; Observers: {observers}");
        playerInfoObservers?.Invoke(message);
    }

    protected void NotifyObservers(WeaponActions message)
    {
        print($"NOTIFY OBSERVERS {message}; Observers: {observers}");
        observers?.Invoke(message.ToString());
    }

    protected void NotifyObservers(PlayerActions message)
    {
        print($"NOTIFY OBSERVERS {message}; Observers: {observers}");
        observers?.Invoke(message.ToString());
        dialogueObservers?.Invoke((DialogueTrigger)message);
    }

    protected void NotifyObservers(DialogueTrigger message)
    {
        print($"NOTIFY OBSERVERS {message}; Observers: {observers}");
        dialogueObservers?.Invoke(message);
    }
}