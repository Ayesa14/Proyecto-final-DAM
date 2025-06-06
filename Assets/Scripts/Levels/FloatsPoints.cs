using UnityEngine;

public class FloatsPoints : MonoBehaviour
{
    public int numPoints = 0;
    public float distance = 3f;
    public float speed = 2.5f;
    public bool destroy = true;

    float targetPos;
    public Points[] points;
    SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        Show(numPoints);
        targetPos = transform.position.y + distance;
    }
    void Update()
    {
        if (transform.position.y < targetPos)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + (speed * Time.deltaTime));
        }
        else if (destroy)
        {
            Destroy(gameObject);
        }
    }
    void Show(int points)
    {
        for (int i = 0; i < this.points.Length; i++)
        {
            if(this.points[i].numPoints == points){
                spriteRenderer.sprite = this.points[i].sprite;
                break;
            }
        }
    }
}
[System.Serializable]
public class Points
{
    public int numPoints;
    public Sprite sprite;        
}
