using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackToPlayerInfo : MonoBehaviour {

    private Button btn;
	// Use this for initialization
	void Start () {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(LoadPrevious);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LoadPrevious()
    {
        DontDestroyOnLoad(GameObject.Find("Connection"));
        SceneManager.LoadScene("PlayerInfo");
    }
}
