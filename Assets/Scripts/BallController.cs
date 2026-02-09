using UnityEngine;

public class BallController : MonoBehaviour
{
    public Transform playerHead;
    public float spawnDistance = 1.8f;
    public float spawnHeight = 0.2f;
    public float rallySpeed = 7f;

    Rigidbody rb;

    enum BallState { WAITING, RALLY }
    BallState state = BallState.WAITING;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SpawnInFrontOfPlayer();
        EnterWaitingState();
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

        dir.y = 0f;

        return dir.normalized;
    }

    private void OnCollisionEnter(Collision col)
    {
        Debug.Log("Collision with: " + col.collider.name);

        if (col.collider.CompareTag("Racket"))
        {
            Vector3 forward = col.transform.forward;

            if (state == BallState.WAITING)
                EnterRallyState(forward);
            else
                ApplyPlayerHit(forward);
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
        Vector3 target = playerHead.position + playerHead.forward * 1.2f + Vector3.up * 0.3f;
        Vector3 dir = (target - transform.position).normalized;

        dir.y -= 0.2f;

        rb.velocity = dir.normalized * rallySpeed;
        rb.angularVelocity = Vector3.zero;
    }
}
