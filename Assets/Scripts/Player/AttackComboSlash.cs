using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackComboSlash : MonoBehaviour
{
    public Animator anim;
    public int combo;
    public bool attack;
    public Transform player;
    [SerializeField] private float scalePlayer = 1.2f;
    [SerializeField] private GameObject damageRangeTrigger;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Start_Combo()
    {
        attack = false;
        if (combo < 3)
        {
            combo++;
        }
    }

    public void Finish_Anim()
    {
        attack = false;
        combo = 0;
    }

    public void Combos_()
    {
        if (Input.GetMouseButton(0) && !attack)
        {
            attack = true;
            anim.SetTrigger("" + combo);
        }
    }

    private void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(player.position);

        if (mousePos.x > playerScreenPoint.x)
        {
            player.localScale = new Vector3(scalePlayer, scalePlayer, 0);
        }
        else
        {
            player.localScale=new Vector3((-1)*scalePlayer, scalePlayer, 0);
        }

        if (attack == true) 
        {
            damageRangeTrigger.SetActive(true);
        }
        else
        {
            damageRangeTrigger.SetActive(false);
        }
        Combos_();
    }

}
