using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorldSettings
{
    [System.Serializable]
    public enum WorldState
    {
        Reset,
        Game,
        Menu
    }

    public static WorldState state = WorldState.Reset;
    public static float moveDelay = 0.25f;
    public static float movementIncrease = 0.0025f;
    public static float resetDelay = 6f;
}
