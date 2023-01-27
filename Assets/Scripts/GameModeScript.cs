using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameModeScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public GameObject readyText;
    public Text seedBoard;
    public Text scoreBoard;
    public Text highscoreBoard;
    public List<GameObject> Ghosts;
    public AudioSource EatingCookiePlayer;
    public AudioSource MusicPlayer;
    public int seedScore = 0;
    public int maxScore = 0;
    float EatingCookieTimer = 0f;
    //bool ghostAudio = false;


    float ghostSpeed = .3f;

    void Start(){
		//reset everything and initiate ghosts
        Time.timeScale = 0;
        seedScore = 0;
        GhostInit();
    }

    private void Update(){{
			//set everything back to normal when start game sound finishes playing
            if (!MusicPlayer.isPlaying){
                Destroy(readyText);
                //ghostAudio = true;
                Time.timeScale = 1;
            }
			//pause the sound if some kind of timer hits zero
            if (EatingCookieTimer <= 0)
                EatingCookiePlayer.Pause();
            else
                EatingCookieTimer -= Time.deltaTime;
        }
        ScoreControler();
        //GhostAudioController();

		//quit game/ changes to main menu
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    void GhostInit(){
        foreach (GameObject ghost in Ghosts){
            //ghost.GetComponent<GhostScript>().speed = ghostSpeed;
        }
    }

    public void EatingCookieSoundPlay(){
        EatingCookieTimer = .95f - player.GetComponent<PlayerController>().speed;
        if (!EatingCookiePlayer.isPlaying)
            EatingCookiePlayer.Play();
    }

	//set collected seed amount, score, and highscore
    void ScoreControler(){
        seedBoard.text = seedScore + " / " + maxScore;
        scoreBoard.text = (seedScore * 10).ToString();
        highscoreBoard.text = PlayerPrefs.GetInt("HighScore").ToString();
        if (seedScore * 10 > PlayerPrefs.GetInt("HighScore"))
            PlayerPrefs.SetInt("HighScore", seedScore * 10);
    }

	
    /*void GhostAudioController(){
        if (ghostAudio){
            foreach (GameObject ghost in Ghosts)
                ghost.GetComponent<AudioSource>().volume = 0;

            GameObject closestGhost = Ghosts[0];
            foreach (GameObject ghost in Ghosts)
                if (Vector3.Distance(ghost.transform.position, player.transform.position) <
                    Vector3.Distance(closestGhost.transform.position, player.transform.position))
                    closestGhost = ghost;

            closestGhost.GetComponent<AudioSource>().volume = 1;

        }
    }*/
}
