using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorldSettings
{
    [System.Serializable]
    public enum WorldState
    {
        Menu,
        Game,
    }

    public static WorldState state = WorldState.Menu;
    public static float moveDelay = 0.25f;
    public static float movementIncrease = 0.0025f;
    public static float resetDelay = 3f;
    public static Food food = null;
    public static Animator fadeAnimator;
}
