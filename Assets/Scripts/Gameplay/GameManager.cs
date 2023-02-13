/*
This script is responsible for managing various aspects of a game like Coins, Distance, Records and Fuel.
*/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/**
 * This class is responsible for coins, distance, fuel, etc.
 *
 * @author	Mahmoud Harmouch
 * @global
 */
public class GameManager : MonoBehaviour
{
    // Variables for displaying different information like coins, distance, fuel, etc
    // on the screen through Text components and group them under the "Informations" header.
    [Header("Informations")]
    [field: SerializeField]
    Text DistanceTXT;

    [field: SerializeField]
    Text RecordTXT;

    [field: SerializeField]
    Text CoinTXT;

    [field: SerializeField]
    Text FuelTXT;

    [field: SerializeField]
    Text LastRecord;

    // A Slider component for showing the amount of fuel for a given car.
    [Header("Sliders")]
    [field: SerializeField]
    Slider FuelSlider;

    // A Slider component for showing the distance traveled by the car.
    [field: SerializeField]
    Slider DitanceSlider;

    // A Transform component to read the car position as the distance.
    [field: SerializeField]
    Transform player;

    // A variable to check if the game has started or not.
    [field: SerializeField]
    bool Started;

    // The initial amount of fuel.
    [Header("Fuel")]
    [field: SerializeField]
    float TotalFuel = 100f;

    // Fuel reducing time step (delay in coroutine), A fuel reducing time step.
    public float FuelTime = .17f;

    // The amount of fuel reduced by time.
    [field: SerializeField]
    float FuelVal = .3f;

    // An internal variable to store the total number of coins.
    [field: SerializeField]
    int Coins;

    // A component for playing the sound effect when a coin is collected.
    [Header("Coins And Awards")]
    [field: SerializeField]
    AudioSource coinSound;

    // Awarded coin box (award based on distance)
    [field: SerializeField]
    GameObject coinAwardedBox;

    // A text displayed inside the animation.
    [field: SerializeField]
    Text awardedText;

    // An animator component are used to display the award for collecting a
    // certain number of coins.
    [field: SerializeField]
    Animator awardAnimator;

    // Variables for managing the end of the level.
    [Header("Complete Level")]
    [field: SerializeField]
    GameObject youWinMenu;

    [field: SerializeField]
    GameObject youLostMenu;

    // A variable that holds the length of the level
    [field: SerializeField]
    float levelLength = 3700f;

    // A variable holds the number of coins awarded to the player when they finish the level.
    [field: SerializeField]
    int winnerAwardedCoins = 30000;

    // A temporary variable to store the distance traveled by the car.
    [field: SerializeField]
    float DistanceTemp;

    // Booleans for car coins.
    [field: SerializeField]
    bool c500,
        c1000,
        c1500,
        c2000,
        c2500,
        c3000,
        c3500,
        c4000;

    // Boolean for lost state.
    [HideInInspector]
    public bool isDead;

    // Boolean for fuel finished state.
    [HideInInspector]
    [field: SerializeField]
    bool fuelFinished;

