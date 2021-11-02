using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class World : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private GameObject gameOverScreen;
    private float time = 0;

    private void SetTime()
    {
        timeText.text = ((int) time).ToString() + "s";
    }

    // Start is called before the first frame update
    void Start()
    {
        SetTime();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(WorldSettings.state);
        if(WorldSettings.state == WorldSettings.WorldState.Game)
        {
            time += Time.deltaTime;
            SetTime();
        }
        else
        {
            time = 0;
            SetTime();
        }
    }
}
