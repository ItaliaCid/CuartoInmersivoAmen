using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseGrab : MonoBehaviour
{
    private Camera cam;
    private Rigidbody grabbedObject;
    public float grabForce = 10f;
    public float maxGrabDistance = 5f;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, maxGrabDistance))
            {
                if (hit.rigidbody != null)
                {
                    grabbedObject = hit.rigidbody;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            grabbedObject = null;
        }

        if (grabbedObject)
        {
            Vector3 targetPos = cam.transform.position + cam.transform.forward * 2f;
            Vector3 direction = targetPos - grabbedObject.position;
            grabbedObject.velocity = direction * grabForce;
        }
    }
}
