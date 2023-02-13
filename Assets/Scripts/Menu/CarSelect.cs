/*
The script is used to determine the current selected Car in the selection panel.
*/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/**
 * This class declares the CarSelect component that can be attached to a Car `GameObject` in the scene.
 *
 * @author	Mahmoud Harmouch
 * @global
 */
public class CarSelect : MonoBehaviour
{
    // Declare the car current id and hide it from the Inspector and cannot be
    // modified directly through the editor.
    [HideInInspector]
    public int id;

    // Declare the total number of coins and create a header labeled
    // "Coins Text" in the Inspector.
    [Header("Coins Text")]
    [field: SerializeField]
    Text CoinsTXT;

    // Declare the prices of the cars as a list of integers and create
    // a header labeled "Cars price" in the Inspector.
    [Header("Cars price")]
    [field: SerializeField]
    int[] priceList;

    [field: SerializeField]
    Text[] levelTexts;

    // Declare an array of GameObjects named locks that represent the cars'
    // lock status and create a header labeled "Cars lock" in the Inspector.
    [Header("Cars lock")]
    [field: SerializeField]
    GameObject[] locks;

    // Declare two GameObject variables that will store references to the
    // menus in the game and create a header labeled "Menus" in the Inspector.
    [Header("Menus")]
    [field: SerializeField]
    GameObject nextMenu,
        currentMenu;

    /**
     * This method takes an integer parameter num and sets the value of the car current id
     * variable to num.
     *
     * @access	public
     * @param	int	num	- A parameter which represents the car's index number in the arrays
     * priceList and locks.
     * @return	void
     */
    public void SetCarID(int num)
    {
        id = num;
    }

    /**
     * This method will be called once when the component is initialized. In this method,
     * a for loop iterates through the locks array and sets the active status of each
     * lock GameObject based on the value of the "Car" + index stored in the PlayerPrefs.
     *
     * @return	void
     */
    void Start()
    {
        for (int a = 0; a < locks.Length; a++)
        {
            if (PlayerPrefs.GetInt("Car" + a.ToString()) == 3) // 3 => true | 0 => false
                locks[a].SetActive(false);
        }
    }

    /**
     * This declares a method that takes an integer parameter num and allows the player to
     * buy a car if they have enough coins. If the car has already been purchased,
     * the player can select it. The method updates the value of the "Car" + num key in the
     * PlayerPrefs and sets the active status of the lock GameObject to false if the car has
     * been purchased.
     *
     * @access	public
     * @param	int	num	- A parameter which represents the car's index number in the arrays
     * priceList and locks.
     * @return	void
     */
    public void Buy(int num)
    {
        // Check if the car represented by num has already been bought or not.
        // 3 means bought, and 0 otherwise.
        if (PlayerPrefs.GetInt("Car" + id.ToString()) == 3)
        {
            // Save the selected car's index number in the player preferences
            // with the key "SelectedCar".
            PlayerPrefs.SetInt("SelectedCar", num);
            // Activate the next menu game object(The upgrade menu).
            nextMenu.SetActive(true);
            // Deactivate the current menu game object (The car select menu).
            currentMenu.SetActive(false);
        }
        else
        {
            // Checking if the player has enough coins to buy the car represented by num.
            if (PlayerPrefs.GetInt("Coins") >= priceList[num])
            {
                // Subtract the price of the car from the total available coins and save
                // the updated value in the player preferences with the key "Coins".
                PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - priceList[num]);
                // Mark that the car has been bought.
                PlayerPrefs.SetInt("Car" + num.ToString(), 3);
                // Deactivate the lock object represented by num in the locks array.
                locks[num].SetActive(false);
                // updating the text of the CoinsTXT Text component to show the updated
                // number of coins after buying the car.
                CoinsTXT.text = PlayerPrefs.GetInt("Coins").ToString();
            }
        }
    }

    /**
     * This declares a method that allows the player to select a car that has already been
     * purchased. The method sets the value of the "SelectedCar
     *
     * @access	public
     * @return	void
     */
    public void Select()
    {
        // Check if the selected car has already been bought or not.
        // 3 means bought, and 0 otherwise.
        if (PlayerPrefs.GetInt("Car" + id.ToString()) == 3)
        {
            // Save the selected car's index number in the player preferences
            // with the key "SelectedCar".
            PlayerPrefs.SetInt("SelectedCar", id);
            // Activate the next menu game object(The upgrade menu).
            nextMenu.SetActive(true);
            // Deactivate the current menu game object (The car select menu).
            currentMenu.SetActive(false);
        }
    }
}
