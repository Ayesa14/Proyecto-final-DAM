using UnityEngine;

/// <summary>
/// Script que gestiona el comportamiento de la bola de fuego lanzada por Mario.
/// Incluye movimiento, colisiones, rebotes y explosión.
/// </summary>
public class FireBall : MonoBehaviour
{
    public float direction;              // Dirección de la bola de fuego (1 para derecha, -1 para izquierda)
    public float speed;                  // Velocidad de la bola de fuego
    public float bounceForce;            // Fuerza de rebote al chocar con el suelo o techo
    public GameObject explosionPrefab;   // Prefab de la explosión al destruirse
    Rigidbody2D rb2D;                    // Referencia al Rigidbody2D

    bool colision;                       // Controla si ya ha habido una colisión para evitar dobles explosiones

    /// <summary>
    /// Inicializa la referencia al Rigidbody2D.
    /// </summary>
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Al iniciar, ajusta la velocidad y dirección inicial de la bola de fuego.
    /// </summary>
    void Start()
    {
        speed *= direction;
        rb2D.linearVelocity = new Vector2(speed, 0);
    }

    /// <summary>
    /// Actualiza la rotación y mantiene la velocidad horizontal constante.
    /// </summary>
    void Update()
    {
        transform.Rotate(0, 0, speed * Time.deltaTime * -45); // Gira la bola para animación
        rb2D.linearVelocity = new Vector2(speed, rb2D.linearVelocity.y); // Mantiene la velocidad horizontal
    }

    /// <summary>
    /// Gestiona las colisiones de la bola de fuego con enemigos, suelo, paredes y techo.
    /// </summary>
    /// <param name="collision">Información de la colisión</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        colision = true;
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.HitFireball(); // Si es enemigo, lo derrota
            Explode(collision.GetContact(0).point); // Explota la bola de fuego
        }
        else
        {
            Vector2 sidePoint = collision.GetContact(0).normal;

            if (Mathf.Abs(sidePoint.x) > 0.01f) // Colisión lateral (pared)
            {
                Explode(collision.GetContact(0).point);
            }
            else if (sidePoint.y > 0) // Colisión por abajo (suelo)
            {
                rb2D.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse); // Rebota hacia arriba
            }
            else if (sidePoint.y < 0) // Colisión por arriba (techo)
            {
                rb2D.AddForce(Vector2.down * bounceForce, ForceMode2D.Impulse); // Rebota hacia abajo
            }
            else
            {
                Explode(collision.GetContact(0).point);
            }
        }
    }

    /// <summary>
    /// Si la bola de fuego permanece en colisión, asegura que explote solo una vez.
    /// </summary>
    private void OnCollisionStay2D(Collision2D collision){
        if(colision){
            colision = false;
        }
        else {
            Explode(collision.GetContact(0).point);
        }
    }

    /// <summary>
    /// Instancia la explosión, reproduce el sonido y destruye la bola de fuego.
    /// </summary>
    /// <param name="point">Punto donde ocurre la explosión</param>
    void Explode(Vector2 point)
    {
        AudioManager.Instance.PlayBump(); // Sonido de explosión
        Instantiate(explosionPrefab, point, Quaternion.identity); // Instancia la explosión
        Destroy(gameObject); // Destruye la bola de fuego
    }
}
