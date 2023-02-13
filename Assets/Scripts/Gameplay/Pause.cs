/*
This script defines the functionality of the pause menu.
*/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Thirdweb;
using System.Threading.Tasks;

/**
 * Pause.
 *
 * @author	Mahmoud Harmouch
 * @global
 */
public class Pause : MonoBehaviour
{
    // A reference to the pause menu.
    [field: SerializeField]
    GameObject PauseMen;

    // A reference to the name of the scene to be loaded when the player
    // exits to the main menu.
    [field: SerializeField]
    string menuLevelName = "MainMenu";

    // A variable used to display a loading message.
    [field: SerializeField]
    Text loadingText;

    /**
     * Pausing.
     *
     * @return	void
     */
    public void Pausing()
    {
        // Make the pause menu visible.
        Time.timeScale = 0f;
        PauseMen.SetActive(true);
    }

    /**
     * Resume.
     *
     * @return	void
     */
    public void Resume()
    {
        // Make the pause menu invisible.
        Time.timeScale = 1f;
        PauseMen.SetActive(false);
    }

    /**
     * Retry.
     *
     * @return	void
     */
    public void Retry()
    {
        Time.timeScale = 1f;
        // reloads the current scene.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /**
     * Exit.
     *
     * @return	void
     */
    public void Exit()
    {
        loadingText.text = "Please Wait ...";
        Time.timeScale = 1f;
        // Load the scene specified by menuLevelName.
        SceneManager.LoadScene(menuLevelName);
    }

    public async Task ClaimNFT()
    {
        Contract contract = ThirdWebController.instance.sdk.GetContract(
            PlayerPrefs.GetString("ContractAddress")
        );
        await contract.ERC721.Claim(1);
    }

    public async void Claim()
    {
        Button claimButton = GameObject.Find("Claim").GetComponent<Button>();
        claimButton.interactable = false;
        await ClaimNFT();
    }
}
