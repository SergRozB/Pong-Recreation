using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BallScript : MonoBehaviour
{
    public float xspeed;
    public float yspeed;
    public Transform ballStartPos;
    private Rigidbody2D rb;
    public float heightLimit;
    public float widthLimit;

    public TextMeshProUGUI LscoreTxt;
    public TextMeshProUGUI RscoreTxt;
    public int Rscore;
    public int Lscore;

    public StickController controlScript;

    public bool leftWonLast;

    Vector3 topRCorner;
    Vector3 topLCorner;
    Vector3 bottomRCorner;
    Vector3 bottomLCorner;


    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        topRCorner = ballStartPos.position + new Vector3(widthLimit, heightLimit);
        topLCorner = ballStartPos.position + new Vector3(-widthLimit, heightLimit);
        bottomRCorner = ballStartPos.position + new Vector3(widthLimit, -heightLimit);
        bottomLCorner = ballStartPos.position + new Vector3(-widthLimit, -heightLimit);
        //Debug.Log("R corner:" + topRCorner);
        rb.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0);
        if (leftWonLast)
            rb.transform.position += new Vector3(xspeed * Time.deltaTime, yspeed * Time.deltaTime);
        else
            rb.transform.position += new Vector3(-1 * xspeed * Time.deltaTime, yspeed * Time.deltaTime);
        CheckIfCorner();
        Limits();
    }

    void Limits()
    {

        if (transform.position.x + transform.lossyScale.x >= ballStartPos.position.x + widthLimit) // if ball passes right boundry
        {
            controlScript.leftWonLast = true;
            Lscore += 1;
            LscoreTxt.text = Lscore.ToString();
            controlScript.Lscore = Lscore;
            Destroy(gameObject);
        }

        if (transform.position.x - transform.lossyScale.x <= ballStartPos.position.x - widthLimit) // if ball passes left boundry
        {
            controlScript.leftWonLast = false;
            Rscore += 1;
            RscoreTxt.text = Rscore.ToString();
            controlScript.Rscore = Rscore;
            Destroy(gameObject);
        }

        if (transform.position.y + transform.lossyScale.y >= ballStartPos.position.y + heightLimit) // if ball passes upper boundry
        {
            yspeed *= -1;
            transform.position -= new Vector3(0, 0.5f);
        }

        if (transform.position.y - transform.lossyScale.y <= ballStartPos.position.y - heightLimit) // if ball passes lower boundry
        {
            yspeed *= -1;
            transform.position += new Vector3(0, 0.5f);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    { 
        float difference = Mathf.Abs(collision.transform.position.y - transform.position.y);
        float maxPossibleDiff = collision.transform.lossyScale.y / 2;
        float interp = Mathf.InverseLerp(0, maxPossibleDiff, difference); //interpolation of distance between ball position to collider transform position
        xspeed *= -1;
        if (yspeed <= 0)
            yspeed = Mathf.Lerp(0f, xspeed, interp);
        else
            yspeed = -1 * Mathf.Lerp(0f, xspeed, interp);
    }

    void CheckIfCorner()
    {
        if (transform.position == topLCorner - new Vector3(transform.lossyScale.x, -transform.lossyScale.y))
        {
            Rscore += 100;
            RscoreTxt.text = Rscore.ToString();
            controlScript.Rscore = Rscore;
        }

        if (transform.position == topRCorner - new Vector3(-transform.lossyScale.x, -transform.lossyScale.y))
        {
            Lscore += 100;
            LscoreTxt.text = Lscore.ToString();
            controlScript.Lscore = Lscore;
        }

        if (transform.position == bottomLCorner - new Vector3(transform.lossyScale.x, transform.lossyScale.y))
        {
            Rscore += 100;
            RscoreTxt.text = Rscore.ToString();
            controlScript.Rscore = Rscore;
        }

        if (transform.position == bottomRCorner - new Vector3(-transform.lossyScale.x, -transform.lossyScale.y))
        {
            Lscore += 100;
            LscoreTxt.text = Lscore.ToString();
            controlScript.Lscore = Lscore;
        }
    }
}
