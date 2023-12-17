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
        playerInfoObservers?.Invoke(message);
    }

    protected void NotifyObservers(WeaponActions message)
    {
        observers?.Invoke(message.ToString());
    }

    protected void NotifyObservers(PlayerActions message)
    {
        observers?.Invoke(message.ToString());
        dialogueObservers?.Invoke((DialogueTrigger)message);
    }

    protected void NotifyObservers(DialogueTrigger message)
    {
        dialogueObservers?.Invoke(message);
    }
}