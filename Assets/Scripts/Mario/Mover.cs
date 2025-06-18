using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script que gestiona el movimiento de Mario: caminar, saltar, fricción, velocidad, animaciones y lógica especial.
/// </summary>
public class Mover : MonoBehaviour
{
    // Enum para la dirección de movimiento
    enum Direction { Left = -1, None = 0, Right = 1 };
    Direction currentDirection = Direction.None;

    // Parámetros de movimiento
    public float speed;               // Velocidad base
    public float acceleration;        // Aceleración al moverse
    public float maxVelocity;         // Velocidad máxima
    public float friction;            // Fricción al dejar de moverse
    float currentVelocity = 0f;       // Velocidad actual

    // Parámetros de salto
    public float jumpForce;           // Fuerza del salto
    public float maxJumpingTime = 1f; // Tiempo máximo de salto prolongado
    public bool isJumping;            // Indica si está saltando
    float jumpTimer = 0;              // Temporizador de salto
    float defaultGravity;             // Gravedad por defecto
    public bool isSkidding;           // Indica si está derrapando

    // Referencias a componentes y estados
    public Rigidbody2D rb2D;          // Referencia al Rigidbody2D
    Colisiones colisiones;            // Referencia al script de colisiones
    public bool inputMoveEnabled = true; // Si se permite el movimiento por input
    public GameObject headBox;        // Caja de colisión para la cabeza (golpear bloques)
    Animaciones animaciones;          // Referencia al script de animaciones
    bool isClimbingFlagPole = false;  // Indica si está bajando por el mástil
    bool isAutoWalk;                  // Indica si está en modo auto-caminar (tras meta)
    public float autoWalkSpeed = 5;   // Velocidad de auto-caminar
    Mario mario;                      // Referencia al script principal de Mario
    public float climbPoleSpeed = 5;  // Velocidad al bajar el mástil
    public bool isFlagDown;           // Indica si la bandera ya ha bajado

    // CameraFollow cameraFollow;
    SpriteRenderer spriteRenderer;    // Referencia al SpriteRenderer

