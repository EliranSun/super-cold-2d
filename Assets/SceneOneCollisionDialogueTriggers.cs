using UnityEngine;

public class SceneOneCollisionDialogueTriggers : ObserverSubject
{
    private bool _seenCrossTheRoadDialogue;
    private bool _seenGetLostDialogue;
    private bool _seenNotHisHouseDialogue;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Wall"))
        {
            if (col.gameObject.name == "GetLostBoundary" && !_seenGetLostDialogue)
            {
                _seenGetLostDialogue = true;
                NotifyObservers(DialogueTrigger.AfraidToGetLost);
            }

            if (col.gameObject.name == "CrossTheRoadBoundary" && !_seenCrossTheRoadDialogue)
            {
                _seenCrossTheRoadDialogue = true;
                NotifyObservers(DialogueTrigger.AfraidToCrossTheRoad);
            }

            if (col.gameObject.name == "NotHisHouseBoundary" && !_seenNotHisHouseDialogue)
            {
                _seenNotHisHouseDialogue = true;
                NotifyObservers(DialogueTrigger.WrongHouse);
            }
        }
    }
}