using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MineFlyerEnemy : MonoBehaviour
{
    [SerializeField] private GameObject Bloom;
    private Animator BloomAnimator;
    private bool isActive = false;
    [SerializeField] private float timeToDestroy;
    // Start is called before the first frame update
    void Start()
    {
        BloomAnimator = GetComponent<Animator>();
        StartCoroutine(WaitToDestroyMine());
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActive) { return; }
        if (collision.CompareTag("Player"))
        {
            BloomAnimator.SetTrigger("Active");
            isActive = true;
        }
    }
    private IEnumerator WaitToDestroyMine()
    {
        yield return new WaitForSeconds(timeToDestroy);
        BloomAnimator.SetTrigger("Active");
        isActive = true;
    }
    public void SpawnBloomAttackPlayer()
    {
        Instantiate(Bloom, transform.position,Quaternion.identity);
    }
    public void Done()
    {
        Destroy(gameObject);
    }
}
