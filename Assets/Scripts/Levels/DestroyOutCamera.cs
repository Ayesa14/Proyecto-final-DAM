using UnityEngine;

public class DestroyOutCamera : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    bool hasBeenVisible;

    public bool onlyBack;
    public float minDistance = 0;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(spriteRenderer.isVisible){
            hasBeenVisible = true;
        }
        else {
            if(hasBeenVisible){
                if(Mathf.Abs(transform.position.x - Camera.main.transform.position.x) > minDistance){
                    if(onlyBack){
                        if(transform.position.x > Camera.main.transform.position.x){
                            return;
                        }
                    }
                    Destroy(gameObject);
                }                
            }
        }
    }
}
