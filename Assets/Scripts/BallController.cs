using UnityEngine;
using Valve.VR;

public class BallController : MonoBehaviour
{
    public Transform playerHead;
    public float spawnDistance = 1.8f;
    public float spawnHeight = 0.2f;
    public float rallySpeed = 7f;
    [Header("SteamVR Reset Input")]
    public SteamVR_Action_Boolean resetAction;
    public SteamVR_Input_Sources resetHand = SteamVR_Input_Sources.RightHand;
    bool timerStarted = false;
    public Transform racketNetForward;

    Rigidbody rb;

    enum BallState { WAITING, RALLY }
    BallState state = BallState.WAITING;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SpawnInFrontOfPlayer();
        EnterWaitingState();
    }
    private void Update()
    {
        if(resetAction != null && resetAction.GetStateDown(resetHand))
        {
            ResetBall();
        }


    }

    void SpawnInFrontOfPlayer()
    {
        Vector3 pos = playerHead.position + playerHead.forward * spawnDistance + Vector3.up * spawnHeight;
        transform.position = pos;
    }

    void EnterWaitingState()
    {
        state = BallState.WAITING;

        // IMPORTANT: keep it dynamic so collision events can happen with VR objects
        rb.isKinematic = false;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Park it in place until the first hit
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    void EnterRallyState(Vector3 hitForward)
    {
        state = BallState.RALLY;

        rb.constraints = RigidbodyConstraints.None;

        Vector3 dir = GetAssistedLaunchDirection(hitForward);
        rb.velocity = dir * rallySpeed;
        rb.angularVelocity = Vector3.zero;
    }

    Vector3 GetAssistedLaunchDirection(Vector3 racketForward)
    {
        Vector3 dir = racketForward;

        dir.y = (dir.y * .1f) ;
        dir.x *= .1f;

        return dir.normalized;
    }

    private void OnCollisionEnter(Collision col)
    {
        Debug.Log("Collision with: " + col.collider.name);

        if (col.collider.CompareTag("Racket"))
        {
            Vector3 forward = racketNetForward.forward;

            if (state == BallState.WAITING) {
                EnterRallyState(forward);
                if (!timerStarted)
                {
                    FindObjectOfType<Scoreboard>().StartTimer();
                    timerStarted = true;
                }
            }
            else
            {
                ApplyPlayerHit(forward);
            }
                
        }

        if (col.collider.CompareTag("Wall") && state == BallState.RALLY)
        {
            ReturnTowardPlayer();
        }
    }

    void ApplyPlayerHit(Vector3 racketForward)
    {
        Vector3 dir = GetAssistedLaunchDirection(racketForward);
        rb.velocity = dir * rallySpeed;
        rb.angularVelocity = Vector3.zero;
    }

    void ReturnTowardPlayer()
    {
        Vector3 target = playerHead.position;
        Vector3 dir = (target - transform.position).normalized;

        dir.y = (dir.y * .3f) - .3f;

        rb.velocity = dir.normalized * rallySpeed;
        rb.angularVelocity = Vector3.zero;
    }

    void ResetBall()
    {
        rb.constraints = RigidbodyConstraints.None;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        SpawnInFrontOfPlayer();

        EnterWaitingState();
    }
}
