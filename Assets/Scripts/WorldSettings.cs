using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorldSettings
{
    [System.Serializable]
    public enum WorldState
    {
        MainMenu,
        SettingsMenu,
        Game,
    }

    public static WorldState state = WorldState.MainMenu;
    public static float moveDelay = 0.25f;
    public static float movementIncrease = 0.0025f;
    public static float resetDelay = 3f;
    public static Food food = null;
    public static Animator fadeAnimator;
    public static SongManager songManager;
}
