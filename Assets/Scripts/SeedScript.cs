using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedScript : MonoBehaviour{
    public GameObject gameMode;

    private void Start(){
        ++gameMode.GetComponent<GameModeScript>().maxScore;
    }

    private void OnTriggerEnter(Collider other){
        if (other.tag == "Player"){
            gameMode.GetComponent<GameModeScript>().EatingCookieSoundPlay();
            ++gameMode.GetComponent<GameModeScript>().seedScore;
            Destroy(gameObject);
        }
    }
}
