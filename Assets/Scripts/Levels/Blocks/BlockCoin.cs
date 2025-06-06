using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockCoin : MonoBehaviour
{
    public GameObject floatPointsPrefab;
    void Start()
    {
        AudioManager.Instance.PlayCoin();
        ScoreManager.Instance.SumarPuntos(200);

        Vector2 positionFloatPoints = new Vector2(transform.position.x, transform.position.y + 1f);

        GameObject newFloatPoints = Instantiate(floatPointsPrefab, positionFloatPoints, Quaternion.identity);
        FloatsPoints floatsPoints = newFloatPoints.GetComponent<FloatsPoints>();
        floatsPoints.numPoints = 200;
        StartCoroutine(Animation());
    }

    IEnumerator Animation(){
        float time = 0;
        float duration = 0.25f;
        Vector2 startPosition = transform.position;
        Vector2 targetPosition = (Vector2)transform.position + (Vector2.up * 3f);

        while(time < duration){
            transform.position = Vector2.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        time = 0;
        while(time < duration){
            transform.position = Vector2.Lerp(targetPosition, startPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = startPosition;
        Destroy(gameObject);
    }
}
