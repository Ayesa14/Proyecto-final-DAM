using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Block : MonoBehaviour
{
    public bool isBreakable;
    public GameObject brickPiecePrefab;
    public int numCoins;
    public GameObject coinBlockPrefab;
    bool bouncing;
    public Sprite emptyBlock;
    bool isEmpty;
    public GameObject itemPrefab;

    //public GameObject floatPointsPrefab;
    public LayerMask onBlockLayers;
    BoxCollider2D boxCollider2D;
    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }
    void OnTheBlock()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(boxCollider2D.bounds.center + Vector3.up * boxCollider2D.bounds.extents.y, boxCollider2D.bounds.size * 0.5f, 0, onBlockLayers);
        foreach (Collider2D c in colliders)
        {
            Enemy enemy = c.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.HitBelowBlock();
            }
            else
            {
                Item item = c.GetComponent<Item>();
                if (item != null)
                {
                    item.HitBelowBlock();
                }
            }
        }
    }
    public void HeadCollision(bool marioBig)
    {
        if (isBreakable)
        {
            if (marioBig)
            {
                Break();
            }
            else
            {
                Bounce();
            }
        }
        else if (!isEmpty)
        {
            if (numCoins > 0)
            {
                if (!bouncing)
                {
                    Instantiate(coinBlockPrefab, transform.position, Quaternion.identity);
                    numCoins--;
                    // AudioManager.Instance.PlayCoin();
                    // ScoreManager.Instance.SumarPuntos(200);
                    // GameObject newFloatPoints = Instantiate(floatPointsPrefab, transform.position, Quaternion.identity);
                    // FloatsPoints floatsPoints = newFloatPoints.GetComponent<FloatsPoints>();
                    // floatsPoints.numPoints = 200;
                    Bounce();
                    if (numCoins <= 0)
                    {
                        isEmpty = true;
                    }
                }
            }
            else if (itemPrefab != null)
            {
                if (!bouncing)
                {
                    StartCoroutine(ShowItem());
                    Bounce();
                    isEmpty = true;
                }
            }
        }
        if (!isEmpty)
        {
            OnTheBlock();
        }
    }
    void Bounce(){
        if(!bouncing){
            StartCoroutine(BounceAnimation());
        }
    }
    IEnumerator BounceAnimation(){
        AudioManager.Instance.PlayBump();
        bouncing = true;
        float time = 0;
        float duration = 0.1f;

        Vector2 startPosition = transform.position;
        Vector2 targetPosition = (Vector2)transform.position + Vector2.up * 0.25f;

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
        bouncing = false;
        if(isEmpty){
            SpritesAnimation spritesAnimation = GetComponent<SpritesAnimation>();
            if(spritesAnimation != null){
                spritesAnimation.stop = true;
            }
            GetComponent<SpriteRenderer>().sprite = emptyBlock;
        }
    }

    void Break(){
        AudioManager.Instance.PlayBreak();
        ScoreManager.Instance.SumarPuntos(50);
        GameObject brickPiece;
        //arriba a la derecha
        brickPiece = Instantiate(brickPiecePrefab, transform.position, Quaternion.Euler(new Vector3(0,0,0)));
        brickPiece.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(6f,12f);

        //arriba a la izquierda
        brickPiece = Instantiate(brickPiecePrefab, transform.position, Quaternion.Euler(new Vector3(0,0,90)));
        brickPiece.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(-6f,12f);

        //abajo a la derecha
        brickPiece = Instantiate(brickPiecePrefab, transform.position, Quaternion.Euler(new Vector3(0,0,-90)));
        brickPiece.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(6f,-8f);

        //abajo a la izquierda
        brickPiece = Instantiate(brickPiecePrefab, transform.position, Quaternion.Euler(new Vector3(0,0,180)));
        brickPiece.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(-6f,-8f);

        Destroy(gameObject);        
    }

    IEnumerator ShowItem()
    {
        AudioManager.Instance.PlayPowerUpAppear();
        GameObject newItem = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        
        Item item = newItem.GetComponent<Item>();
        item.WaitMove();
        float time = 0;
        float duration = 0;
        Vector2 startPosition = newItem.transform.position;
        Vector2 targetPosition = (Vector2)transform.position + Vector2.up * 0.5f;

        while (time < duration)
        {
            newItem.transform.position = Vector2.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        newItem.transform.position = targetPosition;
        
        item.StartMove();
    }
}

    

