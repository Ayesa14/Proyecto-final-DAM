using UnityEngine;

/// <summary>
/// Script que gestiona el mástil de meta (Goal Pole) al final del nivel.
/// Controla la bajada de la bandera, la suma de puntos según la altura y la animación de puntos flotantes.
/// </summary>
public class GoalPole : MonoBehaviour
{
    public Transform flag;                // Referencia a la bandera
    public Transform bottom;              // Punto inferior del mástil (donde termina la bandera)
    public float flagVelocity = 5f;       // Velocidad de bajada de la bandera
    public GameObject floatPointsPrefab;  // Prefab de los puntos flotantes
    bool downFlag;                       // Indica si la bandera está bajando
    Mover mover;                         // Referencia al script de movimiento de Mario

    /// <summary>
    /// Detecta la colisión con el jugador (Mario) al llegar al mástil.
    /// </summary>
    /// <param name="collision">Collider que entra en el trigger.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Mario mario = collision.GetComponent<Mario>();
        if (mario != null)
        {
            downFlag = true;                 // Activa la bajada de la bandera
            mario.Goal();                    // Llama al método de meta en Mario
            mover = collision.GetComponent<Mover>(); // Obtiene el script de movimiento de Mario
            Vector2 contactPoint = collision.ClosestPoint(transform.position); // Punto de contacto con el mástil
            CalculateHeight(contactPoint.y); // Calcula los puntos según la altura alcanzada
        }
    }

    /// <summary>
    /// En cada FixedUpdate, baja la bandera si corresponde.
    /// </summary>
    private void FixedUpdate()
    {
        if (downFlag)
        {
            // Si la bandera no ha llegado al fondo, sigue bajando
            if (flag.position.y > bottom.position.y)
            {
                flag.position = new Vector2(flag.position.x, flag.position.y - (flagVelocity * Time.fixedDeltaTime));
            }
            else
            {
                mover.isFlagDown = true; // Marca que la bandera ha llegado al fondo
            }
        }
    }

    /// <summary>
    /// Calcula la cantidad de puntos según la altura a la que Mario toca el mástil.
    /// Instancia los puntos flotantes y ajusta su animación.
    /// </summary>
    /// <param name="marioPosition">Posición Y de Mario al tocar el mástil</param>
    void CalculateHeight(float marioPosition)
    {
        float size = GetComponent<BoxCollider2D>().bounds.size.y;

        // Calcula los diferentes rangos de altura para los puntos
        float minPosition1 = transform.position.y + (size - size / 5f); // 5000 puntos
        float minPosition2 = transform.position.y + (size - 2 * size / 5f); // 2000 puntos
        float minPosition3 = transform.position.y + (size - 3 * size / 5f); // 800 puntos
        float minPosition4 = transform.position.y + (size - 4 * size / 5f); // 400 puntos

        int numPoints = 0;
        // Asigna los puntos según la altura alcanzada
        if (marioPosition >= minPosition1)
        {
            numPoints = 5000;
        }
        else if (marioPosition >= minPosition2)
        {
            numPoints = 2000;
        }
        else if (marioPosition >= minPosition3)
        {
            numPoints = 800;
        }
        else if (marioPosition >= minPosition4)
        {
            numPoints = 400;
        }
        else
        {
            numPoints = 100;
        }
        ScoreManager.Instance.SumarPuntos(numPoints); // Suma los puntos al marcador

        // Instancia los puntos flotantes en la base del mástil
        Vector2 positionFloatPoints = new Vector2(transform.position.x + 0.65f, bottom.position.y);
        GameObject newFloatPoints = Instantiate(floatPointsPrefab, positionFloatPoints, Quaternion.identity);
        FloatsPoints floatsPoints = newFloatPoints.GetComponent<FloatsPoints>();
        floatsPoints.numPoints = numPoints; // Asigna la cantidad de puntos
        floatsPoints.speed = flagVelocity;  // Ajusta la velocidad de subida de los puntos flotantes
        floatsPoints.distance = flag.position.y - bottom.position.y; // Distancia que deben recorrer los puntos flotantes
        floatsPoints.destroy = false;       // No destruir inmediatamente
    }
}
