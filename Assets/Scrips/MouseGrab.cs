using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MouseGrab : MonoBehaviour
{
    public float grabDistance = 3f;     // Qué tan lejos puede agarrar
    public float grabForce = 10f;       // Qué tan rápido se acerca al punto objetivo
    public float holdDistance = 2f;     // Distancia a la que el objeto se sostiene frente a la cámara

    private Transform grabbedObject;
    private Rigidbody grabbedRb;
    private Collider grabbedCollider;
    private XRSocketInteractor grabbedSocket;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryGrabObject();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            ReleaseObject();
        }

        if (grabbedObject != null && grabbedRb != null)
        {
            Vector3 targetPos = transform.position + transform.forward * holdDistance;
            grabbedRb.MovePosition(Vector3.Lerp(grabbedRb.position, targetPos, grabForce * Time.deltaTime));
        }
    }

    void TryGrabObject()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit; // Declaración correcta

        Debug.DrawRay(transform.position, transform.forward * grabDistance, Color.red, 2f);

        // El problema ocurría si hacías Debug.Log antes de este "if"
        if (Physics.Raycast(ray, out hit, grabDistance))
        {
            Debug.Log("Raycast hit: " + hit.collider.name); // ← ahora sí está dentro del if

            if (hit.collider.CompareTag("Grabbable"))
            {
                grabbedObject = hit.collider.transform;
                grabbedRb = grabbedObject.GetComponent<Rigidbody>();
                grabbedCollider = grabbedObject.GetComponent<Collider>();

                if (grabbedCollider != null)
                    grabbedCollider.enabled = false;

                grabbedSocket = hit.collider.GetComponentInParent<UnityEngine.XR.Interaction.Toolkit.XRSocketInteractor>();
                if (grabbedSocket != null)
                {
                    if (grabbedSocket.hasSelection)
                        grabbedSocket.EndManualInteraction();
                    grabbedSocket.socketActive = false;
                }

                if (grabbedRb != null)
                {
                    grabbedRb.isKinematic = false;
                    grabbedRb.useGravity = false;
                }
            }
        }
        else
        {
            Debug.Log("Raycast no tocó nada");
        }
    }


    void ReleaseObject()
    {
        if (grabbedObject != null)
        {
            // Reactiva colisiones
            if (grabbedCollider != null)
                grabbedCollider.enabled = true;

            // Reactiva el socket si existía
            if (grabbedSocket != null)
                grabbedSocket.socketActive = true;

            // Reactiva gravedad
            if (grabbedRb != null)
                grabbedRb.useGravity = true;

            // Limpieza
            grabbedObject = null;
            grabbedRb = null;
            grabbedCollider = null;
            grabbedSocket = null;
        }
    }
}
