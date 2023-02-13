/*
This script allows the user to adjust various settings in a game such as audio and video settings.
*/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/**
 * SettingsMenu class.
 *
 * @author	Mahmoud Harmouch
 * @global
 */
public class SettingsMenu : MonoBehaviour
{
    // Declare two fields for two sliders that control the engine volume and music volume in the game.
    [field: SerializeField]
    Slider engineVolume,
        musicVolume;

    // Declare two fields for two toggles that control the show distance and coin audio options in the game.
    [field: SerializeField]
    Toggle showDistance,
        coinAudio;

    // TODO: Implement
    [field: SerializeField]
    InputField cheatBox;

    // Declare two fields for two text elements that display the current resolution and current quality
    // settings in the game.
    [field: SerializeField]
    Text currentResolution,
        currentQuality;

    // TODO: Implement
    [field: SerializeField]
    bool activateCheats;

    /**
     * The start method is called when the component is first enabled.
     * @return void
     */
    void Start()
    {
        // Check if the ShowDistance checkbox is selected.
        if (PlayerPrefs.GetInt("ShowDistance") == 3)
            // Show the distance slider.
            showDistance.isOn = true;
        else
            // Hide the distance slider.
            showDistance.isOn = false;

        // Check if the CoinAudio checkbox is selected.
        if (PlayerPrefs.GetInt("CoinAudio") == 3)
            // Turn on coin audio collider.
            coinAudio.isOn = true;
        else
            // Turn off coin audio collider.
            coinAudio.isOn = false;

        if (PlayerPrefs.GetInt("Resolution") == 0)
            currentResolution.text = "500";
        if (PlayerPrefs.GetInt("Resolution") == 1)
            currentResolution.text = "720P";
        if (PlayerPrefs.GetInt("Resolution") == 2)
            currentResolution.text = "1080P";

        if (PlayerPrefs.GetInt("Quality") == 0)
            currentQuality.text = "Low";
        if (PlayerPrefs.GetInt("Quality") == 3)
            currentQuality.text = "Medium";
        if (PlayerPrefs.GetInt("Quality") == 5)
            currentQuality.text = "High";

        engineVolume.value = PlayerPrefs.GetFloat("EngineVolume");
        musicVolume.value = PlayerPrefs.GetFloat("MusicVolume");
    }

    public void SetEngineVolume()
    {
        PlayerPrefs.SetFloat("EngineVolume", engineVolume.value);
    }

    public void SetMusicVolume()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolume.value);
    }

    public void SetShowDistance()
    {
        StartCoroutine(saveDistance());
    }

    IEnumerator saveDistance()
    {
        yield return new WaitForEndOfFrame();

        if (showDistance.isOn)
            PlayerPrefs.SetInt("ShowDistance", 3);
        else
            PlayerPrefs.SetInt("ShowDistance", 0);
    }

    public void SetCoinAudio()
    {
        StartCoroutine(saveCoinAudio());
    }

    IEnumerator saveCoinAudio()
    {
        yield return new WaitForEndOfFrame();

        if (coinAudio.isOn)
            PlayerPrefs.SetInt("CoinAudio", 3);
        else
            PlayerPrefs.SetInt("CoinAudio", 0);
    }

    public void SetResolution(int val)
    {
        PlayerPrefs.SetInt("Resolution", val);

        if (PlayerPrefs.GetInt("Resolution") == 0)
            currentResolution.text = "500";
        if (PlayerPrefs.GetInt("Resolution") == 1)
            currentResolution.text = "720P";
        if (PlayerPrefs.GetInt("Resolution") == 2)
            currentResolution.text = "1080P";
    }

    public void SetQualityLevel(int val)
    {
        PlayerPrefs.SetInt("Quality", val);

        if (PlayerPrefs.GetInt("Quality") == 0)
            currentQuality.text = "Low";
        if (PlayerPrefs.GetInt("Quality") == 3)
            currentQuality.text = "Medium";
        if (PlayerPrefs.GetInt("Quality") == 5)
            currentQuality.text = "High";

        QualitySettings.SetQualityLevel(val);
    }

    public void EnterCheat()
    {
        // TODO: Implement
    }
}
