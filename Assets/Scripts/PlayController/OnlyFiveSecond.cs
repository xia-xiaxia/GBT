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
        // �����ͣ��
        CheckPause();

        if (isPaused)
        {
            return;
        }

        // �������Ƿ��ƶ�
        CheckPlayerMovement();

        // ��⼼���ͷ�
        CheckSkillUsage();

        // ������û���ƶ���δʹ�ü��ܣ���ʼ��ʱ
        if (!HasPlayerNotMovedOrUsedSkill())
        {
            idleTimer += Time.deltaTime;
        }
        else
        {
            idleTimer = 0f;
            //StartCoroutine(ResetIdleState());
        }

        // �������ʱ�䳬�����ƣ�����Ϸ����
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
            lastPosition = player.transform.position; // ����ƶ�ʱ��¼���λ��
        }
    }

    void CheckSkillUsage()
    {
        Debug.Log(isSkillPressed);
        // ��⼼���ͷŵ����
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
        // ��� Possessed ״̬
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
                Time.timeScale = 0f;  // ��ͣ��Ϸ
                Debug.Log("Game Paused");
            }
            else
            {
                Time.timeScale = 1f;  // �ָ���Ϸ
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


