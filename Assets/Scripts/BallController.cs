using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform playerHead;
    public float spawnDistance = 1.8f;
    public float spawnHeight = .2f;

    public float rallySpeed = 7f;

    Rigidbody rb;

    enum BallState {WAITING, RALLY}
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
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    void EnterRallyState(Vector3 hitForward)
    {
        state = BallState.RALLY;
        Vector3 dir = GetAssistedLaunchDirection(hitForward);
        rb.velocity = hitForward * rallySpeed;
    }

    Vector3 GetAssistedLaunchDirection(Vector3 racketForward)
    {
        Vector3 dir = racketForward;

        dir.y = Mathf.Abs(dir.y) + 0.25f;

        return dir.normalized;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.collider.CompareTag("Racket"))
        {
            if (state == BallState.WAITING)
            {
                EnterRallyState(col.transform.forward);
            } else
            {
                ApplyPlayerHit(col.transform.forward);
            }
        }
        Debug.Log("collision detected");
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
        Vector3 target =
            playerHead.position + playerHead.forward * 1.2f + Vector3.up * 0.3f;
        Vector3 dir = (target - transform.position).normalized;

        dir.y += 0.2f;

        rb.velocity = dir.normalized * rallySpeed;
        rb.angularVelocity = Vector3.zero;
    }
    // Update is called once per frame
}
