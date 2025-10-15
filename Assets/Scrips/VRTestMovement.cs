using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRTestMovement : MonoBehaviour
{
    public float speed = 12f;
    public float rotationSpeed = 100f; // velocidad de rotación

    void Update()
    {
        // Movimiento adelante / atrás
        float v = Input.GetAxis("Vertical");

        // Rotación izquierda / derecha
        float h = Input.GetAxis("Horizontal");

        // Avanza en la dirección que mira el jugador
        transform.Translate(Vector3.forward * v * speed * Time.deltaTime);

        // Rota sobre el eje Y
        transform.Rotate(Vector3.up * h * rotationSpeed * Time.deltaTime);
    }
}

