using System.Collections;
using UnityEngine;

public class Boss3Arm : MonoBehaviour
{
    public GameObject pivot;
    public GameObject target;
    public Animator animator;
    public Sprite closedFistSprite;
    public Sprite openFistSprite;
    public SpriteRenderer spriteRenderer;
    private Collider2D collider2d;
    private Boss3ArmLaserBeam laserBeam;

    public float angularSpeed = 1f;
    public float circleRad = 1f;
    private float currentAngle;

    [SerializeField] private AnimationCurve moveCurve;
    private Vector2 defaultRotation;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2d = GetComponent<Collider2D>();
        laserBeam = GetComponentInChildren<Boss3ArmLaserBeam>();
    }

    private void Start()
    {
        defaultRotation = (transform.localScale.x == 1) ? Vector2.down: Vector2.up;
        transform.right = defaultRotation;
        spriteRenderer.sprite = closedFistSprite;
        DisableHitbox();
    }

    public void Idle()
    {
        currentAngle += angularSpeed * Time.deltaTime;
        Vector2 offset = new Vector2 (Mathf.Sin(currentAngle), Mathf.Sin(2 * currentAngle)) * circleRad;
        
        transform.position = (Vector2)pivot.transform.position + offset;
    }

    public void ShootLaser()
    {
        laserBeam.Enable();
        OpenFist();
    }

    public void StopShootLaser()
    {
        laserBeam.Disable();
        ClosedFist();
    }

    public IEnumerator Move(Vector2 _from, Vector2 _to, float _time, bool useCurve = true)
    {
        transform.position = _from;
        float elapsedTime = 0;
        while (elapsedTime < _time)
        {
            transform.position = Vector2.Lerp(_from, _to, useCurve ? moveCurve.Evaluate(elapsedTime / _time) : elapsedTime / _time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        this.transform.position = _to;
    }

    public void MoveBackToOrg()
    {
        StartCoroutine(Move(transform.position, (Vector2)pivot.transform.position, 1f));
        ClosedFist();
        transform.right = defaultRotation;
    }

    public void EnableHitbox()
    {
        collider2d.enabled = true;
    }

    public void DisableHitbox()
    {
        collider2d.enabled = false;
    }

    public void OpenFist()
    {
        spriteRenderer.sprite = openFistSprite;
    }

    public void ClosedFist()
    {
        spriteRenderer.sprite = closedFistSprite;
    }

}
