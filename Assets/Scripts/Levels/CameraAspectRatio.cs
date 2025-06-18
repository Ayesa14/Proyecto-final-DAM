using UnityEngine;

/// <summary>
/// Script que ajusta la cámara para mantener una relación de aspecto específica (por ejemplo, 16:15).
/// Añade barras laterales si la pantalla no coincide con la relación deseada.
/// </summary>
public class CameraAspectRatio : MonoBehaviour
{
    public Vector2 targetAspects = new Vector2(16f, 15f); // Relación de aspecto objetivo (ancho:alto)

    /// <summary>
    /// Al iniciar, ajusta el rectángulo de la cámara para mantener la relación de aspecto deseada.
    /// </summary>
    private void Awake()
    {
        float targetAspect = targetAspects.x / targetAspects.y; // Calcula la relación de aspecto objetivo
        float screenAspect = (float)Screen.width / (float)Screen.height; // Calcula la relación de aspecto de la pantalla actual

        float scaleHeight = screenAspect / targetAspect; // Escala de altura respecto al objetivo
        float scaleWidth = 1f / scaleHeight; // Escala de ancho respecto al objetivo

        Camera cam = GetComponent<Camera>(); // Obtiene la cámara

        Rect rect = cam.rect; // Obtiene el rectángulo actual de la cámara
        rect.width = scaleWidth; // Ajusta el ancho del rectángulo
        rect.height = 1.0f; // Mantiene la altura al 100%
        rect.x = (1.0f - scaleWidth) / 2.0f; // Centra horizontalmente la cámara
        rect.y = 0f; // Mantiene la posición vertical en 0
        cam.rect = rect; // Asigna el nuevo rectángulo a la cámara
    }
}
