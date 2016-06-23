using UnityEngine;
using System.Collections;

public class GameStateHandler : MonoBehaviour {

    public enum State {
        transitionToMain,
        main,
        transitionToPlay,
        beginGamePlay,
        gamePlaying,
        gameEnd,
        gameOver
    }

    public static State gameState;
    public string displayGameState;

    public delegate void TransitionToMain();
    public static event TransitionToMain transitionToMain;

    public delegate void TransitionToPlay();
    public static event TransitionToMain transitionToPlay;

    public delegate void BeginPlay();
    public static event BeginPlay beginPlay;

    public delegate void PlayReady();
    public static event PlayReady playReady;

    public delegate void GameOver();
    public static event GameOver gameOver;

    public static void BeginGame(){
        if (transitionToPlay != null) {
            transitionToPlay();
        }
        gameState = State.transitionToPlay;
    }
    public static void EndGame() {
        gameState = State.gameEnd;
    }
    public static void TransitionToPlayComplete() {
        if (beginPlay != null) {
            beginPlay();
        }
        gameState = State.beginGamePlay;
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        displayGameState = gameState.ToString();
        if (gameState == State.transitionToMain) {
            Debug.Log("call back from : " + gameState.ToString());
            if (transitionToMain != null)
            {
                
                transitionToMain();
            }
            gameState = State.main;
        }
        if (gameState == State.main)
        {
            
        }
        
        if (gameState == State.transitionToPlay)
        {
            
        }

        if (gameState == State.beginGamePlay) {
            Debug.Log("call back from : " + gameState.ToString());
            if (playReady != null) {
                playReady();
            }
            
            gameState = State.gamePlaying;
        }

        if (gameState == State.gamePlaying)
        {
          
        }

        if (gameState == State.gameEnd)
        {
            Debug.Log("call back from : " + gameState.ToString());
            if (gameOver != null)
            {
                gameOver();
            }
            gameState = State.gameOver;
        }
	}
}
