using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GetInGame : MonoBehaviour {


    private Button btn;
	// Use this for initialization
	void Start () {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(NextScene);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void NextScene()
    {
        DontDestroyOnLoad(GameObject.Find("Connection"));
        SceneManager.LoadScene("DisplayDungeon");
    }
}
