using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBox : MonoBehaviour
{
    [field:SerializeField] SpriteRenderer spriteRenderer;
    private SokobanManager sokobanManager;
    public Sprite activeSprite;
    public Sprite inactiveSprite;
    public bool active = false;

    private void Awake()
    {
        sokobanManager = GetComponentInParent<SokobanManager>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Active()
    {
        active = true;
        spriteRenderer.sprite = activeSprite;
    }

    public void Inactive()
    {
        active = false;
        spriteRenderer.sprite = inactiveSprite;
        sokobanManager.targetActiveCount--;
    }
}
