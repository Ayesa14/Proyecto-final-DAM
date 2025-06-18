using UnityEngine;

/// <summary>
/// Script que gestiona todos los sonidos del juego.
/// Permite reproducir efectos de sonido específicos mediante métodos públicos.
/// Implementa el patrón Singleton para acceso global.
/// </summary>
public class AudioManager : MonoBehaviour
{
    // Clips de audio para cada efecto de sonido
    public AudioClip clipJump;           // Sonido de salto pequeño
    public AudioClip clipBigJump;        // Sonido de salto grande
    public AudioClip clipCoin;           // Sonido de recoger moneda
    public AudioClip clipStomp;          // Sonido de pisar enemigo
    public AudioClip clipFlipDie;        // Sonido de muerte volteada de enemigo
    public AudioClip clipShoot;          // Sonido de disparar bola de fuego
    public AudioClip clipPowerUp;        // Sonido de obtener power-up
    public AudioClip clipPowerDown;      // Sonido de perder power-up
    public AudioClip clipPowerUpAppear;  // Sonido de aparición de power-up
    public AudioClip clipBreak;          // Sonido de romper bloque
    public AudioClip clipBump;           // Sonido de golpear bloque o explosión
    public AudioClip clipDie;            // Sonido de muerte de Mario
    public AudioClip clipFlagPole;       // Sonido de llegar a la meta
    public AudioClip clipOneUp;          // Sonido de vida extra

    AudioSource audioSource;             // Referencia al componente AudioSource
    public static AudioManager Instance; // Singleton para acceso global

    /// <summary>
    /// Inicializa el singleton y la referencia al AudioSource.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject); // Mantiene el objeto entre escenas
        }
        else
        {
            Destroy(gameObject); // Evita duplicados
        }
    }

    /// <summary>
    /// Reproduce el sonido de salto pequeño.
    /// </summary>
    public void PlayJump()
    {
        audioSource.PlayOneShot(clipJump);
    }

    /// <summary>
    /// Reproduce el sonido de salto grande.
    /// </summary>
    public void PlayBigJump()
    {
        audioSource.PlayOneShot(clipBigJump);
    }

    /// <summary>
    /// Reproduce el sonido de recoger moneda.
    /// </summary>
    public void PlayCoin()
    {
        audioSource.PlayOneShot(clipCoin);
    }

    /// <summary>
    /// Reproduce el sonido de pisar enemigo.
    /// </summary>
    public void PlayStomp()
    {
        audioSource.PlayOneShot(clipStomp);
    }

    /// <summary>
    /// Reproduce el sonido de muerte volteada de enemigo.
    /// </summary>
    public void PlayFlipDie()
    {
        audioSource.PlayOneShot(clipFlipDie);
    }

    /// <summary>
    /// Reproduce el sonido de disparar bola de fuego.
    /// </summary>
    public void PlayShoot()
    {
        audioSource.PlayOneShot(clipShoot);
    }

    /// <summary>
    /// Reproduce el sonido de obtener power-up.
    /// </summary>
    public void PlayPowerUp()
    {
        audioSource.PlayOneShot(clipPowerUp);
    }

    /// <summary>
    /// Reproduce el sonido de perder power-up.
    /// </summary>
    public void PlayPowerDown()
    {
        audioSource.PlayOneShot(clipPowerDown);
    }

    /// <summary>
    /// Reproduce el sonido de aparición de power-up.
    /// </summary>
    public void PlayPowerUpAppear()
    {
        audioSource.PlayOneShot(clipPowerUpAppear);
    }

    /// <summary>
    /// Reproduce el sonido de romper bloque.
    /// </summary>
    public void PlayBreak()
    {
        audioSource.PlayOneShot(clipBreak);
    }

    /// <summary>
    /// Reproduce el sonido de golpear bloque o explosión.
    /// </summary>
    public void PlayBump()
    {
        audioSource.PlayOneShot(clipBump);
    }

    /// <summary>
    /// Reproduce el sonido de muerte de Mario.
    /// </summary>
    public void PlayDie()
    {
        audioSource.PlayOneShot(clipDie);
    }

    /// <summary>
    /// Reproduce el sonido de llegar a la meta.
    /// </summary>
    public void PlayFlagPole()
    {
        audioSource.PlayOneShot(clipFlagPole);
    }

    /// <summary>
    /// Reproduce el sonido de vida extra (1UP).
    /// </summary>
    public void PlayOneUp()
    {
        audioSource.PlayOneShot(clipOneUp);
    }
}
