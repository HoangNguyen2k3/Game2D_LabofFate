using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelTimer : MonoBehaviour
{
    public Slider timerSlider;
    public Image timerSliderFillImage;
    public TextMeshProUGUI timerText;
    public float time;

    public Color neutralColor;
    public Color warningColor;
    public Color dangerColor;

    [field: SerializeField] public bool isDone {get; private set;}
    private bool stopTimer;

    private void Start() {
        stopTimer = false;
        timerSlider.maxValue = time;
        timerSlider.value = time;
    }

    private void Update() {
        if (isDone) return;

        UpdateSliderColor();
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        if (stopTimer)
        {
            timerText.text = "YOU ARE DOOMED!!!";
            isDone = true;
            return;
        }
        float _time = time - Time.time;

        int minutes = Mathf.FloorToInt(_time / 60);
        int seconds = Mathf.FloorToInt(_time - minutes * 60f);

        string textTime = string.Format("{0:0}:{1:00}", minutes, seconds);

        if (_time <= 0)
        {
            stopTimer = true;
        }

        if (!stopTimer)
        {
            timerText.text = textTime;
            timerSlider.value = _time;
        }
    }

    private void UpdateSliderColor()
    {
        Color color = neutralColor;
        if (timerSlider.value < time * 0.6f)
        {
            color = warningColor;
        }
        if (timerSlider.value < time * 0.3f)
        {
            color = dangerColor;
        }
        timerSliderFillImage.color = color;
    }
    

}
