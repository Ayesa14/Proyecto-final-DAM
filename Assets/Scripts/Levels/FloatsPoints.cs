using UnityEngine;

/// <summary>
/// Script que gestiona los puntos flotantes que aparecen al conseguir puntos (por ejemplo, al derrotar un enemigo o recoger una moneda).
/// Muestra el sprite correspondiente y lo hace subir antes de destruirlo.
/// </summary>
public class FloatsPoints : MonoBehaviour
{
    public int numPoints = 0;      // Cantidad de puntos a mostrar
    public float distance = 3f;    // Distancia que sube el texto de puntos
    public float speed = 2.5f;     // Velocidad de subida
    public bool destroy = true;    // Si se destruye automáticamente al llegar a la distancia

    float targetPos;               // Posición Y objetivo a la que debe llegar
    public Points[] points;        // Array de posibles puntos y sus sprites asociados
    SpriteRenderer spriteRenderer; // Referencia al SpriteRenderer

    /// <summary>
    /// Inicializa la referencia al SpriteRenderer.
    /// </summary>
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Al iniciar, muestra el sprite correspondiente y calcula la posición objetivo.
    /// </summary>
    void Start()
    {
        Show(numPoints); // Muestra el sprite según los puntos
        targetPos = transform.position.y + distance; // Calcula la posición Y objetivo
    }

    /// <summary>
    /// Actualiza la posición del objeto, haciéndolo subir hasta la posición objetivo y luego lo destruye.
    /// </summary>
    void Update()
    {
        if (transform.position.y < targetPos)
        {
            // Sube el objeto hacia arriba a la velocidad indicada
            transform.position = new Vector2(transform.position.x, transform.position.y + (speed * Time.deltaTime));
        }
        else if (destroy)
        {
            // Si llegó a la posición objetivo y debe destruirse, lo destruye
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Busca el sprite correspondiente a la cantidad de puntos y lo asigna.
    /// </summary>
    /// <param name="points">Cantidad de puntos a mostrar</param>
    void Show(int points)
    {
        for (int i = 0; i < this.points.Length; i++)
        {
            if(this.points[i].numPoints == points){
                spriteRenderer.sprite = this.points[i].sprite;
                break;
            }
        }
    }
}

/// <summary>
/// Clase auxiliar para asociar una cantidad de puntos con un sprite.
/// </summary>
[System.Serializable]
public class Points
{
    public int numPoints;   // Cantidad de puntos
    public Sprite sprite;   // Sprite que representa esa cantidad
}
