using UnityEngine;

/// <summary>
/// Script para movimiento automático de enemigos (como Goombas o Koopas).
/// Gestiona la activación, pausa, cambio de dirección y animación del sprite.
/// </summary>
public class AutoMovement : MonoBehaviour
{
    public float speed = 1f; // Velocidad de movimiento
    bool movementPaused; // Indica si el movimiento está pausado
    Rigidbody2D rb2D; // Referencia al Rigidbody2D
    SpriteRenderer spriteRenderer; // Referencia al SpriteRenderer
    Vector2 lastVelocity; // Última velocidad antes de pausar
    Vector2 currentDirection; // Dirección actual de movimiento
    float defaultSpeed; // Velocidad por defecto (valor absoluto)
    public bool flipSprite = true; // Si se debe voltear el sprite según la dirección

    bool hasBeenVisible; // Indica si el enemigo ya fue visible en pantalla
    public AutoMovement partner; // Referencia a otro enemigo para activar juntos

    float timer = 0; // Temporizador para cambio de dirección

    /// <summary>
    /// Inicializa referencias a componentes.
    /// </summary>
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Configura el estado inicial del enemigo.
    /// </summary>
    private void Start()
    {
        // El enemigo inicia como kinemático y pausado hasta que sea visible
        defaultSpeed = Mathf.Abs(speed);
        rb2D.bodyType = RigidbodyType2D.Kinematic;
        movementPaused = true;
    }

    /// <summary>
    /// Activa el movimiento del enemigo cuando es visible en pantalla.
    /// </summary>
    public void Activate()
    {
        hasBeenVisible = true;
        rb2D.bodyType = RigidbodyType2D.Dynamic;
        rb2D.linearVelocity = new Vector2(speed, rb2D.linearVelocity.y);
        movementPaused = false;
        // Si tiene un compañero, lo activa también
        if (partner != null)
        {
            partner.Activate();
        }
    }

    /// <summary>
    /// En cada frame, verifica si el enemigo ya es visible para activarlo.
    /// </summary>
    private void Update()
    {
        if (spriteRenderer.isVisible && !hasBeenVisible)
        {
            Activate();
        }
    }

    /// <summary>
    /// Lógica de movimiento automático y cambio de dirección en colisiones.
    /// </summary>
    private void FixedUpdate()
    {
        if (!movementPaused)
        {
            // Si la velocidad en X es casi cero, invierte la dirección tras un pequeño tiempo
            if (rb2D.linearVelocity.x > -0.1f && rb2D.linearVelocity.x < 0.1f)
            {
                if (timer > 0.05f)
                {
                    speed = -speed;
                }
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0;
            }
            // Aplica la velocidad en X
            rb2D.linearVelocity = new Vector2(speed, rb2D.linearVelocity.y);

            // Voltea el sprite según la dirección si está activado
            if (!flipSprite)
            {
                if (rb2D.linearVelocity.x > 0)
                {
                    spriteRenderer.flipX = true;
                }
                else
                {
                    spriteRenderer.flipX = false;
                }
            }
        }
    }

    /// <summary>
    /// Pausa el movimiento del enemigo, guardando su dirección y velocidad.
    /// </summary>
    public void PauseMovement()
    {
        if (!movementPaused)
        {
            currentDirection = rb2D.linearVelocity.normalized;
            lastVelocity = rb2D.linearVelocity;
            movementPaused = true;
            rb2D.linearVelocity = new Vector2(0, 0);
        }
    }

    /// <summary>
    /// Continúa el movimiento usando la dirección y velocidad guardadas.
    /// </summary>
    public void ContinueMovement()
    {
        if (movementPaused)
        {
            speed = defaultSpeed * currentDirection.x;
            rb2D.linearVelocity = new Vector2(speed, lastVelocity.y);
            movementPaused = false;
        }
    }

    /// <summary>
    /// Continúa el movimiento con una nueva velocidad específica.
    /// </summary>
    public void ContinueMovement(Vector2 newVelocity)
    {
        if (movementPaused)
        {
            rb2D.linearVelocity = newVelocity;
            movementPaused = false;
        }
    }

    /// <summary>
    /// Cambia la dirección de movimiento (por ejemplo, al chocar con una pared).
    /// </summary>
    public void ChangeDirection()
    {
        speed = -speed;
        rb2D.linearVelocity = new Vector2(speed, rb2D.linearVelocity.y);
    }
}
