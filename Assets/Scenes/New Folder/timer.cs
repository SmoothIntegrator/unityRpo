using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class timer : MonoBehaviour
{

    public float starttime = 10f;
    public float currtime;
    public TMP_Text timertext;
    public GameObject panel;
    public GameObject panel2;
    public GameObject Winpanel1;
    public GameObject Winpanel2;
    public GameObject Tiepanel;

    private bool isrunning;
    public bool player1;
    private bool gameover;
    private int rounds;
    public int roundsmax = 3;
    public TextMeshProUGUI ScoreText1;
    public TextMeshProUGUI ScoreText2;
    public GameObject Player1;
    public GameObject Player2;
    //
    void Start()
    {
        currtime = starttime;
        isrunning = true;
        player1 = true;
        panel.SetActive(true);
        panel2.SetActive(false);
        Winpanel1.SetActive(false);
        Winpanel2.SetActive(false);
        Tiepanel.SetActive(false);
        gameover = false;
        rounds = 0;
        Player2.GetComponent<Collider2D>().enabled = true;
        Player1.GetComponent<Collider2D>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isrunning && !gameover)
        {
            currtime -= Time.deltaTime;
            if (!player1)
            {
                Player1.GetComponent<Collider2D>().enabled = true;
                Player2.GetComponent<Collider2D>().enabled = false;
            }
            else if (player1)
            {
                Player2.GetComponent<Collider2D>().enabled = true;
                Player1.GetComponent<Collider2D>().enabled = false;
            }
        }

        if (currtime <= 0 && isrunning && !gameover)
        {
            currtime = 0;
            isrunning = false;
            player1 = !player1;
            if (!player1)
            {
                Player1.GetComponent<Collider2D>().enabled = true;
                Player2.GetComponent<Collider2D>().enabled = false;
                panel.SetActive(false);
                panel2.SetActive(true);
            }
            else if (player1)
            {
                Player2.GetComponent<Collider2D>().enabled = true;
                Player1.GetComponent<Collider2D>().enabled = false;
                panel.SetActive(true);
                panel2.SetActive(false);
            }
            currtime = starttime;
            isrunning = true;
            rounds += 1;
            if (rounds >= roundsmax * 2)
            {
                gameover = true;
                isrunning = false;
                panel.SetActive(true);
                panel2.SetActive(true);
            }
        }


        timertext.text = currtime.ToString("0.00");
        if (gameover)
        {
            if (ScoreText1 == null || ScoreText2 == null)
            {
                TextMeshProUGUI[] texts = FindObjectsOfType<TextMeshProUGUI>(true);
                foreach (var text in texts)
                {
                    if (text.name == "ScoreText1")
                        ScoreText1 = text;
                    if (text.name == "ScoreText2")
                        ScoreText2 = text;
                }

            }
            int Score1 = int.Parse(ScoreText1.text);
            int Score2 = int.Parse(ScoreText2.text);
            if (Score1 > Score2)
            {
                Winpanel1.SetActive(true);
                timertext.gameObject.SetActive(false);
                ScoreText1.gameObject.SetActive(false);
                ScoreText2.gameObject.SetActive(false);
            }
            else if (Score1 < Score2)
            {
                Winpanel2.SetActive(true);
                timertext.gameObject.SetActive(false);
                ScoreText1.gameObject.SetActive(false);
                ScoreText2.gameObject.SetActive(false);
            }
            else if (Score1 == Score2)
            {
                Tiepanel.SetActive(true);
                timertext.gameObject.SetActive(false);
                ScoreText1.gameObject.SetActive(false);
                ScoreText2.gameObject.SetActive(false);
            }
            panel.SetActive(false);
            panel2.SetActive(false);
        }
    }
}
