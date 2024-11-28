using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float idleTimeLimit = 5f;
    private float idleTimer = 0f;
    private Vector3 lastPosition;
    private bool isSkillPressed = false;
    public Metamorphosm Metamorphosm;
    public Possessed Possessed;
    public bool isGameOver;

    private bool isPaused = false;

    void Start()
    {
        lastPosition = transform.position;
        isGameOver = false;
    }

    void Update()
    {
        // ¼ì²âÔÝÍ£¼ü
        CheckPause();

        if (isPaused)
        {
            return;
        }

        // ¼ì²âÍæ¼ÒÊÇ·ñÒÆ¶¯
        CheckPlayerMovement();

        // ¼ì²â¼¼ÄÜÊÍ·Å
        CheckSkillUsage();

        if (HasPlayerNotMovedOrUsedSkill())
        {
            idleTimer += Time.deltaTime;
        }
        else
        {
            idleTimer = 0f;
        }

        if (idleTimer >= idleTimeLimit)
        {
            GameOver();
        }
    }

    void CheckPlayerMovement()
    {
        float moveInput = Input.GetAxis("Horizontal") + Input.GetAxis("Vertical");
        if (moveInput != 0f)
        {
            lastPosition = transform.position;
        }
    }

    void CheckSkillUsage()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Metamorphosm.isPlayerInRange && !Metamorphosm.isMark)
                isSkillPressed = true;
            else if (Metamorphosm.isMark)
                isSkillPressed = true;
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            if (Possessed.isPlayerInRange && Possessed.PM.enabled)
                isSkillPressed = true;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {

            if (Possessed.PM.enabled == false)
                isSkillPressed = true;
        }
    }

    bool HasPlayerNotMovedOrUsedSkill()
    {
        return transform.position == lastPosition && !isSkillPressed;
    }

    void GameOver()
    {
        Debug.Log("Game Over! Player did not perform any actions in time.");
        isGameOver = true;
    }

    void CheckPause()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            isPaused = !isPaused;

            if (isPaused)
            {
                Time.timeScale = 0f;
                Debug.Log("Game Paused");
            }
            else
            {
                Time.timeScale = 1f;
                Debug.Log("Game Resumed");
            }
        }
    }
}
