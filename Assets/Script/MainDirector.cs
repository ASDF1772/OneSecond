using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainDirector : MonoBehaviour {
    
	void Start () {
		
	}
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.X))
        {
            SceneManager.LoadScene("GameScene");
        }
	}
}
