using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public HUD hud;
    int coins;

    public int time;
    public float timer;

    Mario mario;

    public bool levelFinished;
    public static LevelManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        coins = 0;
        hud.UpdateCoins(coins);
        timer = time;
        hud.UpdateTime(timer);
        mario = FindAnyObjectByType<Mario>();
    }

    void Update()
    {
        if (!levelFinished)
        {
            timer -= Time.deltaTime / 0.4f; //1 segundo del juego = a 0.4segundos reales
            if (timer <= 0)
            {
                mario.Dead();
                timer = 0;
            }
            hud.UpdateTime(timer);
        }
    }
    public void AddCoins()
    {
        coins++;
        hud.UpdateCoins(coins);
    }
    public void LevelFinished()
    {
        levelFinished = true;
        StartCoroutine(SecondsToPoints());
    }
    IEnumerator SecondsToPoints()
    {
        yield return new WaitForSeconds(1f);

        int timeLeft = Mathf.RoundToInt(timer);
        while (timeLeft > 0)
        {
            timeLeft--;
            hud.UpdateTime(timeLeft);
            ScoreManager.Instance.SumarPuntos(50);
            AudioManager.Instance.PlayCoin();
            yield return new WaitForSeconds(0.05f);
        }
    }
}
