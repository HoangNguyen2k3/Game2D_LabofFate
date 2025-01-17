using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ManageRoomWait : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI timeRevert;
    private float timeAll = 20;
    public NetworkVariable<float> timeRemain = new NetworkVariable<float>(20,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Server);
    bool isDone = false;
    public Color redColor;
    [SerializeField] private GameObject enemiesTest;
    [SerializeField] private Transform positionSpawn;
    private GameObject enemyTest;
    private void Start()
    {
        timeRevert.text = timeAll.ToString();
        if (IsServer) { timeRemain.Value = timeAll; 
        enemyTest = Instantiate(enemiesTest,positionSpawn.position, Quaternion.identity);
        enemyTest.GetComponent<NetworkObject>().Spawn();
       enemyTest.GetComponent<EnemyAI>().isActive = false;
        enemyTest.GetComponent<EnemyPathFinding>().isIceFreeze = true;
        enemyTest.GetComponent<SemiBoss>().FrezzeWaiting();
        }
        timeRemain.OnValueChanged += OnRemainingTimeChanged;
        
    }
    private void Update()
    {
        if (isDone)
        {
            return;
        }
        if (IsServer)
        {
        timeRemain.Value -= Time.deltaTime;
        
        }
/*        if(timeRemain.Value <= 3&& timeRemain.Value >=1)
        {
            timeRevert.color = redColor;
        }*/
        if (timeRemain.Value <= 0)
        {
            GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject p in player)
            {
                p.GetComponent<PlayerController>().stopMovingInstruction = true;
            }
            isDone = true;
            if (IsServer)
            {
                if (enemyTest)
                {
                    enemyTest.GetComponent<NetworkObject>().Despawn();
                }               
               NetworkManager.Singleton.SceneManager.LoadScene("SampleScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
           
        }
    }
    private void OnRemainingTimeChanged(float oldValue, float newValue)
    {
        if (timeRemain.Value <= 0)
        {
            timeRevert.text = "START GAME";
        }
        else
        {
        int timer=(int)timeRemain.Value;
        timeRevert.text = timer.ToString();
        }

    }
}
