using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private BoxCollider2D grid;
    [SerializeField] private bool justSpawned = true;

    private void Start()
    {
        RandomizePosition();
    }

    private void RandomizePosition()
    {
        Bounds bounds = grid.bounds;
        float newX = Random.Range(bounds.min.x, bounds.max.x);
        float newY = Random.Range(bounds.min.y, bounds.max.y);

        transform.position = new Vector3(
                (int)(newX),
                (int)(newY),
                0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        RandomizePosition();
    }
}
