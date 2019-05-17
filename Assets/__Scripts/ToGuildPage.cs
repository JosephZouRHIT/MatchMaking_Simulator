using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ToGuildPage : MonoBehaviour {


    private Button btn;
	// Use this for initialization
	void Start () {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(LoadGuildPage);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LoadGuildPage()
    {
        DontDestroyOnLoad(GameObject.Find("Connection"));
        SceneManager.LoadScene("Guild");
    }
}
