using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerEnemy : MonoBehaviour,IEnemy
{
    private Animator animator;
    [SerializeField] private GameObject Mine;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Attack()
    {
        animator.SetTrigger("Attack");
    }
    public void SpawnBoom()
    {
        Instantiate(Mine,transform.position,Quaternion.identity);
    }
}
