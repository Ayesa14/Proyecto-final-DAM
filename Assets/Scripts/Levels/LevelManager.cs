using System.Collections;
using UnityEngine;

/// <summary>
/// Script que gestiona el estado general del nivel: tiempo, checkpoints, finalización y conversión de tiempo en puntos.
/// </summary>
public class LevelManager : MonoBehaviour
{

    public int time;                // Tiempo inicial del nivel (en segundos)
    public float timer;             // Tiempo actual restante

    Mario mario;                    // Referencia al jugador Mario

    public bool levelFinished;      // Indica si el nivel ha terminado

    public Transform startPoint;    // Punto de inicio del nivel
    public Transform checkPoint;    // Punto de checkpoint del nivel
    public CameraFollow cameraFollow; // Referencia al script de seguimiento de cámara
    public static LevelManager Instance; // Singleton para acceso global

    /// <summary>
    /// Inicializa el singleton.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    /// <summary>
    /// Inicializa el tiempo, referencias y notifica al GameManager que el nivel ha cargado.
    /// </summary>
    void Start()
    {
        timer = time; // Asigna el tiempo inicial
        GameManager.Instance.hud.UpdateTime(timer); // Actualiza el HUD con el tiempo inicial
        mario = FindAnyObjectByType<Mario>(); // Busca a Mario en la escena
        cameraFollow = Camera.main.GetComponent<CameraFollow>(); // Obtiene la cámara principal
        GameManager.Instance.LevelLoaded(); // Notifica que el nivel está listo
    }

    /// <summary>
    /// Actualiza el temporizador y el HUD mientras el nivel no ha terminado.
    /// </summary>
    void Update()
    {
        if (!levelFinished)
        {
            timer -= Time.deltaTime / 0.4f; // 1 segundo del juego = 0.4 segundos reales
            if (timer <= 0)
            {
                GameManager.Instance.OutOfTime(); // Llama a la lógica de tiempo agotado
                timer = 0;
            }
            GameManager.Instance.hud.UpdateTime(timer); // Actualiza el HUD con el tiempo restante
        }
    }

    /// <summary>
    /// Marca el nivel como terminado y comienza la conversión del tiempo restante en puntos.
    /// </summary>
    public void LevelFinished()
    {
        levelFinished = true;
        StartCoroutine(SecondsToPoints());
    }

    /// <summary>
    /// Corrutina que convierte el tiempo restante en puntos y lo muestra en el HUD.
    /// </summary>
    IEnumerator SecondsToPoints()
    {
        yield return new WaitForSeconds(1f); // Espera un segundo antes de empezar

        int timeLeft = Mathf.RoundToInt(timer); // Redondea el tiempo restante
        while (timeLeft > 0)
        {
            timeLeft--; // Resta un segundo
            timer = timeLeft; // Actualiza el temporizador
            GameManager.Instance.hud.UpdateTime(timer); // Actualiza el HUD
            ScoreManager.Instance.SumarPuntos(50); // Suma 50 puntos por cada segundo restante
            AudioManager.Instance.PlayCoin(); // Reproduce el sonido de moneda
            yield return new WaitForSeconds(0.05f); // Espera un poco entre cada suma de puntos
        }
    }
}
