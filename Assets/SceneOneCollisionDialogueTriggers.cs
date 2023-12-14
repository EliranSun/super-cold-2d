using UnityEngine;

public class SceneOneCollisionDialogueTriggers : ObserverSubject
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Wall"))
        {
            if (col.gameObject.name == "GetLostBoundary")
                NotifyObservers(DialogueTrigger.AfraidToGetLost);

            if (col.gameObject.name == "CrossTheRoadBoundary")
                NotifyObservers(DialogueTrigger.AfraidToCrossTheRoad);

            if (col.gameObject.name == "NotHisHouseBoundary")
                NotifyObservers(DialogueTrigger.WrongHouse);
        }
    }
}