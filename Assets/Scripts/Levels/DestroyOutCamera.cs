using UnityEngine;

/// <summary>
/// Script que destruye el objeto cuando sale de la vista de la cámara.
/// Útil para eliminar enemigos u objetos que ya no son visibles.
/// </summary>
public class DestroyOutCamera : MonoBehaviour
{
    SpriteRenderer spriteRenderer; // Referencia al SpriteRenderer del objeto
    bool hasBeenVisible;           // Indica si el objeto ya fue visible alguna vez

    public bool onlyBack;          // Si es true, solo destruye si el objeto queda atrás de la cámara
    public float minDistance = 0;  // Distancia mínima desde la cámara para permitir la destrucción

    /// <summary>
    /// Inicializa la referencia al SpriteRenderer.
    /// </summary>
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// En cada frame, verifica si el objeto está fuera de la cámara y lo destruye si corresponde.
    /// </summary>
    void Update()
    {
        if(spriteRenderer.isVisible){
            hasBeenVisible = true; // Marca que el objeto ya fue visible
        }
        else {
            // Si ya fue visible y ahora no lo es
            if(hasBeenVisible){
                // Si está suficientemente lejos de la cámara
                if(Mathf.Abs(transform.position.x - Camera.main.transform.position.x) > minDistance){
                    if(onlyBack){
                        // Si solo debe destruirse cuando queda atrás de la cámara
                        if(transform.position.x > Camera.main.transform.position.x){
                            return; // No destruye si está delante de la cámara
                        }
                    }
                    Destroy(gameObject); // Destruye el objeto
                }                
            }
        }
    }
}
