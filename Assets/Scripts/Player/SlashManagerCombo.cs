using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashManagerCombo : MonoBehaviour
{
    public static SlashManagerCombo instance;
    public bool canAttack;
    public bool canCombo;
    public Animator animator;
    [SerializeField] private GameObject slashRange;

    private void Awake()
    {
        instance = this;
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
        if (mousePos.x > playerScreenPoint.x)
        {
            transform.localScale = new Vector3(1, 1, 0);
            transform.localScale *= 1.2f;
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 0);
            transform.localScale *= 1.2f;
        }
        if (canAttack)
        {
            slashRange.SetActive(true);
        }
        else
        {
            slashRange.SetActive(false);
        }
        if (Input.GetMouseButtonDown(0))
        {
            canAttack = true;
        }
    }

}
