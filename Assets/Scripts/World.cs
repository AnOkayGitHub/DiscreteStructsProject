using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class World : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;

    private float time = 0;

    private void SetTime()
    {
        timeText.text = ((int) time).ToString() + "s";
    }

    // Start is called before the first frame update
    void Start()
    {
        if(timeText.gameObject.activeSelf)
        {
            SetTime();
        }

        if(WorldSettings.fadeAnimator == null)
        {
            WorldSettings.fadeAnimator = GameObject.FindGameObjectWithTag("UIFade").GetComponent<Animator>();
        }

        if (WorldSettings.food == null)
        {
            WorldSettings.food = GameObject.FindGameObjectWithTag("Food").GetComponent<Food>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTime();
        SetTime();
        Debug.Log(WorldSettings.state);
        switch(WorldSettings.state)
        {
            case WorldSettings.WorldState.MainMenu:
                if(!mainMenu.activeSelf || settingsMenu.activeSelf)
                {
                    settingsMenu.SetActive(false);
                    mainMenu.SetActive(true);
                }
                break;
            case WorldSettings.WorldState.SettingsMenu:
                if (mainMenu.activeSelf || !settingsMenu.activeSelf)
                {
                    mainMenu.SetActive(false);
                    settingsMenu.SetActive(true);
                }
                break;
        }
    }

    private void UpdateTime()
    {
        if (WorldSettings.state == WorldSettings.WorldState.Game)
        {
            time += Time.deltaTime;
        }
        else
        {
            time = 0;
        }
    }

    public void Settings()
    {
        SongManager.PlayButtonSound();
        WorldSettings.state = WorldSettings.WorldState.SettingsMenu;
        Debug.Log("State: Settings");
    }

    public void MainMenu()
    {
        SongManager.PlayButtonSound();
        WorldSettings.state = WorldSettings.WorldState.MainMenu;
        Debug.Log("State: Main");
    }
}
