/*
This script implements functionality for the start menu of the game.
*/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Threading.Tasks;
using Thirdweb;

/**
 * This class implements the logic for the start menu of the game.
 *
 * @author	Mahmoud Harmouch
 * @global
 */
public class StartMenuController : MonoBehaviour
{
    [Header("third web wallet")]
    [field: SerializeField]
    GameObject connected;

    [field: SerializeField]
    GameObject disconnected;

    [field: SerializeField]
    GameObject startButton;

    [field: SerializeField]
    GameObject settingsButton;

    [field: SerializeField]
    GameObject exitButton;

    [Header("wallet, contract info")]
    [field: SerializeField]
    Text addressTXT;

    [field: SerializeField]
    Text ownsNftTXT;

    /**
     * Start is called before the first frame update
     *
     * @return	void
     */
    void Start() { }

    /**
     * Update is called once per frame
     *
     * @return	void
     */
    void Update() { }

    /**
     * A method to connect to the user's wallet using the Connect method of the Thirdweb SDK.
     *
     * @return	void
     */
    public async void ConnectWallet()
    {
        // Connect to the user's wallet via CoinbaseWallet
        string address = await ThirdWebController.instance.sdk.wallet.Connect(
            new WalletConnection()
            {
                provider = WalletProvider.MetaMask,
                chainId = 5 // Switch the wallet Goerli on connection
            }
        );
        if (address.Length != 0)
        {
            addressTXT.text =
                address.Substring(0, 5) + "..." + address.Substring(address.Length - 4, 4);
        }

        await CheckBalance();
    }

    /**
     * A method to check if the user is allowed to connect to the game.
     *
     * @return	void
     */
    public async Task CheckBalance()
    {
        Contract contract = ThirdWebController.instance.sdk.GetContract(
            PlayerPrefs.GetString("ContractAddress")
        );
        string balance = await contract.ERC721.Balance();
        float balanceValue = 0;

        if (balance != null)
        {
            balanceValue = float.Parse(balance);
        }

        if (balanceValue == 0)
        {
            ownsNftTXT.text = "Low Balance. Access Denied!";
            return;
        }
        else
        {
            ownsNftTXT.text = "Welcome!";
            connected.SetActive(true);
            disconnected.SetActive(false);
        }
    }
}
