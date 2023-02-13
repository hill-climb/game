/*
This script is used for displaying and managing in-game items (e.g. cars or levels).
*/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/**
 * The enumeration is used to determine the type of item that the player is selecting.
 * @global
 */
public enum ItemType
{
    Level,
    Car
}

/**
 * A class that is used to control the selection of items in the game.
 *
 * @author	Mahmoud Harmouch
 * @global
 */
public class ItemSelect : MonoBehaviour
{
    // A variable that determines the type of item that the player is selecting.
    [field: SerializeField]
    ItemType itemType;

    // `itemIcons`: This is an array of Sprite objects that represent the icons for each item.
    // `currentItemImage`: This is a reference to the Image component for the current item.
    // `prevItemImage`: This is a reference to the Image component for the previous item.
    // `nextItemImage`: This is a reference to the Image component for the next item.
    // Group these variables under the header "Icons" in the Inspector window.
    [Header("Icons")]
    [field: SerializeField]
    Sprite[] itemIcons;

    [field: SerializeField]
    Image currentItemImage;

    [field: SerializeField]
    Image prevItemImage;

    [field: SerializeField]
    Image nextItemImage;

    // `currentAnimator`: This is a reference to the Animator component that is used to animate the current
    // item when it is selected. Add this variable under the header "Current Item Animation"
    // in the Inspector window.
    [Header("Current Item Animation")]
    [field: SerializeField]
    Animator currentAnimator;

    // `audioSource`: This is a reference to the AudioSource component that is used to play sounds.
    // `okClip`: This is a reference to the sound clip that is played when the player has enough money to
    // buy an item.
    // `errorClip`: This is a reference to the sound clip that is played when the player doesn't have enough
    // money to buy
    // an item.
    // Group these variables under the header "Sounds" in the Inspector window.
    [Header("Sounds")]
    [field: SerializeField]
    AudioSource audioSource;

    [field: SerializeField]
    AudioClip okClip;

    [field: SerializeField]
    AudioClip errorClip;

    // `CurentValue`: This is a reference to the Text component that displays the current item's price.
    // `coinsTXT`: This is a reference to the Text component that displays the player's total coins.
    // Group variables under the header "Texts" in the Inspector window.
    [Header("Texts")]
    [field: SerializeField]
    Text CurentValue;

    [field: SerializeField]
    Text coinsTXT;

    // Group variables under the header "Windows" in the Inspector window.
    [Header("Windows")]
    [field: SerializeField]
    GameObject shopOffer;

    [field: SerializeField]
    GameObject lockIcon;

    [field: SerializeField]
    GameObject nextMen;

    // The selected item id, which is set either to the last selected car or level
    // depending on the item type.
    [field: SerializeField]
    int id;

    [field: SerializeField]
    bool canAnimate;

    [field: SerializeField]
    bool animationState;

    // List of the items price
    [field: SerializeField]
    int[] itemsPrice;

    /**
     * In this function, the total number of coins is displayed and the last selected item id
     * is read from player preferences. The selected item's image is then set, and the previous
     * and next items' images are updated as well. The lock icon is activated if the item is locked,
     * and the item price is displayed.
     *
     * @return	void
     */
    void Start()
    {
        // Assign the value of the player's coins that is retrieved from the PlayerPrefs data
        // storage to the corresponding top left text component on the GameObject in the scene.
        coinsTXT.text = PlayerPrefs.GetInt("Coins").ToString();

        // Check if the selected item is a car.
        if (itemType == ItemType.Car)
            // Retrieve the last selected car ID from PlayerPrefs and assigns it to id.
            id = PlayerPrefs.GetInt("CarID");
        // Check if the selected item is a level.
        if (itemType == ItemType.Level)
            // Retrieve the last selected level ID from PlayerPrefs and assigns it to id.
            id = PlayerPrefs.GetInt("LevelID");

        // Assigns a sprite to the sprite renderer component of the current selected item.
        if (currentItemImage != null)
        {
            currentItemImage.sprite = itemIcons[id];
        }

        // Select the last selected item by default.
        if (id != 0)
        {
            // Call PrevCar() first then NextCar() to select the last selected item.
            PrevCar();
            NextCar();
        }
        else
        {
            // Call NextCar() first then PrevCar() to select the first item in the list.
            NextCar();
            PrevCar();
        }

        // A flag that determines if an animation can be played.
        canAnimate = true;

        // Check if the current selected item is a car.
        if (itemType == ItemType.Car)
        {
            // Retrieve the lock status of the current selected car from PlayerPrefs, and then
            // check if it is not locked.
            if (PlayerPrefs.GetInt("Car" + id.ToString()) != 3)
                // Activate the lockIcon component to display a lock icon on the UI.
                lockIcon.SetActive(true);
            else
                // Deactivate the lockIcon component to display a lock icon on the UI.
                lockIcon.SetActive(false);
        }
        // Check if the current selected item is a level.
        if (itemType == ItemType.Level)
        {
            // Retrieve the lock status of the current selected level from PlayerPrefs, and then
            // check if it is not locked.
            if (PlayerPrefs.GetInt("Level" + id.ToString()) != 3)
                // Activate the lockIcon component to display a lock icon on the UI.
                lockIcon.SetActive(true);
            else
                // Deactivate the lockIcon component to display a lock icon on the UI.
                lockIcon.SetActive(false);
        }

        // Update items prices.
        CurentValue.text = itemsPrice[id].ToString();
    }

