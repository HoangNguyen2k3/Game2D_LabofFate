using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionSprite : MonoBehaviour
{
    public Sprite activeSprite;
    public Sprite inactiveSprite;
    public Sprite inactiveSpriteUp;
    public Sprite inactiveSpriteDown;
    public Sprite inactiveSpriteLeft;
    public Sprite inactiveSpriteRight;
    public Sprite activeSpriteUp;
    public Sprite activeSpriteDown;
    public Sprite activeSpriteLeft;
    public Sprite activeSpriteRight;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        spriteRenderer.sprite = inactiveSprite;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Active()
    {
        spriteRenderer.sprite = activeSprite;
    }

    public void InActive()
    {
        spriteRenderer.sprite = inactiveSprite;
    }
}