    /**
     * This coroutine defines the behavior of a Raycast that is used to determine
     * if a car object is on the ground or not.
     *
     * @return	mixed
     */
    IEnumerator Start()
    {
        // Retrieve the total number of coins from the PlayerPrefs class.
        Coins = PlayerPrefs.GetInt("Coins");
        CoinTXT.text = Coins.ToString();

        // Wait for the player to spawn.
        yield return new WaitForEndOfFrame();
        // Set the player variable to the player game object's Transform component.
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Started = true; // The game is now started. you can run your codes on update function

        // Read the saved records for different distances from the PlayerPrefs class and
        // set the corresponding flags for these distances.
        if (PlayerPrefs.GetInt("c500" + PlayerPrefs.GetInt("SelectedLevel").ToString()) == 3)
        {
            c500 = true;
            LastRecord.text = "Record:500";
        }
        if (PlayerPrefs.GetInt("c1000" + PlayerPrefs.GetInt("SelectedLevel").ToString()) == 3)
        {
            c1000 = true;
            LastRecord.text = "Record:1000";
        }
        if (PlayerPrefs.GetInt("c1500" + PlayerPrefs.GetInt("SelectedLevel").ToString()) == 3)
        {
            c1500 = true;
            LastRecord.text = "Record:1500";
        }
        if (PlayerPrefs.GetInt("c2000" + PlayerPrefs.GetInt("SelectedLevel").ToString()) == 3)
        {
            c2000 = true;
            LastRecord.text = "Record:2000";
        }
        if (PlayerPrefs.GetInt("c2500" + PlayerPrefs.GetInt("SelectedLevel").ToString()) == 3)
        {
            c2500 = true;
            LastRecord.text = "Record:2500";
        }
        if (PlayerPrefs.GetInt("c3000" + PlayerPrefs.GetInt("SelectedLevel").ToString()) == 3)
        {
            c3000 = true;
            LastRecord.text = "Record:3000";
        }
        if (PlayerPrefs.GetInt("c3500" + PlayerPrefs.GetInt("SelectedLevel").ToString()) == 3)
        {
            c3500 = true;
            LastRecord.text = "Record:3500";
        }
        if (PlayerPrefs.GetInt("c4000" + PlayerPrefs.GetInt("SelectedLevel").ToString()) == 3)
        {
            c4000 = true;
            LastRecord.text = "Record:4000";
        }

        while (true)
        {
            yield return new WaitForSeconds(FuelTime);
            // Decrease the fuel over time.
            TotalFuel -= FuelVal;
            // Update the slider of the distance and fuel on the screen.
            FuelSlider.value = TotalFuel;
            if (TotalFuel >= 0)
                FuelTXT.text = Mathf.Floor(TotalFuel).ToString();
            if (TotalFuel < 0)
            {
                fuelFinished = true;
                StartFuelFinish();
            }
        }
    }

    /**
     * Update.
     *
     * @return	void
     */
    void Update()
    {
        if (Started)
        {
            CoinDistance();
            if (player.position.x >= DistanceTemp)
            {
                DistanceTXT.text = Mathf.Floor(player.position.x).ToString();
                DistanceTemp = player.position.x;
                DitanceSlider.value = player.position.x;
            }
        }
    }

    /**
     * AddCoin.
     *
     * @access	public
     * @param	int	value
     * @return	void
     */
    public void AddCoin(int value)
    { //add Coin called from coins trigger
        StartCoroutine(TakeCoins());

        CoinTXT.transform.localScale = new Vector3(
            CoinTXT.transform.localScale.x,
            CoinTXT.transform.localScale.y + 0.7f,
            CoinTXT.transform.localScale.z
        );

        if (coinSound)
            coinSound.Play();
        Coins += value;
        CoinTXT.text = Coins.ToString();
        PlayerPrefs.SetInt("Coins", Coins);
    }

    /**
     * TakeCoins.
     *
     * @return	mixed
     */
    IEnumerator TakeCoins()
    {
        yield return new WaitForSeconds(0.03f);
        CoinTXT.transform.localScale = new Vector3(
            CoinTXT.transform.localScale.x,
            CoinTXT.transform.localScale.y - 0.7f,
            CoinTXT.transform.localScale.z
        );
    }

    /**
     * AddFuel.
     *
     * @access	public
     * @param	int	value
     * @return	void
     */
    public void AddFuel(int value)
    {
        if (coinSound)
            coinSound.Play();
        TotalFuel = value;
    }

