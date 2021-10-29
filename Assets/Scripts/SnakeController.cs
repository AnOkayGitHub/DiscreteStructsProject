using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject snakePiece;

    private int id;
    private float step = GameRules.step;
    private Vector2 target;
    private Vector2 behind;

    private enum Direction
    {
        L,
        R,
        U,
        D
    };

    private void Start()
    {
        behind = transform.position;
        target = transform.position;
        GameRules.snake.Add(gameObject);
        id = GameRules.snake.IndexOf(gameObject);
    }

    private void Update()
    {
        DistanceCheck();
    }

    private void Testing()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Move(Direction.L);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Move(Direction.R);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            Move(Direction.U);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Move(Direction.D);
        }
    }

    public Vector2 GetBehind()
    {
        return behind;
    }

    private void DistanceCheck()
    {
        if (Vector2.Distance(transform.position, target) > 0.025f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, speed);
        }
        else
        {
            transform.position = target;
            Testing();
            
        }
    }

    private void Move(Direction dir)
    {
        if (id == 0)
        {
            switch (dir)
            {
                case Direction.L:
                    
                    target = new Vector2(transform.position.x - step, transform.position.y);
                    behind = new Vector2(transform.position.x, transform.position.y);
                    break;
                case Direction.R:
                    target = new Vector2(transform.position.x + step, transform.position.y);
                    behind = new Vector2(transform.position.x, transform.position.y);
                    break;
                case Direction.U:
                    target = new Vector2(transform.position.x, transform.position.y + step);
                    behind = new Vector2(transform.position.x, transform.position.y);
                    break;
                case Direction.D:
                    target = new Vector2(transform.position.x, transform.position.y - step);
                    behind = new Vector2(transform.position.x, transform.position.y);
                    break;
            }
        }
        else
        {
            target = GameRules.snake[id - 1].GetComponent<SnakeController>().GetBehind();
            behind = (Vector2) transform.position + ((Vector2) GameRules.snake[id - 1].transform.position - (Vector2) GameRules.snake[id - 1].GetComponent<SnakeController>().GetBehind());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Apple")
        {
            Destroy(collision.gameObject, 0f);
            GameObject newPiece = (GameObject)Instantiate(snakePiece, transform.parent.transform);
            newPiece.transform.position = GameRules.snake[GameRules.snake.Count - 1].GetComponent<SnakeController>().behind;
            newPiece.transform.localScale = new Vector2(1, 1);
        }
    }
}
