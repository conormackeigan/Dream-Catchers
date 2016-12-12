//================================
// Alex
//  temp script for little into scene in new game
//================================
using UnityEngine;
using System.Collections;
using Rewired;

public class TempMessageInto : MonoBehaviour {

    public string nextscene;
    public bool credits;
    private Player player; // rewired player

	// Use this for initialization
	void Start ()
    {
        player = ReInput.players.GetPlayer(0);
	}
	
	// Update is called once per frame
	void Update () {

        if (player.GetButtonDown("Item"))
        {
            changeScene();
        }

	}

    public void changeScene()
    {
        Game_Manager.instance.ChangeScene(nextscene);
        if (credits)
        {
            Level_Manager.Instance.CurrentLevel = Level_Manager.Levels.MENU;
            Game_Manager.instance.currentGameState = Game_Manager.GameState.MENU;
        }
        else
        {
            Level_Manager.Instance.CurrentLevel = Level_Manager.Levels.TUTORIAL;
            PlayerPrefs.SetString("CurrentLevel", "Tutorial");
            Game_Manager.instance.currentGameState = Game_Manager.GameState.CINEMATIC;
            UI_Manager.instance.ShowMenu(GameObject.FindGameObjectWithTag("InGameUI").GetComponent<Menu>());
            GameObject.FindGameObjectWithTag("InGameUI").GetComponent<InGameStats>().updateFragments();
        }
    }



   
}
