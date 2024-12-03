using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3Arm : MonoBehaviour
{

    public float angularSpeed = 1f;
    public float circleRad = 1f;

    private Vector2 fixedPoint;
    private float currentAngle;

    void Start ()
    {
        fixedPoint = transform.position;
    }

    void Update ()
    {
        currentAngle += angularSpeed * Time.deltaTime;
        Vector2 offset = new Vector2 (Mathf.Sin(currentAngle), Mathf.Cos(2 * currentAngle)) * circleRad;
        transform.position = fixedPoint + offset;
    }
}
