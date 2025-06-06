using System.Collections;
using UnityEngine;

public class SpritesAnimation : MonoBehaviour
{
    public Sprite[] sprites;
    public float frameTime = 0.1f;
    // float timer = 0f;
    int animationFrame = 0;
    public bool stop;
    public bool loop = true;
    SpriteRenderer spriteRenderer;
    private void Awake()
    {
       spriteRenderer = GetComponent<SpriteRenderer>(); 
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Animation());
    }

    // Update is called once per frame
    // void Update()
    // {
    //     timer += Time.deltaTime;
    //     if(timer >= frameTime){
    //         animationFrame++;
    //         if(animationFrame >= sprites.Length){
    //             animationFrame = 0;
    //         }
    //         spriteRenderer.sprite = sprites[animationFrame];
    //         timer = 0;
    //     }
    // }
    IEnumerator Animation()
    {
        if (loop)
        {
            while (!stop)
            {
                spriteRenderer.sprite = sprites[animationFrame];
                animationFrame++;
                if (animationFrame >= sprites.Length)
                {
                    animationFrame = 0;
                }
                yield return new WaitForSeconds(frameTime);
            }
        }
        else
        {
            while (animationFrame < sprites.Length)
            {
                spriteRenderer.sprite = sprites[animationFrame];
                animationFrame++;
                yield return new WaitForSeconds(frameTime);
            }
            Destroy(gameObject);
        }       
    }
}
