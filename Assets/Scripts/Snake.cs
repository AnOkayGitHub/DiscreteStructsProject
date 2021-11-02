using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private float moveDelay;
    [SerializeField] private GameObject segmentObject;
    [SerializeField] private int startSize = 4;
    [SerializeField] private GameObject munchPS;
    [SerializeField] private TextMeshProUGUI foodEatenText;

    private Vector2 direction = Vector2.right;
    private Vector2 startPosition;
    private List<Transform> segments = new List<Transform>();
    private bool canMove = true;
    private bool justAtefood = false;
    private bool waitingForReset = false;
    private float startDelay;
    private int foodEaten = 0;

    private void Start()
    {
        startPosition = transform.position;

        if(moveDelay == 0f)
        {
            moveDelay = WorldSettings.moveDelay;
        }
        startDelay = moveDelay;
    }

    private void Update()
    {
        if(!waitingForReset)
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

            if (canMove)
            {
                if (justAtefood)
                {
                    justAtefood = false;
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
        WorldSettings.state = WorldSettings.WorldState.Reset;

        moveDelay = startDelay;
        foodEaten = 0;
        foodEatenText.text = foodEaten.ToString();

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

        WaitForReset();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (WorldSettings.state == WorldSettings.WorldState.Game)
        {
            if (collision.gameObject.tag == "Food")
            {
                Grow();
                justAtefood = true;
                foodEaten += 1;
                foodEatenText.text = foodEaten.ToString();
                GameObject ps = (GameObject)Instantiate(munchPS);
                ps.transform.position = transform.position;
                Destroy(ps, 1f);
            }
            else if (collision.gameObject.tag == "Obstacle" && !justAtefood)
            {
                Reset();
            }
        }
        
    }

    private void WaitForReset()
    {
        waitingForReset = true;
        
    }
}
