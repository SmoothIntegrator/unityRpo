using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class timer : MonoBehaviour
{
    // Start is called before the first frame update
    public float starttime = 10f;
    public float currtime;
    public TMP_Text timertext;
    public GameObject panel;
    public GameObject panel2;

    private bool isrunning;
    public bool player1;
    private bool gameover;
    private int rounds;
    public int roundsmax = 3;
    void Start()
    {
        currtime = starttime;
        isrunning = true;
        player1 = true;
        panel.SetActive(true);
        panel2.SetActive(false);
        gameover = false;
        rounds = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isrunning && !gameover)
        {
            currtime -= Time.deltaTime;
        }

        if (currtime <= 0 && isrunning && !gameover)
        {
            currtime = 0;
            isrunning = false;
            player1 = !player1;
            if (!player1)
            {
                panel.SetActive(false);
                panel2.SetActive(true);
            }
            else if (player1)
            {
                panel.SetActive(true);
                panel2.SetActive(false);
            }
            currtime = starttime;
            isrunning = true;
            rounds += 1;
            if (rounds >= roundsmax*2)
            {
                gameover = true;
                isrunning = false;
                panel.SetActive(true);
                panel2.SetActive(true);
            }
        }

        
        timertext.text = currtime.ToString("0.00");
    }
}
