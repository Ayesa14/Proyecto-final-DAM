using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Script que gestiona la zona de muerte (Kill Zone) del nivel.
/// Si el jugador cae en esta zona, se activa la lógica de muerte.
/// </summary>
public class KillZone : MonoBehaviour
{
    /// <summary>
    /// Detecta la entrada de un objeto en el trigger de la Kill Zone.
    /// </summary>
    /// <param name="collision">Collider que entra en el trigger.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si el objeto que entra es el jugador
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameManager.Instance.KillZone(); // Llama al método de muerte en el GameManager
        }
    }
}
