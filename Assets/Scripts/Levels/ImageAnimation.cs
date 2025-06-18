using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script que anima una imagen UI cambiando entre varios sprites como si fuera un GIF.
/// </summary>
public class ImageAnimator : MonoBehaviour
{
    public Sprite[] sprites;         // Array de sprites para la animación
    public float frameTime = 0.1f;   // Tiempo entre cada frame de la animación

    Image image;                     // Referencia al componente Image
    int animationFrame = 0;          // Índice del frame actual

    /// <summary>
    /// Inicializa la referencia al Image y comienza la animación.
    /// </summary>
    void Start()
    {
        image = GetComponent<Image>();
        InvokeRepeating("ChangeImage", frameTime, frameTime); // Llama a ChangeImage repetidamente cada frameTime segundos
    }

    /// <summary>
    /// Cambia el sprite mostrado por el siguiente en el array, creando la animación.
    /// </summary>
    void ChangeImage()
    {
        image.sprite = sprites[animationFrame]; // Asigna el sprite actual
        animationFrame++;                       // Avanza al siguiente frame
        if (animationFrame >= sprites.Length)   // Si llega al final,
        {
            animationFrame = 0;                 // Reinicia al primer frame
        }
    }
}
