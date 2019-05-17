using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour {

    private float tgtTime;
    private float time;
    private float previousTime;
    private Text txt;
    private int minute = 0;
    private int second = 0;
	// Use this for initialization
	void Start () {
        time = 0f;
        //tgtTime = 0f;
        previousTime = 0f;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (tgtTime > 0f)
        {
            if(time - previousTime >= 1)
            {
                second++;
                minute = (int) (time / 60);
                second = second % 60;
                previousTime = time;
                string minuteStr = minute.ToString();
                string secondStr = second.ToString();
                if(minuteStr.Length < 2)
                {
                    minuteStr = "0" + minuteStr;
                }
                if(secondStr.Length < 2)
                {
                    secondStr = "0" + secondStr;
                }
                txt.text = minuteStr + ":" + secondStr;
            }
            time += Time.deltaTime;
            if(time >= tgtTime)
            {
                SetTime(0f);
                LoadNext();
            }
        }
	}

    public void SetTime(float target)
    {
        txt = GetComponent<Text>();
        txt.text = "00:00";
        tgtTime = target;
        time = 0f;
        previousTime = 0f;
        minute = 0;
        second = 0;
        
    }

    private void LoadNext()
    {
        DontDestroyOnLoad(GameObject.Find("Connection"));
        SceneManager.LoadScene("DungeonProgress");
    }
}
