using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastVisual : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 2f);
            if (Physics.Raycast(ray, out RaycastHit hit, 10f))
            {
                Debug.Log("Golpeó: " + hit.collider.name);
            }
            else
            {
                Debug.Log("No golpeó nada");
            }
        }
    }
}
