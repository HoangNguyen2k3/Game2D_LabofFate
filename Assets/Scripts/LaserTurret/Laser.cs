using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public Transform target;
    [SerializeField] private GameObject bloom;
    // Start is called before the first frame update
    void Start()
    {

        target = GameObject.FindGameObjectWithTag("Player").transform;
        Vector3 direction = (-target.position + transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x);
        transform.rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg - 180f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Instantiate(bloom, collision.transform.position, Quaternion.identity);
            StartCoroutine(ReturnPlayerToBegin(collision.gameObject));
        }
    }
    private IEnumerator ReturnPlayerToBegin(GameObject player)
    {
        yield return new WaitForSeconds(0.2f);
        player.transform.position = ManagePuzzleRoom.Instance.returnPlayer.position;
    }
}
