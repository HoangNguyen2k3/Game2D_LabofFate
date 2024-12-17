using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerInstruction : MonoBehaviour
{
    [SerializeField] private GameObject instruction;
    private float timeIntruction = 28f;
    private bool onlyOneTimeActive = false;
    private void Awake()
    {
        instruction.SetActive(false);
    }
    void Start()
    {
        instruction.SetActive(false);
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(onlyOneTimeActive) { return; }
        if (collision.CompareTag("Player")&&instruction.activeSelf==false)
        {
            instruction.SetActive(true);
            StartCoroutine(stopPlayer(collision.gameObject));

        }
       
    }
    private IEnumerator stopPlayer(GameObject player)
    {
        player.GetComponent<PlayerController>().stopMovingInstruction = true;
        yield return new WaitForSeconds(timeIntruction);
        player.GetComponent<PlayerController>().stopMovingInstruction = false;
        onlyOneTimeActive = true;
        instruction.SetActive(false);

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        onlyOneTimeActive = true;
       // instruction.SetActive(false);
    }
}
