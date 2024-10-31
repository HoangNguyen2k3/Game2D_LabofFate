using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthPlayerBar : MonoBehaviour
{
    private PlayerHealth playerHealth;
    [SerializeField] private TextMeshProUGUI textHealth;
    // Start is called before the first frame update
    void Start()
    {
        playerHealth = FindFirstObjectByType<PlayerController>().gameObject.GetComponent<PlayerHealth>();
        transform.GetComponent<Slider>().maxValue = playerHealth.startingHealth;
        transform.GetComponent<Slider>().value = playerHealth.startingHealth;
        textHealth.text= playerHealth.startingHealth.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        transform.GetComponent<Slider>().value = playerHealth.currentHealth;
        textHealth.text = playerHealth.currentHealth.ToString()+"/"+ playerHealth.startingHealth.ToString();
    }
}
