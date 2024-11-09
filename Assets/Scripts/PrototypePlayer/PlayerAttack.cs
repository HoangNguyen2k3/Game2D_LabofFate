using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] public Animator anim;
    [SerializeField] float damage;

    public static PlayerAttack instance;

    public bool isAttacking = false;
    public bool canCombo = false;
    public bool shaking = false;


    [SerializeField] GameObject anchor;
    // Update is called once per frame


    void Awake() {
        instance = this;
    }

    void Update()
    {
        if (shaking)
        {
            StartCoroutine(CameraShake.instance.Shake(0.1f, 0.1f));
        }
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            float angle = GetMouseAngle();
            if (angle > 90 || angle < -90)
            {
                transform.localScale = new Vector3(1,-1,1);
            }
            else
            {
                transform.localScale = Vector3.one;
            }
            anchor.transform.rotation = Quaternion.Euler(0, 0, angle);
            isAttacking = true;
        }
    }

    float GetMouseAngle()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);
        return Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
    }
}
