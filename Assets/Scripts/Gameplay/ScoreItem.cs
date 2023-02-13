/*
This script defines the behavior of the "ScoreItem" component in the game.
*/
using UnityEngine;
using System.Collections;

/**
 * Declare an enum called with two members, "Coin" and "Fuel".
 *
 * @global
 */
public enum ScoreType
{
    Coin,
    Fuel
}

/**
 * ScoreItem.
 *
 * @author	Mahmoud Harmouch
 * @global
 */
public class ScoreItem : MonoBehaviour
{
    // A variable that represents the score that a player will receive for collecting the ScoreItem.
    [field: SerializeField]
    int Score = 100;

    // A a reference to a "GameManager" object in the scene. The GameManager is used to manage the
    // game state, score, etc.
    [field: SerializeField]
    GameManager man;

    // A reference to a "SpriteRenderer" component. The SpriteRenderer component is used to render 2D
    // images in Unity.
    [field: SerializeField]
    SpriteRenderer sprite;

    // These variables define the upward movement and speed of the ScoreItem and the speed at which
    // the ScoreItem fades out.
    [field: SerializeField]
    Vector3 Up__Vector3 = new Vector3(0, 7f, 0);

    [field: SerializeField]
    float speed = 2.3f,
        FadeSpeed = 2.3f;

    // A variable named "type" that represents the type of score item, either "Coin" or "Fuel".
    [field: SerializeField]
    ScoreType type;

    // A variable that determines if the ScoreItem should be computed.
    [field: SerializeField]
    bool canCompute;

    // A reference to the Transform component of the ScoreItem object.
    [field: SerializeField]
    Transform trans;

    /**
     * Define the Start method.
     *
     * @return	void
     */
    void Start()
    {
        trans = GetComponent<Transform>();
        man = GameObject.FindObjectOfType<GameManager>();
        sprite = GetComponent<SpriteRenderer>();
    }

    /**
     * This method is called when a collider enters the collider attached to the ScoreItem.
     *
     * @param	collider2d	col
     * @return	void
     */
    void OnTriggerEnter2D(Collider2D col)
    {
        // Check if the collider has a tag of "CoinTrigger".
        if (col.CompareTag("CoinTrigger"))
        {
            // Check the type of score item.
            if (type == ScoreType.Coin)
                man.AddCoin(Score);
            if (type == ScoreType.Fuel)
                man.AddFuel(100);

            canCompute = true;
            Fade = true;
        }
    }

    bool Fade;

    /**
     * Update.
     *
     * @return	void
     */
    void Update()
    {
        if (canCompute)
        {
            if (Fade)
            {
                // Move the ScoreItem upward by the "Up__Vector3" multiplied by the "speed"
                // and the time since the last frame.
                trans.Translate(Up__Vector3 * Time.deltaTime * speed);

                if (sprite.material.color.a > 0)
                {
                    sprite.material.color = new Color(
                        1f,
                        1f,
                        1f,
                        sprite.material.color.a - FadeSpeed * Time.deltaTime
                    );
                }
                else
                    Destroy(gameObject);
            }
        }
    }
}
