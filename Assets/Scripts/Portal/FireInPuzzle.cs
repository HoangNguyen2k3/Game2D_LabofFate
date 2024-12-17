using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class FireInPuzzle : NetworkBehaviour
{
    [SerializeField] private List<Transform> trans;
    public bool isActive = false;
    private GameObject player_1;
    public int current_pos = 0;
    public float time_change_pos = 5f;
    public bool isWait = true;
    public bool isFrezze = false;
    public float timeFrezze = 1f;
    private void Update()
    {
        if (!isActive) { return; }
        if (isFrezze)
        {
            return;
        }
        if (isWait)
        {
            StartCoroutine(ChangePos());
        }
        if (player_1 == null&& FindFirstObjectByType<PlayerController>())
        {
            player_1 = FindFirstObjectByType<PlayerController>().gameObject;
            GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < player.Length; i++)
            {
                if (Vector2.Distance(transform.position, player[i].transform.position) <
                    Vector2.Distance(transform.position, player_1.transform.position))
                {
                    player_1 = player[i];
                }
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, player_1.transform.position) < 8f)
            {
                int temp;
                do
                {
                    temp = Random.Range(0, trans.Count - 1);
                } while (temp == current_pos);
                current_pos = temp;
                transform.position = trans[temp].position;
            }
        }

    }

    private IEnumerator ChangePos()
    {
        isWait = false;
        int temp;
        do
        {
            temp = Random.Range(0, trans.Count-1);
        } while (temp == current_pos);
        current_pos = temp;
        transform.position = trans[temp].position;
        yield return new WaitForSeconds(time_change_pos);
        
        isWait = true;
    }
    private IEnumerator Frezze()
    {
        isFrezze = true;
        yield return new WaitForSeconds(timeFrezze);
        isFrezze = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
/*        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }*/
        if (collision.GetComponent<ProjectilePlayer>())
        {
            StartCoroutine(Frezze());
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            Destroy(gameObject);
        }
    }
}
