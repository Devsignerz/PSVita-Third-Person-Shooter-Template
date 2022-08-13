using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MMBtns : MonoBehaviour {

	//unloads mainmenu scene and loads the first level
	public void newgame(){
		SceneManager.UnloadSceneAsync(0);
		SceneManager.LoadSceneAsync(1);
	}

	//open another menu to choose the level or save
	public void loadgame(){
		
	}

	//game setings go here
	public void options(){
		
	}
}
