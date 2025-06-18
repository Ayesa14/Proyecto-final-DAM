using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Script que gestiona el comportamiento de los bloques del juego (rompibles, con monedas, con ítems, etc).
/// </summary>
public class Block : MonoBehaviour
{
    public bool isBreakable; // Indica si el bloque se puede romper
    public GameObject brickPiecePrefab; // Prefab de los trozos de ladrillo al romperse
    public int numCoins; // Número de monedas que contiene el bloque
    public GameObject coinBlockPrefab; // Prefab de la moneda que aparece al golpear el bloque
    bool bouncing; // Indica si el bloque está en animación de rebote
    public Sprite emptyBlock; // Sprite del bloque vacío
    bool isEmpty; // Indica si el bloque ya está vacío
    public GameObject itemPrefab; // Prefab del ítem que puede aparecer

    //public GameObject floatPointsPrefab;
    public LayerMask onBlockLayers; // Capas de objetos que pueden estar sobre el bloque
    BoxCollider2D boxCollider2D; // Referencia al BoxCollider2D

    /// <summary>
    /// Inicializa la referencia al BoxCollider2D.
    /// </summary>
    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    /// <summary>
    /// Llama a los métodos de reacción de los objetos que están sobre el bloque.
    /// </summary>
    void OnTheBlock()
    {
        // Busca todos los colliders sobre el bloque
        Collider2D[] colliders = Physics2D.OverlapBoxAll(
            boxCollider2D.bounds.center + Vector3.up * boxCollider2D.bounds.extents.y,
            boxCollider2D.bounds.size * 0.5f,
            0,
            onBlockLayers
        );
        foreach (Collider2D c in colliders)
        {
            Enemy enemy = c.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.HitBelowBlock(); // Si es enemigo, reacciona al golpe desde abajo
            }
            else
            {
                Item item = c.GetComponent<Item>();
                if (item != null)
                {
                    item.HitBelowBlock(); // Si es ítem, reacciona al golpe desde abajo
                }
            }
        }
    }

    /// <summary>
    /// Lógica al golpear el bloque con la cabeza de Mario.
    /// </summary>
    /// <param name="marioBig">Indica si Mario es grande.</param>
    public void HeadCollision(bool marioBig)
    {
        if (isBreakable)
        {
            if (marioBig)
            {
                Break(); // Si es rompible y Mario es grande, se rompe
            }
            else
            {
                Bounce(); // Si es pequeño, solo rebota
            }
        }
        else if (!isEmpty)
        {
            if (numCoins > 0)
            {
                if (!bouncing)
                {
                    Instantiate(coinBlockPrefab, transform.position, Quaternion.identity); // Aparece una moneda
                    numCoins--;
                    // AudioManager.Instance.PlayCoin();
                    // ScoreManager.Instance.SumarPuntos(200);
                    // GameObject newFloatPoints = Instantiate(floatPointsPrefab, transform.position, Quaternion.identity);
                    // FloatsPoints floatsPoints = newFloatPoints.GetComponent<FloatsPoints>();
                    // floatsPoints.numPoints = 200;
                    Bounce(); // Rebota el bloque
                    if (numCoins <= 0)
                    {
                        isEmpty = true; // Si ya no quedan monedas, el bloque queda vacío
                    }
                }
            }
            else if (itemPrefab != null)
            {
                if (!bouncing)
                {
                    StartCoroutine(ShowItem()); // Muestra el ítem
                    Bounce(); // Rebota el bloque
                    isEmpty = true; // El bloque queda vacío
                }
            }
        }
        if (!isEmpty)
        {
            OnTheBlock(); // Notifica a los objetos sobre el bloque
        }
    }

    /// <summary>
    /// Inicia la animación de rebote del bloque.
    /// </summary>
    void Bounce()
    {
        if(!bouncing){
            StartCoroutine(BounceAnimation());
        }
    }

    /// <summary>
    /// Corrutina que realiza la animación de rebote del bloque.
    /// </summary>
    IEnumerator BounceAnimation()
    {
        AudioManager.Instance.PlayBump(); // Sonido de golpe
        bouncing = true;
        float time = 0;
        float duration = 0.1f;

        Vector2 startPosition = transform.position;
        Vector2 targetPosition = (Vector2)transform.position + Vector2.up * 0.25f;

        // Sube el bloque
        while(time < duration){
            transform.position = Vector2.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        time = 0;
        // Baja el bloque
        while(time < duration){
            transform.position = Vector2.Lerp(targetPosition, startPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = startPosition;
        bouncing = false;
        // Si el bloque está vacío, cambia el sprite
        if(isEmpty){
            SpritesAnimation spritesAnimation = GetComponent<SpritesAnimation>();
            if(spritesAnimation != null){
                spritesAnimation.stop = true;
            }
            GetComponent<SpriteRenderer>().sprite = emptyBlock;
        }
    }

    /// <summary>
    /// Rompe el bloque en pedazos y lo destruye.
    /// </summary>
    void Break()
    {
        AudioManager.Instance.PlayBreak(); // Sonido de romper
        ScoreManager.Instance.SumarPuntos(50); // Suma puntos
        GameObject brickPiece;
        // Instancia los 4 trozos de ladrillo con diferentes direcciones
        // Arriba a la derecha
        brickPiece = Instantiate(brickPiecePrefab, transform.position, Quaternion.Euler(new Vector3(0,0,0)));
        brickPiece.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(6f,12f);

        // Arriba a la izquierda
        brickPiece = Instantiate(brickPiecePrefab, transform.position, Quaternion.Euler(new Vector3(0,0,90)));
        brickPiece.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(-6f,12f);

        // Abajo a la derecha
        brickPiece = Instantiate(brickPiecePrefab, transform.position, Quaternion.Euler(new Vector3(0,0,-90)));
        brickPiece.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(6f,-8f);

        // Abajo a la izquierda
        brickPiece = Instantiate(brickPiecePrefab, transform.position, Quaternion.Euler(new Vector3(0,0,180)));
        brickPiece.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(-6f,-8f);

        Destroy(gameObject); // Destruye el bloque original
    }

    /// <summary>
    /// Corrutina que muestra el ítem saliendo del bloque.
    /// </summary>
    IEnumerator ShowItem()
    {
        AudioManager.Instance.PlayPowerUpAppear(); // Sonido de aparición de ítem
        GameObject newItem = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        
        Item item = newItem.GetComponent<Item>();
        item.WaitMove(); // Espera antes de moverse
        float time = 0;
        float duration = 0;
        Vector2 startPosition = newItem.transform.position;
        Vector2 targetPosition = (Vector2)transform.position + Vector2.up * 0.5f;

        // Sube el ítem
        while (time < duration)
        {
            newItem.transform.position = Vector2.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        newItem.transform.position = targetPosition;
        
        item.StartMove(); // Comienza a moverse
    }
}



