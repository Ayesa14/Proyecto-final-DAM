using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public int time;
    public float timer;

    Mario mario;

    public bool levelFinished;

    public Transform startPoint;
    public Transform checkPoint;
    public CameraFollow cameraFollow;
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
        timer = time;
        GameManager.Instance.hud.UpdateTime(timer);
        mario = FindAnyObjectByType<Mario>();
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
        GameManager.Instance.LevelLoaded();
    }

    void Update()
    {
        if (!levelFinished)
        {
            timer -= Time.deltaTime / 0.4f; //1 segundo del juego = a 0.4segundos reales
            if (timer <= 0)
            {
                GameManager.Instance.OutOfTime();
                timer = 0;
            }
            GameManager.Instance.hud.UpdateTime(timer);
        }
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
            GameManager.Instance.hud.UpdateTime(timer);
            ScoreManager.Instance.SumarPuntos(50);
            AudioManager.Instance.PlayCoin();
            yield return new WaitForSeconds(0.05f);
        }
    }
}
