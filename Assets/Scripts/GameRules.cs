using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameRules
{
    public static float topBound = 2.75f;
    public static float bottomBound = -2.75f;
    public static float rightBound = 6.75f;
    public static float leftBound = -6.75f;
    public static float step = 0.5f;
    public static int gridW = 28;
    public static int gridH = 12;
    public static List<GameObject> snake = new List<GameObject>();
}