    /**
     * This function is used to select the next car. It update the id, play
     * the appropriate sound clip, and update the images and lock icon accordingly.
     *
     * @return	void
     */
    public void NextCar()
    {
        // Check if the current item is not equal to the last item in the itemIcons array.
        if (id < itemIcons.Length - 1)
        {
            id++;
            // Check if an animation is allowed to play.
            if (canAnimate)
                // Play the animation, if allowed.
                PlayAnim();
            // This line sets the audio clip that will be played to okClip.
            audioSource.clip = okClip;
            // Play the audio clip.
            audioSource.Play();
        }
        if (currentItemImage != null)
        {
            // Set the sprite of the current item to the sprite at the current index in the itemIcons array.
            currentItemImage.sprite = itemIcons[id];
        }

        // Check if the current item is not the last item in the itemIcons array.
        if (id < itemIcons.Length - 1)
        {
            // Set the color of the previous item image to fully opaque (white).
            prevItemImage.color = new Color(1f, 1f, 1f, 1f);
            // Set the sprite of the next item to the sprite at the next index in the itemIcons array.
            nextItemImage.sprite = itemIcons[id + 1];
            // Set the sprite of the next item to the sprite at the previous  index in the itemIcons array.
            prevItemImage.sprite = itemIcons[id - 1];
        }
        else
        {
            nextItemImage.sprite = null;
            // Set the color of the next item image to fully transparent (black).
            nextItemImage.color = new Color(0, 0, 0, 0);
            prevItemImage.sprite = itemIcons[id - 1];
        }

        if (itemType == ItemType.Car)
            PlayerPrefs.SetInt("CarID", id);
        if (itemType == ItemType.Level)
            PlayerPrefs.SetInt("LevelID", id);

        if (itemType == ItemType.Car)
        {
            if (PlayerPrefs.GetInt("Car" + id.ToString()) != 3)
                lockIcon.SetActive(true);
            else
                lockIcon.SetActive(false);
        }
        if (itemType == ItemType.Level)
        {
            if (PlayerPrefs.GetInt("Level" + id.ToString()) != 3)
                lockIcon.SetActive(true);
            else
                lockIcon.SetActive(false);
        }

        CurentValue.text = itemsPrice[id].ToString();
    }

    /**
     * This function is used to select the previous car. It update the id, play
     * the appropriate sound clip, and update the images and lock icon accordingly.
     *
     * @return	void
     */
    public void PrevCar()
    {
        if (id > 0)
        {
            --id;
            if (canAnimate)
                PlayAnim();

            audioSource.clip = okClip;
            audioSource.Play();
        }
        if (currentItemImage != null)
        {
            currentItemImage.sprite = itemIcons[id];
        }
        if (id > 0)
        {
            nextItemImage.color = new Color(1f, 1f, 1f, 1f);
            prevItemImage.sprite = itemIcons[id - 1];
            nextItemImage.sprite = itemIcons[id + 1];
        }
        else
        {
            prevItemImage.sprite = null;
            prevItemImage.color = new Color(0, 0, 0, 0);
            nextItemImage.sprite = itemIcons[id + 1];
        }

        if (itemType == ItemType.Level)
            PlayerPrefs.SetInt("LevelID", id);
        if (itemType == ItemType.Car)
            PlayerPrefs.SetInt("CarID", id);

        if (itemType == ItemType.Level)
        {
            if (PlayerPrefs.GetInt("Level" + id.ToString()) != 3)
                lockIcon.SetActive(true);
            else
                lockIcon.SetActive(false);
        }

        if (itemType == ItemType.Car)
        {
            if (PlayerPrefs.GetInt("Car" + id.ToString()) != 3)
                lockIcon.SetActive(true);
            else
                lockIcon.SetActive(false);
        }

        CurentValue.text = itemsPrice[id].ToString();
    }

    /**
     * This  function is used to animate the current item when it is selected.
     * The animation state is controlled by the currentAnimator animator.
     *
     * @return	void
     */
    void PlayAnim()
    {
        animationState = !animationState;
        if (animationState)
            currentAnimator.CrossFade("Next", .003f);
        else
            currentAnimator.CrossFade("Prev", .003f);
    }

    // Select current item and go to the next menu
    public void SelectCurrent()
    {
        if (itemType == ItemType.Level)
        {
            if (PlayerPrefs.GetInt("Level" + id.ToString()) == 3)
            {
                gameObject.SetActive(false);
                nextMen.SetActive(true);
                PlayerPrefs.SetInt("SelectedLevel", id);
            }
            else
            {
                audioSource.clip = errorClip;
                audioSource.Play();
            }
        }

        if (itemType == ItemType.Car)
        {
            if (PlayerPrefs.GetInt("Car" + id.ToString()) == 3)
            {
                gameObject.SetActive(false);
                nextMen.SetActive(true);
                PlayerPrefs.SetInt("SelectedCar", id);
            }
            else
            {
                audioSource.clip = errorClip;
                audioSource.Play();
            }
        }
    }

    // Public function used in current selected button (ui button )
    public void Buy()
    {
        if (itemType == ItemType.Level)
        {
            if (PlayerPrefs.GetInt("Level" + id.ToString()) != 3)
            {
                if (PlayerPrefs.GetInt("Coins") >= itemsPrice[id])
                {
                    PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - itemsPrice[id]);
                    PlayerPrefs.SetInt("Level" + id.ToString(), 3);
                    lockIcon.SetActive(false);
                    coinsTXT.text = PlayerPrefs.GetInt("Coins").ToString();
                }
            }
        }

        if (itemType == ItemType.Car)
        {
            if (PlayerPrefs.GetInt("Car" + id.ToString()) != 3)
            {
                if (PlayerPrefs.GetInt("Coins") >= itemsPrice[id])
                {
                    PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - itemsPrice[id]);
                    PlayerPrefs.SetInt("Car" + id.ToString(), 3);
                    lockIcon.SetActive(false);
                    coinsTXT.text = PlayerPrefs.GetInt("Coins").ToString();
                }
            }
        }
    }
}
