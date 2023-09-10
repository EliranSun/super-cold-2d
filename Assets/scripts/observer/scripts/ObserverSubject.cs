using UnityEngine;
using UnityEngine.Events;

public class ObserverSubject : MonoBehaviour
{
    public UnityEvent<string> observers;

    protected void NotifyObservers(WeaponActions message)
    {
        print($"NOTIFY OBSERVERS {message}; Observers: {observers}");
        observers?.Invoke(message.ToString());
    }

    protected void NotifyObservers(PlayerActions message)
    {
        print($"NOTIFY OBSERVERS {message}; Observers: {observers}");
        observers?.Invoke(message.ToString());
    }
}