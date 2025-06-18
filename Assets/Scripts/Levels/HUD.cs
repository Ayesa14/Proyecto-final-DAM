using UnityEngine;
using TMPro;

/// <summary>
/// Script que gestiona el HUD del juego (puntuación, monedas y tiempo).
/// </summary>
public class HUD : MonoBehaviour
{
    public TextMeshProUGUI score;    // Referencia al texto de la puntuación
    public TextMeshProUGUI numCoins; // Referencia al texto de las monedas
    public TextMeshProUGUI time;     // Referencia al texto del tiempo

    /// <summary>
    /// Actualiza la puntuación cada frame.
    /// </summary>
    void Update()
    {
        score.text = ScoreManager.Instance.puntos.ToString("D6"); // Muestra la puntuación con 6 dígitos
    }

    /// <summary>
    /// Actualiza el texto de las monedas.
    /// </summary>
    /// <param name="totalCoins">Cantidad total de monedas</param>
    public void UpdateCoins(int totalCoins)
    {
        numCoins.text = "x" + totalCoins.ToString("D2"); // Muestra las monedas con 2 dígitos
    }

    /// <summary>
    /// Actualiza el texto del tiempo restante.
    /// </summary>
    /// <param name="timeLeft">Tiempo restante</param>
    public void UpdateTime(float timeLeft)
    {
        int timeLeftInt = Mathf.RoundToInt(timeLeft); // Redondea el tiempo a entero
        time.text = timeLeftInt.ToString("D3");       // Muestra el tiempo con 3 dígitos
    }
}
