using UnityEngine;

/// <summary>
/// Script que gestiona la reacción de un enemigo al ser golpeado por el jugador.
/// </summary>
public class PlayerHit : MonoBehaviour
{
    Animator animator; // Referencia al Animator

    /// <summary>
    /// Inicializa la referencia al Animator.
    /// </summary>
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Ejecuta la animación y lógica cuando el enemigo es golpeado por el jugador.
    /// </summary>
    public void Hit()
    {
        animator.SetTrigger("Hit"); // Activa la animación de golpe
        gameObject.layer = LayerMask.NameToLayer("OnlyGround"); // Cambia la capa para evitar más colisiones con el jugador
        Destroy(gameObject, 1f); // Destruye el objeto después de 1 segundo
        GetComponent<AutoMovement>().PauseMovement(); // Pausa el movimiento automático
    }
}
