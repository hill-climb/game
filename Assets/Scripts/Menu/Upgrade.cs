/*
The script contains variables and functions that allow the player to upgrade various aspects of their car.
*/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/**
 * Upgrade class.
 *
 * @author	Mahmoud Harmouch
 * @global
 */
public class Upgrade : MonoBehaviour
{
    // Declare variables to store the current level of each upgrade for the player's car,
    // and hide these variables from the Unity Editor's Inspector window.
    [HideInInspector]
    [field: SerializeField]
    int Engine;

    [field: SerializeField]
    int Fuel;

    [field: SerializeField]
    int Suspension;

    [field: SerializeField]
    int Speed;

    // Declare arrays to store the prices of each upgrade level.
    [Header("Upgrades price")]
    [field: SerializeField]
    int[] enginePrice;

    [field: SerializeField]
    int[] fuelPrice;

    [field: SerializeField]
    int[] suspensionPrice;

    [field: SerializeField]
    int[] speedPrice;

    // A variable that keeps track of the ID of the player's selected car.
    [field: SerializeField]
    int id;

    //  These are Text variables to display information about the upgrades and the player's coins.
    [Header("Informatin Texts")]
    [field: SerializeField]
    Text CoinsTXT;

    [field: SerializeField]
    Text TorqueTXT;

    [field: SerializeField]
    Text SuspensionTXT;

    [field: SerializeField]
    Text FuelTXT;

    [field: SerializeField]
    Text SpeedTXT;

    [field: SerializeField]
    Text priceTorqueTXT;

    [field: SerializeField]
    Text priceSuspensionTXT;

    [field: SerializeField]
    Text priceFuelTXT;

    [field: SerializeField]
    Text priceSpeedTXT;

    // The Loading screen GameObject.
    [Header("Show Window")]
    [field: SerializeField]
    GameObject Loading;

    // Store the sound effects for buying upgrades and when the player doesn't have enough coins.
    [Header("Sound Clips")]
    [field: SerializeField]
    AudioClip Buy;

    [field: SerializeField]
    AudioClip Caution;

    [field: SerializeField]
    AudioSource audioSource;

    // Check if the player has turned on control assistance.
    [Header("Control Assistance CheakBox")]
    [field: SerializeField]
    Toggle ControllAsist;

    [field: SerializeField]
    GameObject Shop;

    /**
     * This function is called when the script starts. It calls the LoadUpgrade() function.
     *
     * @return	void
     */
    void Start()
    {
        LoadUpgrade();
    }

    /**
     * This function is called when the script starts and when the player buys an upgrade. It
     * loads the player's current upgrade levels and coins, and updates the display texts accordingly.
     *
     * @access	public
     * @return	void
     */
    public void LoadUpgrade()
    {
        // Load the ID of the player's selected car.
        id = PlayerPrefs.GetInt("SelectedCar");

        // Load the current levels of each upgrade for the player's car.
        Engine = PlayerPrefs.GetInt("Coins" + id.ToString());
        Fuel = PlayerPrefs.GetInt("Fuel" + id.ToString());
        Suspension = PlayerPrefs.GetInt("Suspension" + id.ToString());
        Speed = PlayerPrefs.GetInt("Speed" + id.ToString());

        // Set the text display of each upgrade's level and the maximum level.
        TorqueTXT.text =
            "Level: "
            + PlayerPrefs.GetInt("Engine" + id.ToString()).ToString()
            + " / "
            + enginePrice.Length.ToString();
        SuspensionTXT.text =
            "Level: "
            + PlayerPrefs.GetInt("Suspension" + id.ToString()).ToString()
            + " / "
            + suspensionPrice.Length.ToString();
        FuelTXT.text =
            "Level: "
            + PlayerPrefs.GetInt("Fuel" + id.ToString()).ToString()
            + " / "
            + fuelPrice.Length.ToString();
        SpeedTXT.text =
            "Level: "
            + PlayerPrefs.GetInt("Speed" + id.ToString()).ToString()
            + " / "
            + speedPrice.Length.ToString();

        CoinsTXT.text = PlayerPrefs.GetInt("Coins").ToString();

        if (PlayerPrefs.GetInt("Engine" + id.ToString()) < enginePrice.Length)
            priceTorqueTXT.text =
                enginePrice[PlayerPrefs.GetInt("Engine" + id.ToString())].ToString() + " $";
        else
            priceTorqueTXT.text = "Completed";

        if (PlayerPrefs.GetInt("Speed" + id.ToString()) < speedPrice.Length)
            priceSpeedTXT.text =
                speedPrice[PlayerPrefs.GetInt("Speed" + id.ToString())].ToString() + " $";
        else
            priceSpeedTXT.text = "Completed";

        if (PlayerPrefs.GetInt("Fuel" + id.ToString()) < fuelPrice.Length)
            priceFuelTXT.text =
                fuelPrice[PlayerPrefs.GetInt("Fuel" + id.ToString())].ToString() + " $";
        else
            priceFuelTXT.text = "Completed";

        if (PlayerPrefs.GetInt("Suspension" + id.ToString()) < suspensionPrice.Length)
            priceSuspensionTXT.text =
                suspensionPrice[PlayerPrefs.GetInt("Suspension" + id.ToString())].ToString() + " $";
        else
            priceSuspensionTXT.text = "Completed";
    }

