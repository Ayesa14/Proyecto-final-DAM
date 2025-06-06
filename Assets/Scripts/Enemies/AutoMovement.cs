using UnityEngine;

public class AutoMovement : MonoBehaviour
{
    public float speed = 1f;
    bool movementPaused;
    Rigidbody2D rb2D;
    SpriteRenderer spriteRenderer;
    Vector2 lastVelocity;
    Vector2 currentDirection;
    float defaultSpeed;
    public bool flipSprite = true;
    float timer = 0;
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        rb2D.linearVelocity = new Vector2(speed, rb2D.linearVelocity.y);
        defaultSpeed = Mathf.Abs(speed);

    }
    private void FixedUpdate()
    {
        if (!movementPaused)
        {
            if (rb2D.linearVelocity.x > -0.1f && rb2D.linearVelocity.x < 0.1f)
            {
                if (timer > 0.05f)
                {
                    speed = -speed;
                }
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0;
            }
            rb2D.linearVelocity = new Vector2(speed, rb2D.linearVelocity.y);
            if (!flipSprite)
            {
                if (rb2D.linearVelocity.x > 0)
                {
                    spriteRenderer.flipX = true;
                }
                else
                {
                    spriteRenderer.flipX = false;
                }
            }

        }
    }
    public void PauseMovement()
    {
        if (!movementPaused)
        {
            currentDirection = rb2D.linearVelocity.normalized;
            lastVelocity = rb2D.linearVelocity;
            movementPaused = true;
            rb2D.linearVelocity = new Vector2(0, 0);
        }
    }
    public void ContinueMovement()
    {
        if (movementPaused)
        {
            speed = defaultSpeed * currentDirection.x;
            rb2D.linearVelocity = new Vector2(speed, lastVelocity.y);
            movementPaused = false;
        }
    }
    public void ContinueMovement(Vector2 newVelocity)
    {
        if (movementPaused)
        {
            rb2D.linearVelocity = newVelocity;
            movementPaused = false;
        }
    }
    public void ChangeDirection()
    {
        speed = -speed;
        rb2D.linearVelocity = new Vector2(speed, rb2D.linearVelocity.y);
    }
}
