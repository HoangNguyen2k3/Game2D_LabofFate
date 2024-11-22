using System.Collections;
using UnityEngine;

public class GolemEnemy : MonoBehaviour, IEnemy
{
    private Animator animator;

    [SerializeField] private float jumpHeight = 8f; 
    [SerializeField] private float jumpDuration = 3f; 
    [SerializeField] private float attackRange = 10f;
    [SerializeField] private Transform attackPostion;
    [SerializeField] private GameObject lightning;
    private EnemyAI enemyAI;


    private void Start()
    {
        enemyAI = GetComponent<EnemyAI>();
        animator = GetComponent<Animator>();
    }

    public void Attack()
    {
        Vector3 playerPos=FindFirstObjectByType<PlayerController>().transform.position;
        if (enemyAI.target)
        {
            playerPos = enemyAI.target.position;
        }
        
       


        if (Vector3.Distance(transform.position, playerPos) <= attackRange)
        {
            StartCoroutine(JumpToTarget(playerPos));
        }
    }

    private IEnumerator JumpToTarget(Vector3 targetPosition)
    {

        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;
        animator.SetTrigger("Attack");
        while (elapsedTime < jumpDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / jumpDuration;

            Vector3 currentPosition = Vector3.Lerp(startPosition, targetPosition, t);
            currentPosition.y += jumpHeight * Mathf.Sin(Mathf.PI * t); 
            transform.position = currentPosition;
           
            yield return null;
        }

        transform.position = targetPosition;
      
       
        yield return null;

    }
    public void AttackLightning()
    {
        Instantiate(lightning, attackPostion.position, Quaternion.identity);
    }

}