    /// <summary>
    /// Inicializa referencias y valores por defecto.
    /// </summary>
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        colisiones = GetComponent<Colisiones>();
        animaciones = GetComponent<Animaciones>();
        mario = GetComponent<Mario>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb2D.gravityScale = 3f; // Valor inicial de gravedad
    }

    /// <summary>
    /// Inicializa valores al empezar la escena.
    /// </summary>
    void Start()
    {
        defaultGravity = rb2D.gravityScale;
        // cameraFollow = Camera.main.GetComponent<CameraFollow>();
    }

    /// <summary>
    /// Lógica de entrada y control de salto, dirección y límites de cámara.
    /// </summary>
    void Update()
    {
        bool grounded = colisiones.Grounded();
        animaciones.Grounded(grounded);

        // Si el nivel terminó y Mario está en el mástil, salta del mástil
        if (LevelManager.Instance.levelFinished)
        {
            if (grounded && isClimbingFlagPole)
            {
                StartCoroutine(JumpOffPole());
            }
        }
        else
        {
            headBox.SetActive(false);

            // Lógica de salto variable
            if (isJumping)
            {
                if (rb2D.linearVelocity.y > 0f)
                {
                    headBox.SetActive(true);
                    if (Input.GetKey(KeyCode.Space))
                    {
                        jumpTimer += Time.deltaTime;
                    }
                    if (Input.GetKeyUp(KeyCode.Space))
                    {
                        if (jumpTimer < maxJumpingTime)
                        {
                            rb2D.gravityScale = defaultGravity * 3f;
                        }
                    }
                }
                else
                {
                    rb2D.gravityScale = defaultGravity;
                    if (colisiones.Grounded())
                    {
                        isJumping = false;
                        jumpTimer = 0;
                        animaciones.Jumping(false);
                        rb2D.gravityScale = defaultGravity;
                    }
                }
            }

            // Entrada de movimiento
            currentDirection = Direction.None;
            if (inputMoveEnabled)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (grounded)
                    {
                        Jump();
                    }
                }
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    currentDirection = Direction.Left;
                }
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    currentDirection = Direction.Right;
                }
            }
        }

        // Lógica de límites de cámara
        bool limitRight;
        bool limitLeft;
        if (LevelManager.Instance.cameraFollow != null)
        {
            float posX = LevelManager.Instance.cameraFollow.PositionInCamera(transform.position.x, spriteRenderer.bounds.extents.x, out limitRight, out limitLeft);
            if (limitRight && (currentDirection == Direction.Right || currentDirection == Direction.None))
            {
                rb2D.linearVelocity = new Vector2(0, rb2D.linearVelocity.y);
            }
            else if (limitLeft && (currentDirection == Direction.Left || currentDirection == Direction.None))
            {
                rb2D.linearVelocity = new Vector2(0, rb2D.linearVelocity.y);
            }
            transform.position = new Vector2(posX, transform.position.y);
            
        }
        
    }

    /// <summary>
    /// Lógica física de movimiento, aceleración, fricción y animaciones.
    /// </summary>
    private void FixedUpdate()
    {
        if (LevelManager.Instance.levelFinished)
        {
            if (isClimbingFlagPole)
            {
                // Mario baja por el mástil
                rb2D.MovePosition(rb2D.position + Vector2.down * climbPoleSpeed * Time.fixedDeltaTime);
            }
            else if (isAutoWalk)
            {
                // Mario camina automáticamente tras llegar a la meta
                Vector2 linearVelocity = new Vector2(currentVelocity, rb2D.linearVelocity.y);
                rb2D.linearVelocity = linearVelocity;
                animaciones.Velocity(Mathf.Abs(currentVelocity));
            }
        }
        else
        {
            isSkidding = false;
            currentVelocity = rb2D.linearVelocity.x;

            // Movimiento a la derecha
            if (currentDirection > 0)
            {
                if (currentVelocity < 0)
                {
                    currentVelocity += (acceleration + friction) * Time.deltaTime;
                    isSkidding = true;
                }
                else if (currentVelocity < maxVelocity)
                {
                    currentVelocity += acceleration * Time.deltaTime;
                    transform.localScale = new Vector2(1, 1);
                }

            }
            // Movimiento a la izquierda
            else if (currentDirection < 0)
            {
                if (currentVelocity > 0)
                {
                    currentVelocity -= (acceleration + friction) * Time.deltaTime;
                    isSkidding = true;
                }
                else if (currentVelocity > -maxVelocity)
                {
                    currentVelocity -= acceleration * Time.deltaTime;
                    transform.localScale = new Vector2(-1, 1);
                }
            }
            // Sin movimiento, aplicar fricción
            else
            {
                if (currentVelocity > 1f)
                {
                    currentVelocity -= friction * Time.deltaTime;
                }
                else if (currentVelocity < -1f)
                {
                    currentVelocity += friction * Time.deltaTime;
                }
                else
                {
                    currentVelocity = 0f;
                }
            }

            // Si Mario está agachado, no se mueve
            if (mario.isCrouched)
            {
                currentVelocity = 0;
            }
            Vector2 linearVelocity = new Vector2(currentVelocity, rb2D.linearVelocity.y);
            rb2D.linearVelocity = linearVelocity;

            animaciones.Velocity(currentVelocity);
            animaciones.Skid(isSkidding);
        }
        
    }

    /// <summary>
    /// Ejecuta el salto de Mario.
    /// </summary>
    void Jump()
    {
        if (!isJumping)
        {
            if (mario.isBig())
            {
                AudioManager.Instance.PlayBigJump();
            }
            else
            {
                AudioManager.Instance.PlayJump();
            }
            
            isJumping = true;
            Vector2 fuerza = new Vector2(0, jumpForce);
            rb2D.AddForce(fuerza, ForceMode2D.Impulse);
            animaciones.Jumping(true);
        }
    }

    /// <summary>
    /// Mueve a Mario a la derecha (usado en auto-walk).
    /// </summary>
    void MoveRight()
    {
        Vector2 linearVelocity = new Vector2(1f, 0f);
        rb2D.linearVelocity = linearVelocity;
    }

    /// <summary>
    /// Lógica cuando Mario muere.
    /// </summary>
    public void Dead()
    {
        inputMoveEnabled = false;
        rb2D.linearVelocity = Vector2.zero;
        rb2D.gravityScale = 1;
        rb2D.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
    }

    /// <summary>
    /// Lógica cuando Mario reaparece.
    /// </summary>
    public void Respawn()
    {
        inputMoveEnabled = true;
        rb2D.linearVelocity = Vector2.zero;
        rb2D.gravityScale = defaultGravity;
        transform.localScale = Vector2.one;
    }

    /// <summary>
    /// Hace que Mario rebote hacia arriba (por ejemplo, al pisar un enemigo).
    /// </summary>
    public void BounceUp()
    {
        rb2D.linearVelocity = Vector2.zero;
        rb2D.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
    }

    /// <summary>
    /// Lógica para bajar por el mástil de meta.
    /// </summary>
    public void DownFlagPole()
    {
        inputMoveEnabled = false;
        // rb2D.isKinematic = true;
        rb2D.bodyType = RigidbodyType2D.Kinematic;
        rb2D.linearVelocity = new Vector2(0, 0f);
        isClimbingFlagPole = true;
        isJumping = false;
        animaciones.Jumping(false);
        animaciones.Climb(true);
        transform.position = new Vector2(transform.position.x + 0.1f, transform.position.y);
    }

    /// <summary>
    /// Corrutina para saltar del mástil al terminar el nivel.
    /// </summary>
    IEnumerator JumpOffPole()
    {
        isClimbingFlagPole = false;
        rb2D.linearVelocity = Vector2.zero;
        animaciones.Pause();
        yield return new WaitForSeconds(0.25f);

        while (!isFlagDown)
        {
            yield return null;
        }
        transform.position = new Vector2(transform.position.x + 0.5f, transform.position.y);
        GetComponent<SpriteRenderer>().flipX = true;
        yield return new WaitForSeconds(0.25f);

        animaciones.Climb(false);
        // rb2D.isKinematic = false;
        rb2D.bodyType = RigidbodyType2D.Dynamic;
        animaciones.Continue();
        GetComponent<SpriteRenderer>().flipX = false;
        isAutoWalk = true;
        currentVelocity = autoWalkSpeed;
    }
}


