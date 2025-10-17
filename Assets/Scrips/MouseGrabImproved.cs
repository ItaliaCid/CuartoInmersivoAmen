using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseGrabImproved : MonoBehaviour
{
    private Camera cam;
    private Rigidbody grabbedRb;
    private bool wasKinematic;
    private float holdDistance = 2f;
    [Tooltip("Distancia máxima para hacer click y agarrar")]
    public float maxGrabDistance = 5f;
    [Tooltip("Velocidad para mover el objeto hacia la posición objetivo")]
    public float followSpeed = 25f;
    [Tooltip("Capas que pueden ser agarradas (Default = all)")]
    public LayerMask grabLayerMask = ~0;

    void Start()
    {
        cam = Camera.main;
        if (cam == null) Debug.LogError("[MouseGrabImproved] No se encontró Main Camera con tag \"MainCamera\".");
    }

    void Update()
    {
        // CLICK: intento de agarrar
        if (Input.GetMouseButtonDown(0))
        {
            TryGrab();
        }

        // SOLTAR
        if (Input.GetMouseButtonUp(0) && grabbedRb != null)
        {
            Release();
        }

        // Para debugging: muestra si la cámara está en modo estéreo
        // (esto ayuda a detectar si sigue configurada para VR)
#if UNITY_EDITOR
        if (cam != null)
        {
            // target eye check
            // note: Camera.stereoEnabled es true si está en modo estéreo
            if (cam.stereoEnabled)
            {
                Debug.LogWarning("[MouseGrabImproved] La cámara está en modo estéreo (VR). Asegúrate Target Eye = None.");
            }
        }
#endif
    }

    void FixedUpdate()
    {
        if (grabbedRb != null)
        {
            Vector3 targetPos = cam.transform.position + cam.transform.forward * holdDistance;
            // suavizado con MovePosition para física consistente
            Vector3 newPos = Vector3.Lerp(grabbedRb.position, targetPos, followSpeed * Time.fixedDeltaTime);
            grabbedRb.MovePosition(newPos);
            grabbedRb.velocity = Vector3.zero; // evita que la física nos empuje
        }
    }

    private void TryGrab()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, maxGrabDistance, grabLayerMask))
        {
            Debug.Log("[MouseGrabImproved] Raycast hit: " + hit.collider.name + " (layer: " + LayerMask.LayerToName(hit.collider.gameObject.layer) + ")");
            Rigidbody rb = hit.collider.attachedRigidbody;
            if (rb != null)
            {
                grabbedRb = rb;
                wasKinematic = grabbedRb.isKinematic;
                grabbedRb.isKinematic = false; // nos aseguramos que sea no-kinematic para MovePosition
                // opcional: congelar rotaciones si no quieres que gire
                // grabbedRb.freezeRotation = true;
                // ajustar holdDistance a la distancia actual para evitar 'snap'
                holdDistance = Vector3.Distance(cam.transform.position, grabbedRb.position);
                Debug.Log("[MouseGrabImproved] Agarrado: " + grabbedRb.name + " distancia: " + holdDistance);
            }
            else
            {
                Debug.Log("[MouseGrabImproved] El objeto golpeado no tiene Rigidbody: " + hit.collider.name);
            }
        }
        else
        {
            Debug.Log("[MouseGrabImproved] Raycast no golpeó nada.");
        }
    }

    private void Release()
    {
        if (grabbedRb != null)
        {
            Debug.Log("[MouseGrabImproved] Soltado: " + grabbedRb.name);
            // restaurar kinematic
            grabbedRb.isKinematic = wasKinematic;
            // opcional: aplicar un poco de inercia según la dirección de la cámara
            // grabbedRb.velocity = cam.transform.forward * 2f;
            grabbedRb = null;
        }
    }
}
