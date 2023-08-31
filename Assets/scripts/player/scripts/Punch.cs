using UnityEngine;

public class Punch : MonoBehaviour
{
    [SerializeField] private Transform[] hands;

    // [SerializeField] private float punchForce = 500f;
    [SerializeField] private bool isPlayerControlled = true;

    public float punchDistance = 5;
    public float restDistance = 1;
    public float transitionSpeed = 5;

    public bool isRightHandPunchMove;
    public bool isLeftHandPunchMove;
    private Transform activeHand;
    private bool isPunchingLeft;
    private bool isPunchingRight;
    private int punchCount;
    private float timePunching;

    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (isPlayerControlled)
        {
            PunchRight();
            PunchLeft();
        }
    }

    private void PunchRight()
    {
        if (Input.GetMouseButtonDown(0)) isPunchingRight = true;
        if (Input.GetMouseButtonUp(0)) isPunchingRight = false;

        Vector2 targetPosition;
        if (isPunchingRight)
        {
            targetPosition = transform.position;
            targetPosition.x += punchDistance;
        }
        else
        {
            targetPosition = transform.position;
            targetPosition.x += restDistance;
        }

        if (hands[0].position.x < targetPosition.x)
            isRightHandPunchMove = true;
        else
            isRightHandPunchMove = false;

        hands[0].position = Vector2.Lerp(
            hands[0].position,
            targetPosition,
            Time.deltaTime * transitionSpeed
        );
    }

    private void PunchLeft()
    {
        if (Input.GetMouseButtonDown(1)) isPunchingLeft = true;
        if (Input.GetMouseButtonUp(1)) isPunchingLeft = false;

        Vector2 targetPosition;
        if (isPunchingLeft)
        {
            targetPosition = transform.position;
            targetPosition.x += punchDistance - 2;
        }
        else
        {
            targetPosition = transform.position;
            targetPosition.x += restDistance - 2;
        }

        if (hands[1].position.x < targetPosition.x)
            isLeftHandPunchMove = true;
        else
            isLeftHandPunchMove = false;

        hands[1].position = Vector2.Lerp(
            hands[1].position,
            targetPosition,
            Time.deltaTime * transitionSpeed
        );
    }
}