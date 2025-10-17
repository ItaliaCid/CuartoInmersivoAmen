using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Click detectado desde: " + gameObject.name);
            Ray ray = new Ray(transform.position, transform.forward);
            Debug.DrawRay(ray.origin, ray.direction * 10f, Color.green, 3f);
            if (Physics.Raycast(ray, out RaycastHit hit, 10f))
            {
                Debug.Log("Raycast golpeó: " + hit.collider.name);
            }
            else
            {
                Debug.Log("Raycast no golpeó nada");
            }
        }
    }
}
