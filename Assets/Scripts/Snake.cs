using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private float moveDelay;
    [SerializeField] private float loseWaitTime;
    [SerializeField] private int startSize = 4;
    [SerializeField] private TextMeshProUGUI foodEatenText;
    [SerializeField] private TextMeshProUGUI menuText;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private TextMeshProUGUI countDownText;
    [SerializeField] private GameObject[] menuObjects;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject segmentObject;
    [SerializeField] private GameObject munchPS;
    [SerializeField] private GameObject snakeExplodePS;
    [SerializeField] private AudioClip[] clips;

    private List<Transform> segments = new List<Transform>();
    private GameObject gfx;
    private Vector2 direction = Vector2.right;
    private Vector2 startPosition;
    private bool canMove = true;
    private bool justAtefood = false;
    private bool waitingForReset = false;
    private bool gameOver = false;
    private bool countdown = false;
    private bool gfxOff = false;
    private float countDownTimer;
    private float startDelay;
    private int foodEaten = 0;
    private AudioSource audioSource;
    

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        startPosition = transform.position;
        countDownTimer = 0;

        if(moveDelay == 0f)
        {
            moveDelay = WorldSettings.moveDelay;
        }

        startDelay = moveDelay;

        gfx = transform.GetChild(0).gameObject;

        Reset(false);
    }

    private void Update()
    {
        if(!waitingForReset && !gameOver)
        {
            Move();
        }

        UpdateCountdown();
        UpdateGFX();
    }

    private void UpdateGFX()
    {
        if(direction == Vector2.right)
        {
            gfx.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, -90f));
        } 
        else if(direction == Vector2.left)
        {
            gfx.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 90f));
        }
        else if (direction == Vector2.up)
        {
            gfx.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        }
        else if (direction == Vector2.down)
        {
            gfx.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 180f));
        }

        if(WorldSettings.state != WorldSettings.WorldState.Game && !gfxOff)
        {
            gfxOff = true;
            for (int i = 1; i < segments.Count; i++)
            {
                segments[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        else if (WorldSettings.state == WorldSettings.WorldState.Game && gfxOff)
        {
            gfxOff = false;
            for (int i = 1; i < segments.Count; i++)
            {
                segments[i].transform.GetChild(0).gameObject.SetActive(true);
            }
        }

    }

    private void UpdateCountdown()
    {
        if (countDownTimer > 1)
        {
            countDownTimer -= Time.deltaTime;
            countDownText.text = "Starting in " + ((int)countDownTimer).ToString() + "...";
        }
        else
        {
            countDownText.text = "";
        }
    }

    private void Move()
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

    private void Grow()
    {
        GameObject segment = (GameObject) Instantiate(segmentObject);
        segment.transform.position = segments[segments.Count - 1].position;
        segments.Add(segment.transform);
    }

    private void Reset(bool over)
    {
        gameOver = over;
        WorldSettings.fadeAnimator.Play("FadeFromBlack", -1, 0f);
        foreach (GameObject g in menuObjects)
        {
            g.gameObject.SetActive(false);
        }
        
        if (gameOver)
        {
            gameOverScreen.gameObject.SetActive(true);
            menuText.text = "Game Over";
            buttonText.text = "Play Again";
        }
        else
        {
            gameOverScreen.gameObject.SetActive(true);
            menuText.text = "Snake";
            buttonText.text = "Play";
        }

        
        WorldSettings.state = WorldSettings.WorldState.MainMenu;
        waitingForReset = true;
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

    }

    private IEnumerator WaitForPlay()
    {
        foreach (GameObject g in menuObjects)
        {
            g.gameObject.SetActive(true);
        }
        WorldSettings.fadeAnimator.Play("FadeFromBlack", -1, 0f);
        yield return new WaitForSeconds(WorldSettings.resetDelay);

        waitingForReset = false;
        WorldSettings.state = WorldSettings.WorldState.Game;


    }

    private IEnumerator WaitForMove()
    {
        canMove = false;
        yield return new WaitForSeconds(moveDelay);
        canMove = true;
    }

    public void Play()
    {
        SongManager.PlayButtonSound();
        gameOverScreen.gameObject.SetActive(false);

        if (gameOver)
        {
            gameOver = false;
        }

        countDownTimer = WorldSettings.resetDelay + 1;
        StartCoroutine("WaitForPlay");
    }

    private void PlaySound(AudioClip sound)
    {
        audioSource.clip = sound;
        audioSource.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (WorldSettings.state == WorldSettings.WorldState.Game)
        {
            if (collision.gameObject.tag == "Food")
            {
                PlaySound(clips[0]);
                Grow();
                justAtefood = true;
                foodEaten += 1;
                foodEatenText.text = foodEaten.ToString();
                GameObject ps = (GameObject)Instantiate(munchPS);
                ps.transform.position = transform.position;
                Destroy(ps, 1f);

                //moveDelay -= WorldSettings.movementIncrease;
            }
            else if (collision.gameObject.tag == "Obstacle" && !justAtefood)
            {
                WorldSettings.fadeAnimator.Play("FadeToBlack", -1, 0f);
                StartCoroutine("LoseGame");
                
            }
        }

    }

    private IEnumerator LoseGame()
    {
        for (int i = 0; i < segments.Count; i++)
        {
            segments[i].GetChild(0).gameObject.SetActive(false);
            Vector2 pos = segments[i].transform.position;
            GameObject snakeExplode = (GameObject)Instantiate(snakeExplodePS);
            snakeExplode.transform.position = pos;
            Destroy(snakeExplode, 1f);
        }

        gameOver = true;
        yield return new WaitForSeconds(loseWaitTime);

        for (int i = 0; i < segments.Count; i++)
        {
            segments[i].GetChild(0).gameObject.SetActive(true);
        }

        Reset(true);
    }
}
