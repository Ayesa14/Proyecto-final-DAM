using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Enum que define los tipos de ítems disponibles.
/// </summary>
public enum ItemType {MagicMushroom, FireFlower, Coin, Life, Star}

/// <summary>
/// Script que gestiona el comportamiento de los ítems del juego (setas, flores, monedas, vidas, estrellas).
/// </summary>
public class Item : MonoBehaviour
{
    public ItemType type;                  // Tipo de ítem
    bool isCatched;                        // Indica si el ítem ya fue recogido
    public int points;                     // Puntos que otorga al recogerlo
    public Vector2 startVelocity;          // Velocidad inicial al aparecer
    AutoMovement autoMovement;             // Referencia al movimiento automático (si aplica)
    public GameObject floatPointsPrefab;   // Prefab de los puntos flotantes

    /// <summary>
    /// Inicializa la referencia al AutoMovement.
    /// </summary>
    private void Awake()
    {
        autoMovement = GetComponent<AutoMovement>();
    }

    /// <summary>
    /// (Opcional) Lógica al iniciar el ítem.
    /// </summary>
    private void Start()
    {
        //Invoke("StartMove", 5f); // Si quieres que el ítem empiece a moverse tras un tiempo
    }

    /// <summary>
    /// Detecta la colisión física con el jugador para recoger el ítem.
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isCatched)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                isCatched = true; // Marca como recogido
                collision.gameObject.GetComponent<Mario>().CatchItem(type); // Aplica el efecto al jugador
                //Destroy(gameObject);
                CatchItem(); // Suma puntos y muestra puntos flotantes
            }
        }
    }

    /// <summary>
    /// Detecta la colisión por trigger con el jugador para recoger el ítem.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if(collision.gameObject.layer == LayerMask.NameToLayer("Player")){
        //     collision.gameObject.GetComponent<Mario>().CatchItem(type);
        //     Destroy(gameObject);
        // }
        if (!isCatched)
        {
            Mario mario = collision.gameObject.GetComponent<Mario>();
            if (mario != null)
            {
                // isCatched = true;
                mario.CatchItem(type); // Aplica el efecto al jugador
                //Destroy(gameObject);
                CatchItem(); // Suma puntos y muestra puntos flotantes
            }
        }
    }

    /// <summary>
    /// Desactiva el movimiento automático del ítem (por ejemplo, mientras sale del bloque).
    /// </summary>
    public void WaitMove()
    {
        if (autoMovement != null)
        {
            autoMovement.enabled = false;
        }
    }

    /// <summary>
    /// Activa el movimiento automático o aplica una velocidad inicial si no tiene AutoMovement.
    /// </summary>
    public void StartMove()
    {
        if (autoMovement != null)
        {
            autoMovement.enabled = true;
        }
        else
        {
            if (startVelocity != Vector2.zero)
            {
                GetComponent<Rigidbody2D>().linearVelocity = startVelocity;
            }
        }
    }

    /// <summary>
    /// Lógica cuando el ítem es golpeado desde abajo por un bloque.
    /// Cambia de dirección si tiene movimiento automático.
    /// </summary>
    public void HitBelowBlock()
    {
        if (autoMovement != null && autoMovement.enabled)
        {
            autoMovement.ChangeDirection();
        }
    }

    /// <summary>
    /// Suma puntos, muestra los puntos flotantes y destruye el ítem.
    /// </summary>
    void CatchItem()
    {
        ScoreManager.Instance.SumarPuntos(points); // Suma los puntos al marcador
        GameObject newFloatPoints = Instantiate(floatPointsPrefab, transform.position, Quaternion.identity); // Instancia los puntos flotantes
        FloatsPoints floatsPoints = newFloatPoints.GetComponent<FloatsPoints>();
        floatsPoints.numPoints = points; // Asigna la cantidad de puntos
        Destroy(gameObject); // Destruye el ítem
    }
}

