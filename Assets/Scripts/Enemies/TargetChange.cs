using System;
using Unity.Netcode;
using UnityEngine;

public class TargetChange : NetworkBehaviour
{
    [SerializeField] private float timeChangeTarget = 1f;
    private float timeChange = 0f;
    public GameObject target;

    public event Action<GameObject> OnTargetChanged;

    private void Update()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
        if (timeChange < timeChangeTarget)
        {
            timeChange += Time.deltaTime;
        }
        else
        {
            timeChange = 0f;
            GameObject newTarget = UpdateTargetPlayer();
            if (newTarget != target)
            {
                target = newTarget;
                OnTargetChanged?.Invoke(target); 
            }
        }
    }

    private GameObject UpdateTargetPlayer()
    {
        if (target != null)
        {
            GameObject newTranform = target;
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            if(players.Length == 1) { return target; }
            for (int i = 0; i < players.Length; i++)
            {
                if (Vector2.Distance(transform.position, players[i].transform.position) <
                    Vector2.Distance(transform.position, newTranform.transform.position))
                {
                    newTranform = players[i];
                }
            }
            return newTranform;
        }
        return target;
    }
}
