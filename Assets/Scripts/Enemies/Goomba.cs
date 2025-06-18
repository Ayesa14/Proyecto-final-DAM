using UnityEngine;

/// <summary>
/// Clase que representa al enemigo Goomba. Hereda de Enemy y define su comportamiento al ser pisado.
/// </summary>
public class Goomba : Enemy
{
    /// <summary>
    /// Inicializa referencias heredadas.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// Lógica cuando el Goomba es pisado por el jugador.
    /// </summary>
    /// <param name="player">Transform del jugador que lo pisa.</param>
    public override void Stomped(Transform player)
    {
        AudioManager.Instance.PlayStomp(); // Reproduce el sonido de pisotón
        animator.SetTrigger("Hit"); // Activa la animación de ser golpeado
        gameObject.layer = LayerMask.NameToLayer("OnlyGround"); // Cambia la capa para evitar más colisiones con el jugador
        Destroy(gameObject, 1f); // Destruye el Goomba después de 1 segundo
        autoMovement.PauseMovement(); // Pausa el movimiento automático
        Dead(); // Suma puntos y muestra los puntos flotantes
    }
}