    /**
     * Distance based award
     *
     * @return	void
     */
    void CoinDistance()
    {
        if (!c500)
        {
            if (player.transform.position.x >= 500 && player.transform.position.x < 1000)
            {
                AddCoin(1000);
                coinAwardedBox.SetActive(true);
                awardAnimator.SetBool("Award", true);
                awardedText.text = "1000 Coins Awarded";
                StartCoroutine(Awardfalse());
                c500 = true;
                PlayerPrefs.SetInt("c500" + PlayerPrefs.GetInt("SelectedLevel").ToString(), 3);
                LastRecord.text = "Record:500";
            }
        }
        if (!c1000 && c500)
        {
            if (player.transform.position.x >= 1000 && player.transform.position.x < 1500)
            {
                AddCoin(2000);
                coinAwardedBox.SetActive(true);
                awardAnimator.SetBool("Award", true);
                awardedText.text = "2000 Coins Awarded";
                StartCoroutine(Awardfalse());
                c1000 = true;
                PlayerPrefs.SetInt("c1000" + PlayerPrefs.GetInt("SelectedLevel").ToString(), 3);
                LastRecord.text = "Record:1000";
            }
        }
        if (!c1500 && c1000)
        {
            if (player.transform.position.x >= 1500 && player.transform.position.x < 2000)
            {
                AddCoin(3000);
                coinAwardedBox.SetActive(true);
                awardAnimator.SetBool("Award", true);
                awardedText.text = "3000 Coins Awarded";
                StartCoroutine(Awardfalse());
                c1500 = true;
                PlayerPrefs.SetInt("c1500" + PlayerPrefs.GetInt("SelectedLevel").ToString(), 3);
                LastRecord.text = "Record:1500";
            }
        }
        if (!c2000 && c1500)
        {
            if (player.transform.position.x >= 2000 && player.transform.position.x < 2500)
            {
                AddCoin(4000);
                coinAwardedBox.SetActive(true);
                awardAnimator.SetBool("Award", true);
                awardedText.text = "4000 Coins Awarded";
                StartCoroutine(Awardfalse());
                c2000 = true;
                PlayerPrefs.SetInt("c2000" + PlayerPrefs.GetInt("SelectedLevel").ToString(), 3);
                LastRecord.text = "Record:2000";
            }
        }
        if (!c2500 && c2000)
        {
            if (player.transform.position.x >= 2500 && player.transform.position.x < 3000)
            {
                AddCoin(5000);
                coinAwardedBox.SetActive(true);
                awardAnimator.SetBool("Award", true);
                awardedText.text = "5000 Coins Awarded";
                StartCoroutine(Awardfalse());
                c2500 = true;
                PlayerPrefs.SetInt("c2500" + PlayerPrefs.GetInt("SelectedLevel").ToString(), 3);
                LastRecord.text = "Record:2500";
            }
        }
        if (!c3000 && c2500)
        {
            if (player.transform.position.x >= 3000 && player.transform.position.x < 3500)
            {
                AddCoin(6000);
                coinAwardedBox.SetActive(true);
                awardAnimator.SetBool("Award", true);
                awardedText.text = "6000 Coins Awarded";
                StartCoroutine(Awardfalse());
                c3000 = true;
                PlayerPrefs.SetInt("c3000" + PlayerPrefs.GetInt("SelectedLevel").ToString(), 3);
                LastRecord.text = "Record:3000";
            }
        }
        if (!c3500 && c3000)
        {
            if (player.transform.position.x >= 3500 && player.transform.position.x < 4000)
            {
                AddCoin(7000);
                coinAwardedBox.SetActive(true);
                awardAnimator.SetBool("Award", true);
                awardedText.text = "7000 Coins Awarded";
                StartCoroutine(Awardfalse());
                c3500 = true;
                PlayerPrefs.SetInt("c3500" + PlayerPrefs.GetInt("SelectedLevel").ToString(), 3);
                LastRecord.text = "Record:3500";
            }
        }
        if (!c4000 && c3500)
        {
            if (player.transform.position.x >= 3700 && player.transform.position.x < 4500)
            {
                AddCoin(8000);
                coinAwardedBox.SetActive(true);
                awardAnimator.SetBool("Award", true);
                awardedText.text = "8000 Coins Awarded";
                StartCoroutine(Awardfalse());
                c4000 = true;
                PlayerPrefs.SetInt("c4000" + PlayerPrefs.GetInt("SelectedLevel").ToString(), 3);
                LastRecord.text = "Record:4000";
                youWinMenu.SetActive(true);
                AddCoin(winnerAwardedCoins);
                GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().isKinematic =
                    true;
            }
        }

        // End of the level
        if (player.transform.position.x >= levelLength)
        {
            youWinMenu.SetActive(true);
            GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().isKinematic =
                true;
            Time.timeScale = 0;
        }
    }

    IEnumerator Awardfalse()
    {
        yield return new WaitForSeconds(3f);
        awardAnimator.SetBool("Award", false);
        yield return new WaitForSeconds(3f);
        coinAwardedBox.SetActive(false);
    }

    public void StartDead()
    {
        StartCoroutine(Dead());
    }

    IEnumerator Dead()
    {
        yield return new WaitForSeconds(3f);
        if (isDead)
        {
            youLostMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void StartFuelFinish()
    {
        StartCoroutine(DeadFuel());
    }

    IEnumerator DeadFuel()
    {
        yield return new WaitForSeconds(3f);
        if (fuelFinished)
        {
            youLostMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
