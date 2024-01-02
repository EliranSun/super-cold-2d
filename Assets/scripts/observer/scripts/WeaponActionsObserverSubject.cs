using UnityEngine;
using UnityEngine.Events;

namespace observer.scripts
{
    public class WeaponActionsObserverSubject : MonoBehaviour
    {
        public UnityEvent<WeaponActions> observers;

        protected void NotifyObservers(WeaponActions message)
        {
            print("WeaponActions NotifyObservers");
            observers?.Invoke(message);
        }
    }
}