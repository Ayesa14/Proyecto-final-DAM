using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// Script que gestiona el movimiento de un objeto entre dos puntos (mostrar y esconder),
/// como por ejemplo una planta que sale y se esconde de una tubería.
/// </summary>
public class ShowAndHide : MonoBehaviour
{
    public GameObject objectToMove; // Objeto que se moverá (por ejemplo, la planta)
    public Transform showPoint;     // Punto donde el objeto se muestra
    public Transform hidePoint;     // Punto donde el objeto se esconde
    public float waitForShow;       // Tiempo de espera antes de mostrarse
    public float waitForHide;       // Tiempo de espera antes de esconderse
    public float speedShow;         // Velocidad al mostrarse
    public float speedHide;         // Velocidad al esconderse

    float timerShow;                // Temporizador para mostrar
    float timerHide;                // Temporizador para esconder
    float speed;                    // Velocidad actual
    Vector2 targetPoint;            // Punto objetivo actual

    /// <summary>
    /// Inicializa el estado inicial del objeto (escondido).
    /// </summary>
    void Start()
    {
        targetPoint = hidePoint.position; // Comienza escondido
        speed = speedHide;
        timerHide = 0;
        timerShow = 0;
    }

    /// <summary>
    /// Actualiza el movimiento y controla los temporizadores de mostrar/esconder.
    /// </summary>
    void Update()
    {
        // Mueve el objeto hacia el punto objetivo
        objectToMove.transform.position = Vector2.MoveTowards(objectToMove.transform.position, targetPoint, speed * Time.deltaTime);

        // Si está en el punto de escondido
        if(Vector2.Distance(objectToMove.transform.position, hidePoint.position) < 0.1f){
            timerShow += Time.deltaTime;
            // Espera el tiempo necesario y si no está bloqueado, se muestra
            if(timerShow >= waitForShow && !Locked()){
                targetPoint = showPoint.position;
                speed = speedShow;
                timerShow = 0;
            }
        }
        // Si está en el punto de mostrado
        else if(Vector2.Distance(objectToMove.transform.position, showPoint.position) < 0.1f){
            timerHide += Time.deltaTime;
            // Espera el tiempo necesario y luego se esconde
            if(timerHide >= waitForHide){
                targetPoint = hidePoint.position;
                speed = speedHide;
                timerHide = 0;
            }
        }
    }

    /// <summary>
    /// Comprueba si hay algún objeto encima (por ejemplo, el jugador) para evitar que la planta salga.
    /// </summary>
    /// <returns>True si está bloqueado, false si puede salir.</returns>
    bool Locked(){
        return Physics2D.OverlapBox(transform.position + Vector3.up, Vector2.one, 0);
    }
}
