using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script principal que gestiona el estado y comportamiento de Mario.
/// Controla power-ups, daño, disparo, muerte, respawn y reacciones a ítems.
/// </summary>
public class Mario : MonoBehaviour
{
    public GameObject stompBox;         // Caja de colisión para pisar enemigos
    Mover mover;                        // Referencia al script de movimiento
    Colisiones colisiones;              // Referencia al script de colisiones
    Animaciones animaciones;            // Referencia al script de animaciones
    Rigidbody2D rb2D;                   // Referencia al Rigidbody2D
    public GameObject fireBallPrefab;   // Prefab de la bola de fuego
    public Transform shootPos;          // Posición desde donde dispara la bola de fuego
    public bool isInvincible;           // Indica si Mario está en modo invencible
    public float invincibleTime;        // Duración de la invencibilidad
    float invincibleTimer;              // Temporizador de invencibilidad

    public bool isHurt;                 // Indica si Mario está herido
    public float hurtTime;              // Tiempo de estado herido
    float hurtTimer;                    // Temporizador de herido
    public bool isCrouched;             // Indica si Mario está agachado
    
    bool isDead;                        // Indica si Mario está muerto
    enum State { Default = 0, Super = 1, Fire = 2 } // Estados posibles de Mario
    State currentState = State.Default; // Estado actual

    public static Mario Instance;       // Singleton para acceso global

    /// <summary>
    /// Inicializa referencias y el singleton.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            mover = GetComponent<Mover>();
            colisiones = GetComponent<Colisiones>();
            animaciones = GetComponent<Animaciones>();
            rb2D = GetComponent<Rigidbody2D>();
            DontDestroyOnLoad(gameObject); // Mantiene a Mario entre escenas
        }
        else
        {
            Destroy(gameObject); // Evita duplicados
        }
    }

    /// <summary>
    /// Lógica principal de entrada y estados especiales de Mario.
    /// </summary>
    private void Update()
    {
        isCrouched = false;
        if (!isDead)
        {
            // Activa el stompBox solo cuando Mario cae
            if (rb2D.linearVelocity.y < 0)
            {
                stompBox.SetActive(true);
            }
            else
            {
                stompBox.SetActive(false);
            }
            // Agacharse si está en el suelo y se pulsa la flecha abajo
            if (Input.GetKey(KeyCode.DownArrow))
            {
                if (colisiones.Grounded())
                {
                    isCrouched = true;
                }
            }
            // Disparar bola de fuego si pulsa Z
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Shoot();
            }
            // Temporizador de invencibilidad
            if (isInvincible)
            {
                invincibleTimer -= Time.deltaTime;
                if (invincibleTimer <= 0)
                {
                    isInvincible = false;
                    animaciones.InvincibleMode(false);
                }
            }
            // Temporizador de daño
            if (isHurt)
            {
                hurtTimer -= Time.deltaTime;
                if (hurtTimer <= 0)
                {
                    EndHurt();
                }
            }
        }
        animaciones.Crouch(isCrouched); // Actualiza la animación de agachado
    }

    /// <summary>
    /// Lógica cuando Mario recibe daño.
    /// </summary>
    public void Hit()
    {
        if (!isHurt)
        {
            if (currentState == State.Default)
            {
                Dead(); // Si es pequeño, muere
            }
            else
            {
                AudioManager.Instance.PlayPowerDown(); // Sonido de perder power-up
                Time.timeScale = 0;                    // Pausa el juego para la animación
                animaciones.Hit();                     // Activa animación de daño
                StartHurt();                           // Inicia estado de herido
            }
        }
    }

    /// <summary>
    /// Inicia el estado de herido.
    /// </summary>
    void StartHurt()
    {
        isHurt = true;
        animaciones.Hurt(true);
        hurtTimer = hurtTime;
        colisiones.HurtCollision(true); // Cambia la capa para evitar colisiones
    }

    /// <summary>
    /// Finaliza el estado de herido.
    /// </summary>
    void EndHurt()
    {
        isHurt = false;
        animaciones.Hurt(false);
        colisiones.HurtCollision(false); // Restaura la capa
    }

    /// <summary>
    /// Lógica cuando Mario muere.
    /// </summary>
    public void Dead()
    {
        if (!isDead)
        {
            AudioManager.Instance.PlayDie(); // Sonido de muerte
            isDead = true;
            colisiones.Dead();               // Cambia la capa a "PlayerDead"
            mover.Dead();                    // Lógica de muerte en movimiento
            animaciones.Dead();              // Animación de muerte
            GameManager.Instance.LoseLife(); // Resta una vida
        }
    }

    /// <summary>
    /// Lógica cuando Mario reaparece tras morir.
    /// </summary>
    /// <param name="pos">Posición donde reaparece</param>
    public void Respawn(Vector2 pos)
    {
        isDead = false;
        colisiones.Respawn();    // Restaura la capa
        mover.Respawn();         // Restaura el movimiento
        animaciones.Reset();     // Restaura animaciones
        transform.position = pos; // Coloca a Mario en la posición indicada
    }

    /// <summary>
    /// Cambia el estado de Mario (pequeño, grande, fuego).
    /// </summary>
    /// <param name="newState">Nuevo estado</param>
    void ChangeState(int newState)
    {
        currentState = (State)newState;
        animaciones.NewState(newState);
        Time.timeScale = 1; // Restaura la velocidad del juego
    }

    /// <summary>
    /// Mario recoge un ítem y aplica su efecto.
    /// </summary>
    /// <param name="type">Tipo de ítem</param>
    public void CatchItem(ItemType type)
    {
        switch (type)
        {
            case ItemType.MagicMushroom:
                AudioManager.Instance.PlayPowerUp();
                if (currentState == State.Default)
                {
                    animaciones.PowerUp();
                    Time.timeScale = 0; // Pausa para animación de power-up
                }
                break;

            case ItemType.Coin:
                AudioManager.Instance.PlayCoin();
                GameManager.Instance.AddCoins();
                break;

            case ItemType.FireFlower:
                AudioManager.Instance.PlayPowerUp();
                if (currentState != State.Fire)
                {
                    animaciones.PowerUp();
                    Time.timeScale = 0; // Pausa para animación de power-up
                }
                break;

            case ItemType.Life:
                GameManager.Instance.NewLife();                
                break;

            case ItemType.Star:
                AudioManager.Instance.PlayPowerUp();
                isInvincible = true;
                animaciones.InvincibleMode(true);
                invincibleTimer = invincibleTime;
                EndHurt(); // Sale del estado de herido si lo estaba
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Dispara una bola de fuego si Mario está en estado Fire y no está agachado.
    /// </summary>
    void Shoot()
    {
        if (currentState == State.Fire && !isCrouched)
        {
            AudioManager.Instance.PlayShoot();
            GameObject newFireBall = Instantiate(fireBallPrefab, shootPos.position, Quaternion.identity);
            newFireBall.GetComponent<FireBall>().direction = transform.localScale.x;
            animaciones.Shoot();
        }
    }

    /// <summary>
    /// Devuelve true si Mario es grande (no está en estado Default).
    /// </summary>
    public bool isBig()
    {
        return currentState != State.Default;
    }

    /// <summary>
    /// Lógica al llegar a la meta.
    /// </summary>
    public void Goal()
    {
        AudioManager.Instance.PlayFlagPole(); // Sonido de meta
        mover.DownFlagPole();                 // Mario baja por el mástil
        LevelManager.Instance.LevelFinished();// Marca el nivel como terminado
    }
}
