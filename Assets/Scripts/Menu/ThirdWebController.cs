/*
This script implements a singleton class, which is used to interface with a
Thirdweb SDK for interacting with the blockchain network.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thirdweb;

/**
 * A singleton class.
 *
 * @author	Mahmoud Harmouch
 * @global
 */
public class ThirdWebController : MonoBehaviour
{
    public static ThirdWebController instance;

    // Create a Thirdweb SDK instance to use throughout this class.
    public ThirdwebSDK sdk;

    /**
     * An event that is called when the script instance is enabled, here the
     * method sets up the Thirdweb SDK by instantiating a new ThirdwebSDK object
     * with the "goerli" test network.
     *
     * @return	void
     */
    void Start()
    {
        // When the app starts, set up the Thirdweb SDK
        // Here, we're setting up a read-only instance on the "goerli" test network.
        sdk = new ThirdwebSDK("goerli");
    }

    /**
     * An event that is called when the script instance is being loaded, here the method
     * implements a singleton pattern to ensure that there is only one instance of the
     * ThirdWebController class in the scene at any given time.
     *
     * @access	private
     * @return	void
     */
    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    /**
     * Update is called once per frame
     *
     * @return	void
     */
    void Update() { }
}
