using UnityEngine;
using UnityEngine.Events;

namespace observer.scripts
{
    public class PlayerActionsObserverSubject : MonoBehaviour
    {
        public UnityEvent<PlayerActions> observers;

        protected void NotifyObservers(PlayerActions message)
        {
            print("PlayerActions NotifyObservers");
            observers?.Invoke(message);
        }
    }
}