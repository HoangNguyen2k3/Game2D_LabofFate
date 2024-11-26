using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SokobanBoxArea : MonoBehaviour
{
    [field: SerializeField] public bool isActive {get; private set;}
    public Sprite activeSprite;
    public Sprite inactiveSprite;

    private SpriteRenderer spriteRenderer;
    private SokobanManager sokobanManager;

    private void Awake() 
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        sokobanManager = GetComponentInParent<SokobanManager>();    
    }

    private void Start()
    {
        spriteRenderer.sprite = inactiveSprite;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("SokobanBox"))
        {
            Active();
            sokobanManager.areaActiveCount++;
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("SokobanBox")) return;
        if(!other)
        {
            Deactive();   
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("SokobanBox"))
        {
            Deactive();
            sokobanManager.areaActiveCount--;
        }
    }

    public void Active()
    {
        isActive = true;
        spriteRenderer.sprite = activeSprite;
    } 

    public void Deactive()
    {
        isActive = false;
        spriteRenderer.sprite = inactiveSprite;
    }


}
