using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageIntroduceUI : MonoBehaviour
{
    [SerializeField] private GameObject introductionUI;

    private void Start()
    {
        introductionUI.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            introductionUI.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            introductionUI.SetActive(false);
        }
    }
}
