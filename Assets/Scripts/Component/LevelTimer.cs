using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LevelTimer : NetworkBehaviour
{
    public Slider timerSlider;
    public Image timerSliderFillImage;
    public TextMeshProUGUI timerText;

    [SerializeField] private GameObject gameOverUI;
    [SerializeField] public float totalTime;
    public NetworkVariable<float> remainingTime = new NetworkVariable<float>();

    public Color neutralColor;
    public Color warningColor;
    public Color dangerColor;

    public static LevelTimer Instance { get; private set; }

    private bool isTimerStopped;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (IsServer)
        {
            remainingTime.Value = totalTime;
            isTimerStopped = false;
        }

        timerSlider.maxValue = totalTime;
        timerSlider.value = totalTime;

        remainingTime.OnValueChanged += OnRemainingTimeChanged;
    }

    private void Update()
    {
        if (IsServer && !isTimerStopped)
        {
            UpdateTimerOnServer();
        }
    }

    private void UpdateTimerOnServer()
    {
        remainingTime.Value -= Time.deltaTime;

        if (remainingTime.Value <= 0)
        {
            remainingTime.Value = 0;
            isTimerStopped = true;
            ShowGameOverUI();
        }
    }

    private void OnRemainingTimeChanged(float oldValue, float newValue)
    {
        UpdateTimerUI();
        UpdateSliderColor();
    }

    private void UpdateTimerUI()
    {
        float time = remainingTime.Value;

        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        string formattedTime = $"{minutes:0}:{seconds:00}";
        timerText.text = isTimerStopped ? "YOU ARE DOOMED!!!" : formattedTime;

        timerSlider.value = time;
    }

    private void UpdateSliderColor()
    {
        float ratio = timerSlider.value / totalTime;

        if (ratio > 0.6f)
            timerSliderFillImage.color = neutralColor;
        else if (ratio > 0.3f)
            timerSliderFillImage.color = warningColor;
        else
            timerSliderFillImage.color = dangerColor;
    }

    public void ResetTimerFirst()
    {
        if (IsServer)
        {
            remainingTime.Value = 600;
            isTimerStopped = false;
        }
    }
    public void ResetTimerSecond()
    {
        if (IsServer)
        {
            remainingTime.Value = 600;
            isTimerStopped = false;
        }
    }
    private void ShowGameOverUI()
    {
        if (gameOverUI != null)
        {
            Instantiate(gameOverUI);
        }
    }
}
