using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockCoin : MonoBehaviour
{
    public GameObject floatPointsPrefab; // Prefab de los puntos flotantes

    /// <summary>
    /// Al iniciar, suma moneda, puntos, reproduce sonido y muestra los puntos flotantes.
    /// </summary>
    void Start()
    {
        GameManager.Instance.AddCoins(); // Suma una moneda al contador global
        AudioManager.Instance.PlayCoin(); // Reproduce el sonido de moneda
        ScoreManager.Instance.SumarPuntos(200); // Suma 200 puntos al marcador

        // Calcula la posición donde aparecerán los puntos flotantes (encima de la moneda)
        Vector2 positionFloatPoints = new Vector2(transform.position.x, transform.position.y + 1f);

        // Instancia los puntos flotantes y les asigna el valor
        GameObject newFloatPoints = Instantiate(floatPointsPrefab, positionFloatPoints, Quaternion.identity);
        FloatsPoints floatsPoints = newFloatPoints.GetComponent<FloatsPoints>();
        floatsPoints.numPoints = 200;

        // Inicia la animación de la moneda
        StartCoroutine(Animation());
    }

    /// <summary>
    /// Corrutina que anima la moneda subiendo y bajando, y luego la destruye.
    /// </summary>
    IEnumerator Animation(){
        float time = 0;
        float duration = 0.25f;
        Vector2 startPosition = transform.position;
        Vector2 targetPosition = (Vector2)transform.position + (Vector2.up * 3f);

        // Sube la moneda
        while(time < duration){
            transform.position = Vector2.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        time = 0;
        // Baja la moneda
        while(time < duration){
            transform.position = Vector2.Lerp(targetPosition, startPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = startPosition;
        Destroy(gameObject); // Destruye la moneda al finalizar la animación
    }
}
