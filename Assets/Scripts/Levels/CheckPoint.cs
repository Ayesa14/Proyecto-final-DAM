using UnityEngine;

/// <summary>
/// Script que gestiona el checkpoint del nivel. 
/// Cuando el jugador lo toca, marca que se ha alcanzado un checkpoint.
/// </summary>
public class CheckPoint : MonoBehaviour
{
    /// <summary>
    /// Detecta la colisión con el jugador.
    /// </summary>
    /// <param name="collision">Collider que entra en el trigger.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si el objeto que entra en el trigger es el jugador
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameManager.Instance.isLevelCheckPoint = true; // Marca el checkpoint como alcanzado
        }
    }
}
