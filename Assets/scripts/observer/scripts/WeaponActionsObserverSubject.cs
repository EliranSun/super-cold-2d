using action_triggers.scripts;
using UnityEngine;
using UnityEngine.Events;

namespace observer.scripts
{
    public class WeaponActionsObserverSubject : MonoBehaviour
    {
        public UnityEvent<WeaponObserverEvents> observers;

        protected void NotifyObservers(WeaponObserverEvents message)
        {
            print("WeaponActions NotifyObservers");
            observers?.Invoke(message);
        }
    }
}