using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float idleTimeLimit = 5f;
    public float idleTimer;
    private Vector3 lastPosition;
    private bool isSkillPressed;
    public Metamorphosm Metamorphosm;
    public Possessed[] possessed;
    public bool isGameOver;
    private GameObject player;

    private bool isPaused;
    private bool isPlayerInRange;
    private bool isPossessed;
    public int n;

    void Start()
    {
        lastPosition = transform.position;
        isSkillPressed = false;
        isGameOver = false;
        isPlayerInRange = false;
        isPaused = false;
        idleTimer = 0f;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        // 检测暂停键
        CheckPause();

        if (isPaused)
        {
            return;
        }

        // 检测玩家是否移动
        CheckPlayerMovement();

        // 检测技能释放
        CheckSkillUsage();

        // 如果玩家没有移动或未使用技能，则开始计时
        if (!HasPlayerNotMovedOrUsedSkill())
        {
            idleTimer += Time.deltaTime;
        }
        else
        {
            idleTimer = 0f;
            //StartCoroutine(ResetIdleState());
        }

        // 如果空闲时间超过限制，则游戏结束
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
            lastPosition = player.transform.position; // 玩家移动时记录最后位置
        }
    }

    void CheckSkillUsage()
    {
        Debug.Log(isSkillPressed);
        // 检测技能释放的情况
        if (Metamorphosm.isPlayerInRange && !Metamorphosm.isMark)
        {
            Debug.Log(1);
            if (Input.GetKeyUp(KeyCode.F))
            {
                Debug.Log("a");
                isSkillPressed = true;
            }
        }
        else if (Metamorphosm.isMark)
        {
            Debug.Log(2);
            if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("b");
                isSkillPressed = true;
            }
        }
        if ((isPlayerInRange))
        {
            Debug.Log(3);
            if (!isPossessed)
            {
                Debug.Log(3.5f);
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    isSkillPressed = true;
                    isPossessed = true;
                }
            }
        }
        else if (isPossessed)
        {
            Debug.Log(4);
            if (Input.GetKeyDown(KeyCode.E))
            {
                isSkillPressed = true;
                isPossessed = false;
            }
        }

    }

    void possessedBool()
    {
        // 检查 Possessed 状态
        for (int i = 0; i < n; i++)
        {
            {
                if (possessed[i].isPlayerInRange == true)
                    isPlayerInRange = true;
            }
        }
    }

    bool HasPlayerNotMovedOrUsedSkill()
    {
        return player.transform.position == lastPosition || isSkillPressed;
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
                Time.timeScale = 0f;  // 暂停游戏
                Debug.Log("Game Paused");
            }
            else
            {
                Time.timeScale = 1f;  // 恢复游戏
                Debug.Log("Game Resumed");
            }
        }
    }

    IEnumerator ResetIdleState()
    {
        yield return new WaitForSeconds(0.02f);
        isSkillPressed = false;
        isPlayerInRange = false;
    }
}