    /**
     * This function is called once per frame. It contains a conditional statement to delete all player
     * preferences when the H key is pressed in the Unity Editor.
     *
     * @return	void
     */
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.H))
            PlayerPrefs.DeleteAll();
#endif
    }

    /**
     * This functions is called when the player buys an upgrade for each aspect of their car.
     * It checks if the player has enough coins and if the upgrade is not already at the maximum level.
     * If both conditions are met, the player's coins will decrease by the upgrade's price and the upgrade.
     *
     * @access	public
     * @return	void
     */
    public void EngineUpgrade()
    {
        if (PlayerPrefs.GetInt("Engine" + id.ToString()) < enginePrice.Length)
        {
            if (
                PlayerPrefs.GetInt("Coins")
                >= enginePrice[PlayerPrefs.GetInt("Engine" + id.ToString())]
            )
            {
                audioSource.clip = Buy;
                audioSource.Play();
                PlayerPrefs.SetInt(
                    "Coins",
                    PlayerPrefs.GetInt("Coins")
                        - enginePrice[PlayerPrefs.GetInt("Engine" + id.ToString())]
                );
                PlayerPrefs.SetInt(
                    "Engine" + id.ToString(),
                    PlayerPrefs.GetInt("Engine" + id.ToString()) + 1
                );
                CoinsTXT.text = PlayerPrefs.GetInt("Coins").ToString();
                TorqueTXT.text =
                    "Level : "
                    + PlayerPrefs.GetInt("Engine" + id.ToString()).ToString()
                    + " / "
                    + enginePrice.Length.ToString();
                if (PlayerPrefs.GetInt("Engine" + id.ToString()) < enginePrice.Length)
                    priceTorqueTXT.text =
                        enginePrice[PlayerPrefs.GetInt("Engine" + id.ToString())].ToString() + " $";
                else
                    priceTorqueTXT.text = "Completed";
            }
            else
            {
                Shop.SetActive(true);

                audioSource.clip = Caution;
                audioSource.Play();
            }
        }
    }

    /**
     * This functions is called when the player buys an upgrade for each aspect of their car.
     * It checks if the player has enough coins and if the upgrade is not already at the maximum level.
     * If both conditions are met, the player's coins will decrease by the upgrade's price and the upgrade.
     *
     * @access	public
     * @return	void
     */
    public void SuspensionUpgrade()
    {
        if (PlayerPrefs.GetInt("Suspension" + id.ToString()) < suspensionPrice.Length)
        {
            if (
                PlayerPrefs.GetInt("Coins")
                >= suspensionPrice[PlayerPrefs.GetInt("Suspension" + id.ToString())]
            )
            {
                audioSource.clip = Buy;
                audioSource.Play();
                PlayerPrefs.SetInt(
                    "Coins",
                    PlayerPrefs.GetInt("Coins")
                        - suspensionPrice[PlayerPrefs.GetInt("Suspension" + id.ToString())]
                );
                PlayerPrefs.SetInt(
                    "Suspension" + id.ToString(),
                    PlayerPrefs.GetInt("Suspension" + id.ToString()) + 1
                );
                CoinsTXT.text = PlayerPrefs.GetInt("Coins").ToString();
                SuspensionTXT.text =
                    "Level : "
                    + PlayerPrefs.GetInt("Suspension" + id.ToString()).ToString()
                    + " / "
                    + suspensionPrice.Length.ToString();
                if (PlayerPrefs.GetInt("Suspension" + id.ToString()) < speedPrice.Length)
                    priceSuspensionTXT.text =
                        suspensionPrice[PlayerPrefs.GetInt("Suspension" + id.ToString())].ToString()
                        + " $";
                else
                    priceSuspensionTXT.text = "Completed";
            }
        }
    }

    /**
     * This functions is called when the player buys an upgrade for each aspect of their car.
     * It checks if the player has enough coins and if the upgrade is not already at the maximum level.
     * If both conditions are met, the player's coins will decrease by the upgrade's price and the upgrade.
     *
     * @access	public
     * @return	void
     */
    public void FuelUpgrade()
    {
        if (PlayerPrefs.GetInt("Fuel" + id.ToString()) < fuelPrice.Length)
        {
            if (
                PlayerPrefs.GetInt("Coins") >= fuelPrice[PlayerPrefs.GetInt("Fuel" + id.ToString())]
            )
            {
                audioSource.clip = Buy;
                audioSource.Play();
                PlayerPrefs.SetInt(
                    "Coins",
                    PlayerPrefs.GetInt("Coins")
                        - fuelPrice[PlayerPrefs.GetInt("Fuel" + id.ToString())]
                );
                PlayerPrefs.SetInt(
                    "Fuel" + id.ToString(),
                    PlayerPrefs.GetInt("Fuel" + id.ToString()) + 1
                );
                CoinsTXT.text = PlayerPrefs.GetInt("Coins").ToString();
                FuelTXT.text =
                    "Level : "
                    + PlayerPrefs.GetInt("Fuel" + id.ToString()).ToString()
                    + " / "
                    + fuelPrice.Length.ToString();
                if (PlayerPrefs.GetInt("Fuel" + id.ToString()) < fuelPrice.Length)
                    priceFuelTXT.text =
                        fuelPrice[PlayerPrefs.GetInt("Fuel" + id.ToString())].ToString() + " $";
                else
                    priceFuelTXT.text = "Completed";
            }
        }
    }

    /**
     * This functions is called when the player buys an upgrade for each aspect of their car.
     * It checks if the player has enough coins and if the upgrade is not already at the maximum level.
     * If both conditions are met, the player's coins will decrease by the upgrade's price and the upgrade.
     *
     * @access	public
     * @return	void
     */
    public void SpeedUpgrade()
    {
        if (PlayerPrefs.GetInt("Speed" + id.ToString()) < speedPrice.Length)
        {
            if (
                PlayerPrefs.GetInt("Coins")
                >= speedPrice[PlayerPrefs.GetInt("Speed" + id.ToString())]
            )
            {
                audioSource.clip = Buy;
                audioSource.Play();
                PlayerPrefs.SetInt(
                    "Coins",
                    PlayerPrefs.GetInt("Coins")
                        - speedPrice[PlayerPrefs.GetInt("Speed" + id.ToString())]
                );
                PlayerPrefs.SetInt(
                    "Speed" + id.ToString(),
                    PlayerPrefs.GetInt("Speed" + id.ToString()) + 1
                );
                CoinsTXT.text = PlayerPrefs.GetInt("Coins").ToString();
                SpeedTXT.text =
                    "Level : "
                    + PlayerPrefs.GetInt("Speed" + id.ToString()).ToString()
                    + " / "
                    + speedPrice.Length.ToString();
                if (PlayerPrefs.GetInt("Speed" + id.ToString()) < speedPrice.Length)
                    priceSpeedTXT.text =
                        speedPrice[PlayerPrefs.GetInt("Speed" + id.ToString())].ToString() + " $";
                else
                    priceSpeedTXT.text = "Completed";
            }
        }
    }

    public void StartGame()
    {
        Loading.SetActive(true);
        PlayerPrefs.SetInt("AllScoreTemp", PlayerPrefs.GetInt("Coins"));
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(
            "Level" + PlayerPrefs.GetInt("SelectedLevel").ToString()
        );
        gameObject.SetActive(false);
    }

    public void SetControll()
    {
        StartCoroutine(ControllAsistanceSave());
    }

    IEnumerator ControllAsistanceSave()
    {
        yield return new WaitForEndOfFrame();

        if (ControllAsist.isOn)
            PlayerPrefs.SetInt("Assistance", 3);
        else
            PlayerPrefs.SetInt("Assistance", 0);
    }
}
