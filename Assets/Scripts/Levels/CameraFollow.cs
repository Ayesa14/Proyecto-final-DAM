using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float followAhead = 2.5f;
    public float minPosX;
    public float maxPosX;

    public Transform limitLeft;
    public Transform limitRight;

    public Transform collLeft;
    public Transform collRight;

    float camWidth;
    float lastPos;
    void Start()
    {
        camWidth = Camera.main.orthographicSize * Camera.main.aspect;
        minPosX = limitLeft.position.x + camWidth;
        maxPosX = limitRight.position.x - camWidth;
        lastPos = minPosX;

        collLeft.position = new Vector2(transform.position.x - camWidth - 0.5f, collLeft.position.y);
        collRight.position = new Vector2(transform.position.x + camWidth + 0.5f, collRight.position.y);
    }

    void Update()
    {
        if (target != null)
        {
            // transform.position = new Vector3(target.position.x + followAhead, transform.position.y, transform.position.z);
            float newPosX = target.position.x + followAhead;
            newPosX = Mathf.Clamp(newPosX, lastPos, maxPosX);
            transform.position = new Vector3(newPosX, transform.position.y, transform.position.z);
            lastPos = newPosX;
        }
    }

    public float PositionInCamera(float pos, float width, out bool limitRight, out bool limitLeft)
    {
        if (pos + width > maxPosX + camWidth)
        {
            limitLeft = false;
            limitRight = true;
            return (maxPosX + camWidth - width);
        }
        else if (pos - width < lastPos - camWidth)
        {
            limitLeft = true;
            limitRight = false;
            return (lastPos - camWidth + width);
        }
        limitLeft = false;
        limitRight = false;
        return pos;
    }
    public void StartFollow(Transform t)
    {
        target = t;
        float newPosX = target.position.x + followAhead;
        newPosX = Mathf.Clamp(newPosX, lastPos, maxPosX);
        transform.position = new Vector3(newPosX, transform.position.y, transform.position.z);
        lastPos = newPosX;
    }
}
