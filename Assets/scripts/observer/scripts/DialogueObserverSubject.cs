using action_triggers.scripts;
using UnityEngine;
using UnityEngine.Events;

namespace observer.scripts
{
    public class DialogueObserverSubject : MonoBehaviour
    {
        public UnityEvent<DialogueAction> observers;

        protected void Notify(DialogueAction message)
        {
            observers?.Invoke(message);
        }
    }
}