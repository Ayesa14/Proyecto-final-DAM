using UnityEngine;

/// <summary>
/// Script que gestiona el seguimiento de la cámara al jugador, con límites y desplazamiento anticipado.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    public Transform target;           // Referencia al objetivo a seguir (por ejemplo, Mario)
    public float followAhead = 2.5f;   // Distancia de anticipación de la cámara respecto al objetivo
    public float minPosX;              // Límite mínimo en X para la cámara
    public float maxPosX;              // Límite máximo en X para la cámara

    public Transform limitLeft;        // Transform que marca el límite izquierdo del nivel
    public Transform limitRight;       // Transform que marca el límite derecho del nivel

    public Transform collLeft;         // Collider izquierdo para colisiones fuera de cámara
    public Transform collRight;        // Collider derecho para colisiones fuera de cámara

    float camWidth;                    // Ancho de la cámara en unidades del mundo
    float lastPos;                     // Última posición X de la cámara

    /// <summary>
    /// Inicializa los límites y la posición inicial de la cámara.
    /// </summary>
    void Start()
    {
        camWidth = Camera.main.orthographicSize * Camera.main.aspect; // Calcula el ancho de la cámara
        minPosX = limitLeft.position.x + camWidth;                    // Límite izquierdo ajustado al ancho de la cámara
        maxPosX = limitRight.position.x - camWidth;                   // Límite derecho ajustado al ancho de la cámara
        lastPos = minPosX;                                            // Inicializa la última posición

        // Posiciona los colliders de los extremos fuera del área visible
        collLeft.position = new Vector2(transform.position.x - camWidth - 0.5f, collLeft.position.y);
        collRight.position = new Vector2(transform.position.x + camWidth + 0.5f, collRight.position.y);
    }

    /// <summary>
    /// Actualiza la posición de la cámara para seguir al objetivo, respetando los límites.
    /// </summary>
    void Update()
    {
        if (target != null)
        {
            // Calcula la nueva posición X anticipando el movimiento del objetivo
            float newPosX = target.position.x + followAhead;
            newPosX = Mathf.Clamp(newPosX, lastPos, maxPosX); // Limita la posición entre el último valor y el máximo
            transform.position = new Vector3(newPosX, transform.position.y, transform.position.z); // Mueve la cámara
            lastPos = newPosX; // Actualiza la última posición
        }
    }

    /// <summary>
    /// Calcula la posición ajustada de un objeto dentro de los límites de la cámara.
    /// </summary>
    /// <param name="pos">Posición X del objeto</param>
    /// <param name="width">Mitad del ancho del objeto</param>
    /// <param name="limitRight">Devuelve true si se alcanza el límite derecho</param>
    /// <param name="limitLeft">Devuelve true si se alcanza el límite izquierdo</param>
    /// <returns>Posición X ajustada</returns>
    public float PositionInCamera(float pos, float width, out bool limitRight, out bool limitLeft)
    {
        if (pos + width > maxPosX + camWidth)
        {
            limitLeft = false;
            limitRight = true;
            return (maxPosX + camWidth - width);
        }
        else if (pos - width < lastPos - camWidth)
        {
            limitLeft = true;
            limitRight = false;
            return (lastPos - camWidth + width);
        }
        limitLeft = false;
        limitRight = false;
        return pos;
    }

    /// <summary>
    /// Inicia el seguimiento de un nuevo objetivo.
    /// </summary>
    /// <param name="t">Transform del nuevo objetivo</param>
    public void StartFollow(Transform t)
    {
        target = t;
        float newPosX = target.position.x + followAhead;
        newPosX = Mathf.Clamp(newPosX, lastPos, maxPosX);
        transform.position = new Vector3(newPosX, transform.position.y, transform.position.z);
        lastPos = newPosX;
    }
}
