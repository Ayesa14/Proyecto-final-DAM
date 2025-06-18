using UnityEngine;

/// <summary>
/// Clase base para enemigos. Gestiona puntos, animaciones, movimiento automático y reacciones a golpes.
/// </summary>
public class Enemy : MonoBehaviour
{
    public int points; // Puntos que otorga al ser derrotado
    protected Animator animator; // Referencia al Animator
    protected AutoMovement autoMovement; // Referencia al script de movimiento automático
    protected Rigidbody2D rb2D; // Referencia al Rigidbody2D

    public GameObject floatPointsPrefab; // Prefab para mostrar los puntos flotantes

    /// <summary>
    /// Inicializa referencias a componentes.
    /// </summary>
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        autoMovement = GetComponent<AutoMovement>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Método virtual para lógica de actualización (puede ser sobrescrito por hijos).
    /// </summary>
    protected virtual void Update()
    {

    }

    /// <summary>
    /// Cambia la dirección del enemigo al colisionar con otro enemigo.
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si colisiona con otro objeto del mismo layer (otro enemigo)
        if (collision.gameObject.layer == gameObject.layer)
        {
            autoMovement.ChangeDirection();
        }
    }

    /// <summary>
    /// Método virtual llamado cuando el enemigo es pisado por el jugador.
    /// </summary>
    public virtual void Stomped(Transform player)
    {

    }

    /// <summary>
    /// Método virtual llamado cuando el enemigo es golpeado por una bola de fuego.
    /// </summary>
    public virtual void HitFireball()
    {
        FlipDie();
    }

    /// <summary>
    /// Método virtual llamado cuando el enemigo es golpeado por una estrella.
    /// </summary>
    public virtual void HitStarman()
    {
        FlipDie();
    }

    /// <summary>
    /// Método virtual llamado cuando el enemigo es golpeado desde abajo por un bloque.
    /// </summary>
    public virtual void HitBelowBlock()
    {
        FlipDie();
    }

    /// <summary>
    /// Método virtual llamado cuando el enemigo es golpeado por una concha rodante.
    /// </summary>
    public virtual void HitRollingShell()
    {
        FlipDie();
    }

    /// <summary>
    /// Ejecuta la animación y lógica de muerte "volteada" del enemigo.
    /// </summary>
    protected void FlipDie()
    {
        AudioManager.Instance.PlayFlipDie(); // Reproduce sonido de muerte
        animator.SetTrigger("Flip"); // Activa animación de voltear
        rb2D.linearVelocity = Vector2.zero; // Detiene el movimiento
        rb2D.AddForce(Vector2.up * 6, ForceMode2D.Impulse); // Aplica impulso hacia arriba
        if (autoMovement != null)
        {
            autoMovement.enabled = false; // Desactiva el movimiento automático
        }
        GetComponent<Collider2D>().enabled = false; // Desactiva el collider
        Dead(); // Ejecuta la lógica de muerte
    }

    /// <summary>
    /// Suma puntos, instancia los puntos flotantes y realiza limpieza.
    /// </summary>
    protected void Dead()
    {
        ScoreManager.Instance.SumarPuntos(points); // Suma los puntos al marcador
        GameObject newFloatPoints = Instantiate(floatPointsPrefab, transform.position, Quaternion.identity); // Instancia los puntos flotantes
        FloatsPoints floatsPoints = newFloatPoints.GetComponent<FloatsPoints>();
        floatsPoints.numPoints = points; // Asigna la cantidad de puntos a mostrar
    }
}
