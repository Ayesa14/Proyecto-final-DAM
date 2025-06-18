using UnityEngine;

/// <summary>
/// Script que detecta la colisión de la cabeza de Mario con la parte inferior de un bloque.
/// Llama al método correspondiente del bloque según si Mario es grande o pequeño.
/// </summary>
public class CollisionDetect : MonoBehaviour
{
    Block block; // Referencia al bloque padre

    /// <summary>
    /// Obtiene la referencia al componente Block en el objeto padre.
    /// </summary>
    private void Awake()
    {
        block = GetComponentInParent<Block>();
    }

    /// <summary>
    /// Detecta la colisión con el trigger (usado para la cabeza de Mario).
    /// </summary>
    /// <param name="collision">Collider que entra en el trigger.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si el objeto que colisiona tiene la etiqueta "HeadMario"
        if (collision.CompareTag("HeadMario"))
        {
            // Detiene la velocidad de Mario en Y para evitar bugs de salto
            collision.transform.parent.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;

            // Llama al método HeadCollision del bloque, indicando si Mario es grande o no
            if (collision.GetComponentInParent<Mario>().isBig())
            {
                block.HeadCollision(true);
            }
            else
            {
                block.HeadCollision(false);
            }
        }
    }
}
