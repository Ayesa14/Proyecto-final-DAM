using System.Collections;
using UnityEngine;

/// <summary>
/// Script que anima un objeto cambiando entre varios sprites, como si fuera un GIF.
/// Permite animación en bucle o una sola vez, y puede detenerse.
/// </summary>
public class SpritesAnimation : MonoBehaviour
{
    public Sprite[] sprites;           // Array de sprites para la animación
    public float frameTime = 0.1f;     // Tiempo entre cada frame de la animación
    int animationFrame = 0;            // Índice del frame actual
    public bool stop;                  // Si es true, detiene la animación
    public bool loop = true;           // Si es true, la animación se repite en bucle
    SpriteRenderer spriteRenderer;     // Referencia al SpriteRenderer

    /// <summary>
    /// Inicializa la referencia al SpriteRenderer.
    /// </summary>
    private void Awake()
    {
       spriteRenderer = GetComponent<SpriteRenderer>(); 
    }

    /// <summary>
    /// Inicia la corrutina de animación al comenzar.
    /// </summary>
    void Start()
    {
        StartCoroutine(Animation());
    }

    /// <summary>
    /// Corrutina que gestiona la animación de los sprites.
    /// Si loop es true, repite la animación hasta que stop sea true.
    /// Si loop es false, reproduce la animación una vez y destruye el objeto.
    /// </summary>
    IEnumerator Animation()
    {
        if (loop)
        {
            while (!stop)
            {
                spriteRenderer.sprite = sprites[animationFrame];
                animationFrame++;
                if (animationFrame >= sprites.Length)
                {
                    animationFrame = 0;
                }
                yield return new WaitForSeconds(frameTime);
            }
        }
        else
        {
            while (animationFrame < sprites.Length)
            {
                spriteRenderer.sprite = sprites[animationFrame];
                animationFrame++;
                yield return new WaitForSeconds(frameTime);
            }
            Destroy(gameObject); // Destruye el objeto al terminar la animación
        }       
    }
}
