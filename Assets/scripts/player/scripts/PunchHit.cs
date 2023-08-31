using UnityEngine;

public class PunchHit : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            var punchParent = transform.parent.GetComponent<Punch>();

            if (!punchParent) return;
            if (!punchParent.isLeftHandPunchMove && !punchParent.isRightHandPunchMove) return;

            var npcShootingBullets = col.gameObject.GetComponent<NpcShootBullets>();
            if (npcShootingBullets) npcShootingBullets.enabled = false;

            var weapon = GameObject.Find("Pistol");
            if (weapon)
            {
                weapon.transform.parent = null;
                weapon.GetComponent<LookAtPlayer>().enabled = false;
            }
        }
    }
}