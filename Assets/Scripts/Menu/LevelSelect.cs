/*
This script is used for displaying and managing in-game levels.
*/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/**
 * LevelSelect.
 *
 * @author	Mahmoud Harmouch
 * @global
 */
public class LevelSelect : MonoBehaviour
{
    // Declare a variable to store the current level id and hide it from the Inspector
    // window.
    [HideInInspector]
    public int id;

    // A UI text element to display the total number of coins the player has, placed under
    // the "Coins Text" header.
    [Header("Coins Text")]
    [field: SerializeField]
    Text CoinsTXT;

    // An array to store the prices of each level, and placed under the "Levels price" header.
    [Header("Levels price")]
    [field: SerializeField]
    int[] priceList;

    // A UI text elements for each level.
    [field: SerializeField]
    Text[] levelTexts;

    // Declare an array of GameObjects to represent the lock icons for each level, and place
    // it under the "List of the level locks" header.
    [Header("List of the level locks")]
    [field: SerializeField]
    GameObject[] locks;

    [Header("Show window")]
    [field: SerializeField]
    GameObject shop;

    // Declare GameObjects to represent the next and current menus, respectively, and place
    // them under the "Menus" header.
    [Header("Menus")]
    [field: SerializeField]
    GameObject nextMenu;

    [field: SerializeField]
    GameObject currentMenu;

    /**
     * A method to set the current selected level id.
     *
     * @access	public
     * @param	int	num	- The current selected level item id.
     * @return	void
     */
    public void SetLevelID(int num)
    {
        id = num;
    }

    /**
     * A method that initializes the state of the level locks on start. If the level is unlocked
     * (saved as 3 in player preferences), the lock icon is hidden. The price of each level is also
     * displayed in the corresponding level text element.
     *
     * @return	void
     */
    void Start()
    {
        // Hide unlocked level locks on start
        PlayerPrefs.SetInt("Level0", 3);
        for (int a = 0; a < locks.Length; a++)
        {
            if (PlayerPrefs.GetInt("Level" + a.ToString()) == 3)
                locks[a].SetActive(false);

            levelTexts[a].text = priceList[a].ToString();
        }
    }

    /**
     * A method method that allows the player to buy a level. It first checks
     * if the level is already unlocked. If it is, the player is taken to the
     * next menu. If it's not, it checks if the player has enough coins to buy the level.
     * If so, the coins are reduced by the level price and the level is unlocked.
     * The updated coin count is displayed in the CoinsTXT text element.
     *
     * @access	public
     * @param	int	num
     * @return	void
     */
    public void Buy(int num)
    {
        if (PlayerPrefs.GetInt("Level" + id.ToString()) == 3)
        {
            PlayerPrefs.SetInt("SelectedLevel", id);
            nextMenu.SetActive(true);
            currentMenu.SetActive(false);
        }
        else
        {
            if (PlayerPrefs.GetInt("Coins") >= priceList[num])
            {
                PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - priceList[num]);
                PlayerPrefs.SetInt("Level" + num.ToString(), 3);
                locks[num].SetActive(false);
                CoinsTXT.text = PlayerPrefs.GetInt("Coins").ToString();
            }
        }
    }

    /**
     * A method that allows the player to select a level. It first checks if the level is unlocked.
     * If it is, the player is taken to the next menu.
     *
     * @access	public
     * @return	void
     */
    public void Select()
    {
        if (PlayerPrefs.GetInt("Level" + id.ToString()) == 3)
        {
            PlayerPrefs.SetInt("SelectedLevel", id);
            nextMenu.SetActive(true);
            currentMenu.SetActive(false);
        }
    }
}
