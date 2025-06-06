using UnityEngine;

public class GoalPole : MonoBehaviour
{
    public Transform flag;
    public Transform bottom;
    public float flagVelocity = 5f;
    public GameObject floatPointsPrefab;
    bool downFlag;
    Mover mover;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Mario mario = collision.GetComponent<Mario>();
        if (mario != null)
        {
            downFlag = true;
            mario.Goal();
            mover = collision.GetComponent<Mover>();
            Vector2 contactPoint = collision.ClosestPoint(transform.position);
            CalculateHeight(contactPoint.y);
        }
    }
    private void FixedUpdate()
    {
        if (downFlag)
        {
            if (flag.position.y > bottom.position.y)
            {
                flag.position = new Vector2(flag.position.x, flag.position.y - (flagVelocity * Time.fixedDeltaTime));
            }
            else
            {
                mover.isFlagDown = true;
            }
        }
    }
    void CalculateHeight(float marioPosition)
    {
        float size = GetComponent<BoxCollider2D>().bounds.size.y;

        float minPosition1 = transform.position.y + (size - size / 5f); //5000

        float minPosition2 = transform.position.y + (size - 2 * size / 5f); //2000

        float minPosition3 = transform.position.y + (size - 3 * size / 5f); //800

        float minPosition4 = transform.position.y + (size - 4 * size / 5f); //400

        int numPoints = 0;
        if (marioPosition >= minPosition1)
        {
            numPoints = 5000;
        }
        else if (marioPosition >= minPosition2)
        {
            numPoints = 2000;
        }
        else if (marioPosition >= minPosition3)
        {
            numPoints = 800;
        }
        else if (marioPosition >= minPosition4)
        {
            numPoints = 400;
        }
        else
        {
            numPoints = 100;
        }
        ScoreManager.Instance.SumarPuntos(numPoints);

        Vector2 positionFloatPoints = new Vector2(transform.position.x + 0.65f, bottom.position.y);
        GameObject newFloatPoints = Instantiate(floatPointsPrefab, positionFloatPoints, Quaternion.identity);
        FloatsPoints floatsPoints = newFloatPoints.GetComponent<FloatsPoints>();
        floatsPoints.numPoints = numPoints;
        floatsPoints.speed = flagVelocity;
        floatsPoints.distance = flag.position.y - bottom.position.y;
        floatsPoints.destroy = false;
    }
}
