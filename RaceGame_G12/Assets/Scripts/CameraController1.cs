using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController1 : MonoBehaviour
{
    public CarController1 target;
    private Vector3 offsetDir;

    public float minDistance, maxDistance;
    private float activeDistance;

    public Transform startTargetOffset;


    void Start()
    {
        offsetDir = transform.position - startTargetOffset.position;

        activeDistance = minDistance;

        offsetDir.Normalize();
    }


    void Update()
    {
        activeDistance = minDistance + ((maxDistance - minDistance) * (target.theRB.velocity.magnitude / target.maxSpeed));

        transform.position = target.transform.position + (offsetDir * activeDistance);
    }
}
