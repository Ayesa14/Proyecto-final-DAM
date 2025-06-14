using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int puntos;
    public static ScoreManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        puntos = 0;
    }
    public void NewGame()
    {
        puntos = 0;
    }
    public void SumarPuntos(int cantidad)
    {
        puntos += cantidad;
    }
}
