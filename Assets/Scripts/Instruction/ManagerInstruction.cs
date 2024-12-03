using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerInstruction : MonoBehaviour
{
    [SerializeField] private GameObject instruction;
    private void Awake()
    {
        instruction.SetActive(false);
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&&instruction.activeSelf==false)
        {
            instruction.SetActive(true);
        }
       
    }
}
