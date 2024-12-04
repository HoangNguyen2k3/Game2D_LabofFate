using System.Collections;
using TreeEditor;
using UnityEngine;

public class Boss3Arm : MonoBehaviour
{
    public GameObject pivot;
    public GameObject target;
    public Animator animator;

    public float angularSpeed = 1f;
    public float circleRad = 1f;
    private float currentAngle;

    private Vector2 originalPos;
    private Vector2 targetPos;
    [SerializeField] private float attackSpeed = 0.3f;
    [SerializeField] private AnimationCurve attackCurve;

    private Vector2 defaultRotation;

    public bool isDonePunching = false;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        defaultRotation = this.transform.right;
    }

    public void Idle()
    {
        currentAngle += angularSpeed * Time.deltaTime;
        Vector2 offset = new Vector2 (Mathf.Sin(currentAngle), Mathf.Sin(2 * currentAngle)) * circleRad;
        
        transform.position = (Vector2)pivot.transform.position + offset;
    }

    public IEnumerator PunchTarget()
    {
        isDonePunching = false;
        
        originalPos = transform.position;
        var target = GetComponentInParent<Boss3>().target;
        if (!target)
        {
            yield break;
        }

        targetPos = target.transform.position;
        this.transform.right = transform.localScale.x == 1 ? targetPos - originalPos : -(targetPos - originalPos);
        
        // Move to target position
        float elapsedTime = 0;
        while (elapsedTime < attackSpeed)
        {
            transform.position = Vector2.Lerp(originalPos, targetPos, attackCurve.Evaluate(elapsedTime / attackSpeed));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        this.transform.position = targetPos;

        yield return new WaitForSeconds(1);
        this.transform.right = defaultRotation;

        // Move back to original position
        elapsedTime = 0;
        while (elapsedTime < attackSpeed)
        {
            transform.position = Vector2.Lerp(targetPos, originalPos, attackCurve.Evaluate(elapsedTime / attackSpeed));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        this.transform.position = originalPos;
        isDonePunching = true;
    }

}
