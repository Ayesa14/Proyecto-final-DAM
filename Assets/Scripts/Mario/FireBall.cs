using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float direction;
    public float speed;
    public float bounceForce;
    public GameObject explosionPrefab;
    Rigidbody2D rb2D;
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        speed *= direction;
        rb2D.linearVelocity = new Vector2(speed, 0);
    }

    void Update()
    {
        transform.Rotate(0, 0, speed * Time.deltaTime * -45);
        rb2D.linearVelocity = new Vector2(speed, rb2D.linearVelocity.y);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.HitFireball();
            //Destroy(gameObject);
            Explode(collision.GetContact(0).point);
        }
        else
        {
            Vector2 sidePoint = collision.GetContact(0).normal;
            //Debug.Log("Side point: " + sidePoint);

            if (sidePoint.x != 0) //Hya colision lateral
            {
                //Destroy(gameObject);
                Explode(collision.GetContact(0).point);

            }
            else if (sidePoint.y > 0) //colision por abajo
            {
                rb2D.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
            }
            else if (sidePoint.y < 0) //colision por arriba
            {
                rb2D.AddForce(Vector2.down * bounceForce, ForceMode2D.Impulse);
            }
            else
            {
                //Destroy(gameObject);
                Explode(collision.GetContact(0).point);

            }
        }
    }
    void Explode(Vector2 point)
    {
        AudioManager.Instance.PlayBump();
        Instantiate(explosionPrefab, point, Quaternion.identity);
        Destroy(gameObject);
    }
}
