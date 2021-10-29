using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    private float t = 0;

    void Update()
    {
        timerText.text = UpdateTimer() + "s";
    }

    private string UpdateTimer()
    {
        t += Time.deltaTime;
        return ((int) t).ToString();
    }
}
