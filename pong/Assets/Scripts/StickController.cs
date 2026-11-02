using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StickController : MonoBehaviour
{
    [Header("Paddle Settings")]
    public GameObject Lstick;
    public GameObject Rstick;
    public float speed = 10;
    public float limit = 12f;
    public bool useAI = false;
    public float AISpeedReducer = 0.67f;

    [Header("Keybinds")]
    public KeyCode L_UpKey = KeyCode.D;
    public KeyCode L_DownKey = KeyCode.A;
    public KeyCode R_UpKey = KeyCode.LeftArrow;
    public KeyCode R_DownKey = KeyCode.RightArrow;

    [Header("Ball Settings")]
    public GameObject ballPrefab;
    private GameObject currentBall;
    public float xspeed;
    public float yspeed;
    public float heightLimit;
    public float widthLimit;
    public float secondsBeforeRespawn = 3;
    private float waitForSec = 0;
    private BallScript ballScript;

    public Transform ballStartPos;

    [Header("Score")]
    public TextMeshProUGUI LscoreTxt;
    public TextMeshProUGUI RscoreTxt;
    public int Rscore = 0;
    public int Lscore = 0;
    public bool leftWonLast = true;

    private void Start()
    {

    }

    void Update()
    {
        if (currentBall == null)
        {
            WaitAndRespawn();
        }
        moveLstick();
        moveRstick();
    }

    private void moveLstick()
    {
        float LstickTop = Lstick.transform.position.y + (Lstick.transform.lossyScale.y);
        float LstickBottom = Lstick.transform.position.y - (Lstick.transform.lossyScale.y);
        if (!useAI)
        {
            if (Input.GetKey(L_UpKey) && LstickTop <= ballStartPos.position.y + limit)
            {
                Lstick.transform.position += new Vector3(0, speed * Time.deltaTime);
            }

            if (Input.GetKey(L_DownKey) && LstickBottom >= ballStartPos.position.y - limit)
            {
                Lstick.transform.position += new Vector3(0, -speed * Time.deltaTime);
            }
        }

        else
        {
            if (currentBall != null)
            {
                float Ydifference = (Lstick.transform.position - currentBall.transform.position).y;
                if (Ydifference < 0 && LstickTop <= ballStartPos.position.y + limit)
                {
                    Lstick.transform.position += new Vector3(0, speed * AISpeedReducer * Time.deltaTime);
                }

                if (Ydifference > 0 && LstickBottom >= ballStartPos.position.y - limit)
                {
                    Lstick.transform.position += new Vector3(0, -speed * AISpeedReducer * Time.deltaTime);
                }
            }
        }
    }

    private void moveRstick()
    {

        float RstickTop = Rstick.transform.position.y + (Rstick.transform.lossyScale.y);
        float RstickBottom = Rstick.transform.position.y - (Rstick.transform.lossyScale.y);
        if (Input.GetKey(R_UpKey) && RstickTop <= ballStartPos.position.y + limit)
        {
            Rstick.transform.position += new Vector3(0, speed * Time.deltaTime);
        }

        if (Input.GetKey(R_DownKey) && RstickBottom >= ballStartPos.position.y - limit)
        {
            Rstick.transform.position += new Vector3(0, -speed * Time.deltaTime);
        }
    }

    private void WaitAndRespawn()
    {
        waitForSec += Time.deltaTime;
        if (waitForSec >= secondsBeforeRespawn)
        {
            waitForSec = 0;
            currentBall = Instantiate(ballPrefab);
            BallScript currentBallScript = currentBall.GetComponent<BallScript>();
            currentBallScript.controlScript = gameObject.GetComponent<StickController>();
            currentBallScript.Lscore = Lscore;
            currentBallScript.Rscore = Rscore;
            currentBallScript.LscoreTxt = LscoreTxt;
            currentBallScript.RscoreTxt = RscoreTxt;
            currentBallScript.ballStartPos = ballStartPos;
            currentBallScript.xspeed = xspeed;
            currentBallScript.yspeed = yspeed;
            currentBallScript.heightLimit = heightLimit;
            currentBallScript.widthLimit = widthLimit;
            currentBallScript.leftWonLast = leftWonLast;
        }
    }
}
