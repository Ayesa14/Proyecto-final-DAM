using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ShowAndHide : MonoBehaviour
{
    public GameObject objectToMove;
    public Transform showPoint;
    public Transform hidePoint;
    public float waitForShow;
    public float waitForHide;
    public float speedShow;
    public float speedHide;
    float timerShow;
    float timerHide;
    float speed;
    Vector2 targetPoint;
    void Start()
    {
        targetPoint = hidePoint.position;
        speed = speedHide;
        timerHide =0;
        timerShow = 0;
    }

    void Update()
    {
        objectToMove.transform.position = Vector2.MoveTowards(objectToMove.transform.position,targetPoint, speed * Time.deltaTime);
        if(Vector2.Distance(objectToMove.transform.position, hidePoint.position) < 0.1f){
            //El objeto esta en el punto de escondido(hide)
            timerShow+= Time.deltaTime;
            if(timerShow >= waitForShow && !Locked()){
                targetPoint = showPoint.position;
                speed = speedShow;
                timerShow = 0;
            }
        } else if(Vector2.Distance(objectToMove.transform.position, showPoint.position) < 0.1f){
            //El objeto está en el punto de mostrado(show)
            timerHide += Time.deltaTime;
            if(timerHide >= waitForHide){
                targetPoint = hidePoint.position;
                speed = speedHide;
                timerHide = 0;
            }
        }

    }
    bool Locked(){
        return Physics2D.OverlapBox(transform.position + Vector3.up, Vector2.one, 0);
    }
}
