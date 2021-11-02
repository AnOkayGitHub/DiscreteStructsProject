using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private float moveDelay;
    [SerializeField] private GameObject segmentObject;
    [SerializeField] private int startSize = 4;

    private Vector2 direction = Vector2.right;
    private Vector2 startPosition;
    private List<Transform> segments = new List<Transform>();
    private bool canMove = true;
    private bool foodEaten = false;
    private float startDelay;
    
    

    private void Start()
    {
        startPosition = transform.position;

        if(moveDelay == 0f)
        {
            moveDelay = WorldSettings.moveDelay;
        }
        startDelay = moveDelay;

        Reset();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            direction = Vector2.up;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            direction = Vector2.left;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            direction = Vector2.down;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            direction = Vector2.right;
        }

        if(canMove)
        {
            if(foodEaten)
            {
                foodEaten = false;
            }

            for (int i = segments.Count - 1; i > 0; i--)
            {
                segments[i].position = segments[i - 1].position;
            }

            transform.position = new Vector3(
                (int)(transform.position.x + direction.x),
                (int)(transform.position.y + direction.y),
                0);

            StartCoroutine("WaitForMove");
        }
        
    }

    private IEnumerator WaitForMove()
    {
        canMove = false;
        yield return new WaitForSeconds(moveDelay);
        canMove = true;
    }

    private void Grow()
    {
        GameObject segment = (GameObject) Instantiate(segmentObject);
        segment.transform.position = segments[segments.Count - 1].position;
        segments.Add(segment.transform);
    }

    private void Reset()
    {
        moveDelay = startDelay;

        for (int i = 1; i < segments.Count; i++)
        {
            Destroy(segments[i].gameObject, 0f);
        }
        segments.Clear();
        segments.Add(transform);
        transform.position = startPosition;

        for(int i = 0; i < startSize; i++)
        {
            segments.Add(Instantiate(segmentObject).transform);
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Food")
        {
            Grow();
            foodEaten = true;
            //moveDelay -= WorldSettings.movementIncrease;
        } 
        else if (collision.gameObject.tag == "Obstacle" && !foodEaten)
        {
            Reset();
        }
    }
}
