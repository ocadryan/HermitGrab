using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{

    public float currentTime = 0f;
    public float startingTime = 60f;
    public PlayerController pc;
    public GameObject replayButton;


    public Text countdownText;
    public Text WinnerText;
    // Start is called before the first frame update
    void Start()
    {
        currentTime = startingTime;
     
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= 1 * Time.deltaTime;
        countdownText.text = currentTime.ToString("0") + " s";

        if (currentTime <= 30)
        {
            countdownText.color = Color.yellow;

        }

        if (currentTime <= 15)
        {
            countdownText.color = Color.red;

        }

        if (currentTime <= 0)
        {
            currentTime = 0;
            countdownText.text = "Game Over!";

            if (pc.hasShell == true)
            {
                replayButton.transform.GetChild(2).gameObject.SetActive(true);
                WinnerText.text = "Player 1 Wins!";
            } else
            {
                replayButton.transform.GetChild(2).gameObject.SetActive(true);
                WinnerText.text = "Player 2 Wins!";
            }
            
            pc.enabled = false;
            replayButton.transform.GetChild(1).gameObject.SetActive(true);
            
            

        }
    }
}
