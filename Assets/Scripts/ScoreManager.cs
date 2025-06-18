using UnityEngine;

/// <summary>
/// Script que gestiona la puntuación del jugador.
/// Implementa el patrón Singleton para acceso global.
/// </summary>
public class ScoreManager : MonoBehaviour
{
    public int puntos;                      // Puntuación actual del jugador
    public static ScoreManager Instance;    // Singleton para acceso global

    /// <summary>
    /// Inicializa el singleton y evita duplicados.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantiene el objeto entre escenas
        }
        else
        {
            Destroy(gameObject); // Evita duplicados
        }
    }

    /// <summary>
    /// Inicializa la puntuación al iniciar la escena.
    /// </summary>
    void Start()
    {
        puntos = 0;
    }

    /// <summary>
    /// Reinicia la puntuación al comenzar una nueva partida.
    /// </summary>
    public void NewGame()
    {
        puntos = 0;
    }

    /// <summary>
    /// Suma la cantidad indicada de puntos a la puntuación actual.
    /// </summary>
    /// <param name="cantidad">Cantidad de puntos a sumar</param>
    public void SumarPuntos(int cantidad)
    {
        puntos += cantidad;
    }
}
