/*
This script is designed to control various functions for the main menu of the game.
*/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/**
 * Define a new class named "MenuTools" that is a child of the MonoBehaviour class.
 * The class has the properties and methods necessary to perform the functions of the
 * main menu.
 *
 * @author	Mahmoud Harmouch
 * @global
 */
public class MenuTools : MonoBehaviour
{
    // Declare a public integer variable named "startScore" that represents the score
    // a player starts with when playing the game for the first time.
    [field: SerializeField]
    int startScore;

    // Declare a public variable named "CoinsTXT" of type "Text", which is used to represent
    // the text UI element that displays the player's current number of coins.
    [field: SerializeField]
    Text CoinsTXT,
        contractAddress;

    // Declare a variable which is used to represent the audio source for the menu music.
    [field: SerializeField]
    GameObject manuMusic;

    /**
     * The "Start" method is called once when the script is first executed.
     *
     * @return	void
     */
    void Start()
    {
        PlayerPrefs.SetString("ContractAddress", contractAddress.text);

        // Check if an object with the name "LevelMusic(Clone)" exists in the scene, and if it does, the object is destroyed.
        if (GameObject.Find("LevelMusic(Clone)"))
            Destroy(GameObject.Find("LevelMusic(Clone)"));

        // Check if an object with the name "MenuMusic(Clone)" does not exist in the scene, and if it does not,
        // the object is instantiated at the origin (Vector3.zero) with no rotation (Quaternion.identity).
        if (!GameObject.Find("MenuMusic(Clone)"))
            Instantiate(manuMusic, Vector3.zero, Quaternion.identity);

        if (PlayerPrefs.GetString("FirstRun") != "True")
        {
            PlayerPrefs.SetString("FirstRun", "True");
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + startScore);

            PlayerPrefs.SetInt("Resolution", 2);

            PlayerPrefs.SetFloat("EngineVolume", 0.74f);
            PlayerPrefs.SetFloat("MusicVolume", 1f);
            PlayerPrefs.SetInt("ShowDistance", 3);
            PlayerPrefs.SetInt("CoinAudio", 3);

            PlayerPrefs.SetInt("Car0", 3);
            PlayerPrefs.SetInt("Level0", 3);
        }

        if (PlayerPrefs.GetString("Update") != "True")
        {
            PlayerPrefs.SetString("FirstRun", "True");
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + startScore);
        }
        CoinsTXT.text = PlayerPrefs.GetInt("Coins").ToString();
    }

    /**
     * This method is called once per frame.
     *
     * @return	void
     */
    void Update()
    {
        // Check the H key is pressed.
        if (Input.GetKeyDown(KeyCode.H))
        {
            // Delete all the key-value pairs stored in the PlayerPrefs.
            PlayerPrefs.DeleteAll();
            // Set the text of the CoinsTXT text component to the value of
            // the "Coins" key in PlayerPrefs.
            CoinsTXT.text = PlayerPrefs.GetInt("Coins").ToString();
        }
        // Check the V key is pressed.
        if (Input.GetKeyDown(KeyCode.V))
        {
            // Retrieve the current value of the "Coins" key from PlayerPrefs,
            // adds the value of startScore to it, and sets the new value back to the "Coins" key.
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + startScore);
            // Set the text of the CoinsTXT text component to the value of the "Coins" key in PlayerPrefs.
            CoinsTXT.text = PlayerPrefs.GetInt("Coins").ToString();
        }
    }

    public void SetTrue(GameObject target)
    {
        target.SetActive(true);
    }

    public void SetFalse(GameObject target)
    {
        target.SetActive(false);
    }

    public void ToggleObject(GameObject target)
    {
        target.SetActive(!target.activeSelf);
    }

    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void LoadLevelAsync(string name)
    {
        SceneManager.LoadSceneAsync(name);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
