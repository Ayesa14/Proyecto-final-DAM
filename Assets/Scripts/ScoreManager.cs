using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int puntos;
    public static ScoreManager Instance;
    private void Awake()
    {
        if(Instance == null){
            Instance = this;
        }
    }
    void Start()
    {
        puntos = 0;
    }
    public void SumarPuntos(int cantidad)
    {
        puntos += cantidad;
    }
}
