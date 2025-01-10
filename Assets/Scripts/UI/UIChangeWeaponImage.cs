using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChangeWeaponImage : MonoBehaviour
{
    [SerializeField] private GameObject fire;
    [SerializeField] private GameObject ice;
    [SerializeField] private GameObject thunder;
    private PlayerController player;
    private void Start()
    {
        fire.SetActive(true);
        ice.SetActive(false);
        thunder.SetActive(false);
    }
    private void Update()
    {
        if(player == null)
        {
            if (FindFirstObjectByType<PlayerController>())
            {
                player = FindFirstObjectByType<PlayerController>();
            }
            else
            {
                return;
            }
        }
        if(player.transform.position.x > 138&&player.transform.position.x<390)
        {
            fire.SetActive(false);
            ice.SetActive(false);
            thunder.SetActive(true);
        }else if (player.transform.position.x >= 390)
        {
            fire.SetActive(false);
            ice.SetActive(true);
            thunder.SetActive(false);
        }else if (player.transform.position.x <= 138)
        {
            fire.SetActive(true);
            ice.SetActive(false);
            thunder.SetActive(false);
        }
    }
}
