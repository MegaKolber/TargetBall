using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveTarget : MonoBehaviour
{
    // Start is called before the first frame update
    private float x_min = (float)-3.5;
    private float x_max = (float)3.7;
    private float y_min = (float)2.5;
    private float y_max = (float)8;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            MoveTarget();
            GameManager.Instance.AddPoint();

        }
        
    }

    void MoveTarget()
    {
        float x = Random.Range(x_min, x_max);
        float y = Random.Range(y_min, y_max);
        transform.position = new Vector3(x, y, (float)4.85);
    }
}
