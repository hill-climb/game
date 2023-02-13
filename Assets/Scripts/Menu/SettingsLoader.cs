/*
 The script handles loading of audio and game settings when the game is started or reloaded.
*/
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/**
 * Declares the SettingsLoader class.
 *
 * @author	Mahmoud Harmouch
 * @global
 */
public class SettingsLoader : MonoBehaviour
{
    // Declare a GameObject variable that is used to store a reference to the distance
    // slider game object in the scene.
    [field: SerializeField]
    GameObject distanceSlider;

    // Declare two variables, musicVolume and coinAudio, used to store references to the
    // audio sources for the music and coin sound effects in the game.
    [field: SerializeField]
    AudioSource musicVolume,
        coinAudio;

    // Declare a variable that is used to store a reference to the music prefab, which
    // will be instantiated in the game.
    [field: SerializeField]
    GameObject musicPrefab;

    /**
     * Declare the Start method, which runs once when the script component is first enabled.
     * This method returns an IEnumerator type and is used to perform actions in a specific
     * sequence over time, in this case, it is used to wait for the end of the frame.
     *
     * @return	mixed
     */
    IEnumerator Start()
    {
        // Check if the value of the "ShowDistance" key in the PlayerPrefs is true (3 means true).
        if (PlayerPrefs.GetInt("ShowDistance") == 3)
            // Set the distanceSlider game object to active, which means it will be displayed in the scene.
            distanceSlider.SetActive(true);
        else
            // Set the distanceSlider game object to inactive, which means it will not be displayed in the scene.
            distanceSlider.SetActive(false);

        // Check if the value of the "CoinAudio" key in the PlayerPrefs is true (3 means true).
        if (PlayerPrefs.GetInt("CoinAudio") == 3)
            // Set the volume of the coinAudio to 1f, which means that it will be played at full volume.
            coinAudio.volume = 1f;
        else
            coinAudio.volume = 0;

        if (PlayerPrefs.GetInt("Loaded") != 3)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            PlayerPrefs.SetInt("Loaded", 3);
        }
        else
            PlayerPrefs.SetInt("Loaded", 7);

        yield return new WaitForEndOfFrame();

        GameObject.FindObjectOfType<CarController>().EngineSoundS.volume = PlayerPrefs.GetFloat(
            "EngineVolume"
        );

        if (!GameObject.Find("LevelMusic(Clone)"))
        {
            GameObject m = (GameObject)Instantiate(musicPrefab, Vector3.zero, Quaternion.identity);
            GameObject.DontDestroyOnLoad(m);
        }

        // Find the game object named "LevelMusic(Clone)" in the scene and retrieve its AudioSource component.
        // Then set the volume of this AudioSource to the value stored in the "MusicVolume" key of the PlayerPrefs.
        // The value of the volume is expected to be a float.
        GameObject.Find("LevelMusic(Clone)").GetComponent<AudioSource>().volume =
            PlayerPrefs.GetFloat("MusicVolume");
    }
}
