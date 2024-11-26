using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LevelTimer : NetworkBehaviour
{
    public Slider timerSlider;
    public Image timerSliderFillImage;
    public TextMeshProUGUI timerText;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private float totalTime;
    public NetworkVariable<float> remainingTime = new NetworkVariable<float>();

    public Color neutralColor;
    public Color warningColor;
    public Color dangerColor;
    public static LevelTimer Instance { get; private set; }
    [field: SerializeField] public bool isDone { get; private set; }
    private bool stopTimer;

    private void Start()
    {

        if (IsServer)
        {
            // Server initializes the timer
            Instance = this;
            remainingTime.Value = totalTime;
            stopTimer = false;
        }

        timerSlider.maxValue = totalTime;
        timerSlider.value = totalTime;
    }

    private void Update()
    {
        if (isDone) return;

        if (IsServer)
        {
            UpdateTimerOnServer();
        }

        UpdateSliderColor();
        UpdateTimerUI();
    }

    private void UpdateTimerOnServer()
    {
        if (stopTimer)
        {

            isDone = true;
            Instantiate(gameOver);
            return;
        }

        remainingTime.Value -= Time.deltaTime;

        if (remainingTime.Value <= 0)
        {
            remainingTime.Value = 0;
            stopTimer = true;
        }
    }

    private void UpdateTimerUI()
    {
        float _time = remainingTime.Value;

        int minutes = Mathf.FloorToInt(_time / 60);
        int seconds = Mathf.FloorToInt(_time - minutes * 60f);

        string textTime = string.Format("{0:0}:{1:00}", minutes, seconds);

        if (stopTimer)
        {
            timerText.text = "YOU ARE DOOMED!!!";
            return;
        }

        timerText.text = textTime;
        timerSlider.value = _time;
    }

    private void UpdateSliderColor()
    {
        Color color = neutralColor;
        if (timerSlider.value < totalTime * 0.6f)
        {
            color = warningColor;
        }
        if (timerSlider.value < totalTime * 0.3f)
        {
            color = dangerColor;
        }
        timerSliderFillImage.color = color;
    }
}
