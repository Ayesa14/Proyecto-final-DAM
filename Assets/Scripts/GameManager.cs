using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script principal que gestiona el estado global del juego.
/// Controla vidas, monedas, respawn, checkpoints, game over y comunicación con el HUD.
/// Implementa el patrón Singleton para acceso global.
/// </summary>
public class GameManager : MonoBehaviour
{
    public HUD hud;                  // Referencia al HUD para actualizar la UI
    int coins;                       // Contador de monedas actuales
    public Mario mario;              // Referencia a Mario

    public int lives;                // Número de vidas actuales

    bool isRespawning;               // Indica si se está en proceso de respawn
    bool isGameOver;                 // Indica si el juego está en estado de game over
    public bool isLevelCheckPoint;   // Indica si se alcanzó un checkpoint en el nivel

    public static GameManager Instance; // Singleton para acceso global

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
    /// Inicializa vidas, monedas y actualiza el HUD al iniciar el juego.
    /// </summary>
    void Start()
    {
        lives = 3;
        coins = 97;
        hud.UpdateCoins(coins);
    }

    /// <summary>
    /// Suma una moneda y actualiza el HUD. Si llega a 100, suma una vida extra.
    /// </summary>
    public void AddCoins()
    {
        coins++;
        if (coins > 99)
        {
            coins = 0;
            NewLife(); // Suma una vida extra
        }
        hud.UpdateCoins(coins);
    }

    /// <summary>
    /// Llama a la lógica de muerte de Mario si se acaba el tiempo.
    /// </summary>
    public void OutOfTime()
    {
        mario.Dead();
    }

    /// <summary>
    /// Lógica cuando Mario cae en la zona de muerte (KillZone).
    /// </summary>
    public void KillZone()
    {
        if (!isRespawning)
        {
            AudioManager.Instance.PlayDie(); // Sonido de muerte
            LoseLife();
        }
    }

    /// <summary>
    /// Resta una vida y gestiona el respawn o game over.
    /// </summary>
    public void LoseLife()
    {
        if (!isRespawning)
        {
            lives--;
            isRespawning = true;
            if (lives > 0)
            {
                StartCoroutine(Respawn()); // Si quedan vidas, respawnea
            }
            else
            {
                GameOver(); // Si no, game over
            }
        }
    }

    /// <summary>
    /// Suma una vida y reproduce el sonido correspondiente.
    /// </summary>
    public void NewLife()
    {
        lives++;
        AudioManager.Instance.PlayOneUp();
    }

    /// <summary>
    /// Inicializa los valores para una nueva partida.
    /// </summary>
    void NewGame()
    {
        lives = 3;
        coins = 0;
        isGameOver = false;
        ScoreManager.Instance.NewGame();
        isLevelCheckPoint = false;
    }

    /// <summary>
    /// Lógica de game over y respawn.
    /// </summary>
    void GameOver()
    {
        isGameOver = true;
        StartCoroutine(Respawn());
    }

    /// <summary>
    /// Corrutina que espera antes de recargar la escena y respawnear.
    /// </summary>
    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1f);
        isRespawning = false;
        SceneManager.LoadScene(0); // Recarga la escena principal
    }

    /// <summary>
    /// Lógica que se ejecuta cuando el nivel termina de cargar.
    /// Coloca a Mario en el checkpoint o inicio y ajusta la cámara.
    /// </summary>
    public void LevelLoaded()
    {
        if (isGameOver)
        {
            NewGame();
        }
        if (isLevelCheckPoint)
        {
            Mario.Instance.Respawn(LevelManager.Instance.checkPoint.position);
        }
        else
        {
            Mario.Instance.Respawn(LevelManager.Instance.startPoint.position);
        }
        LevelManager.Instance.cameraFollow.StartFollow(Mario.Instance.transform);
    }
}
