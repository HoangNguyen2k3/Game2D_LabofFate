using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveMinimap : MonoBehaviour
{

    [SerializeField] private GameObject map1;
    [SerializeField] private GameObject map2;
    [SerializeField] private GameObject map3;
    [SerializeField] private PlayerController player;
    private void Start()
    {
        map1.SetActive(true);
        map2.SetActive(false);
        map3.SetActive(false);
    }
    void Update()
    {
        if (player == null)
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
        if (player.transform.position.x > 138 && player.transform.position.x < 390)
        {
            map1.SetActive(false);
            map2.SetActive(false);
            map3.SetActive(true);
        }
        else if (player.transform.position.x >= 390)
        {
            map1.SetActive(false);
            map2.SetActive(true);
            map3.SetActive(false);
        }
        else if (player.transform.position.x <= 138)
        {
            map1.SetActive(true);
            map2.SetActive(false);
            map3.SetActive(false);
        }
    }
}
