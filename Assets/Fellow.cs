using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Fellow : MonoBehaviour
{

    public Vector3 startPosition;
    public Transform RightReceiver;
    public Transform LeftReceiver;
    public Text scoreText;
    public Text livesText;
    void Start()
    {
        normalSpeed = speed;
        startPosition = transform.position;
        currentLives = playerLives;
        SetScoreText();
        SetLivesText();
    }

    void SetScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }

      void SetLivesText()
    {
        livesText.text = "Lives: " + currentLives.ToString();
    }
    void Update()
    {
        powerupTime = Mathf.Max(0.0f, powerupTime - Time.deltaTime);

    }
    void FixedUpdate()
    {
        Rigidbody b = GetComponent < Rigidbody >();
        Vector3 velocity = b. velocity ;

        if ( Input . GetKey ( KeyCode .A))
        {
            velocity .x = -speed;
        }
         if (Input.GetKey(KeyCode.D)){

            velocity .x = speed;
        }
         if (Input.GetKey(KeyCode.W)){

            velocity .z = speed;
        }
         if (Input.GetKey(KeyCode.S)){

            velocity .z = -speed;
        }
        b.velocity = velocity;

    }

    int score = 0;
    int pelletsEaten = 0;
    [SerializeField]
    int pointsPerPellet = 100;

     [SerializeField]
    float powerupDuration = 10.0f;
    float powerupTime = 0.0f;
    float speed = 3.0f;
    float normalSpeed;
    float boostedSpeed = 6.0f;
    float speedCooldown = 4.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pellet"))
        {
        pelletsEaten++;
        score += pointsPerPellet;
        Debug.Log("Score is " + score);
        SetScoreText();
        }

        if (other.gameObject.CompareTag("Powerup"))
        {
            powerupTime = powerupDuration;
        }

        if(other.gameObject.CompareTag("Speedup"))
        {
            //Allow the user to speed up
           speed = boostedSpeed;
           Debug.Log("Fellow has been speed up!");
        }
        if(other.gameObject.CompareTag("LeftTeleporter"))
        {
            GetComponent<Transform>().position = RightReceiver.position;
        }
        if(other.gameObject.CompareTag("RightTeleporter"))
        {
            GetComponent<Transform>().position = LeftReceiver.position;
        }
    }

    IEnumerator SpeedDuration()
    {
        yield return new WaitForSeconds(speedCooldown);
        speed = normalSpeed;
    }

     public bool PowerupActive()
    {
        return powerupTime > 0.0f;
    }

    public int PelletsEaten()
    {
        return pelletsEaten;
    }

[SerializeField]
 private static int playerLives = 4;
 private static int currentLives;
    void OnCollisionEnter(Collision collison)
    {
        if (collison.gameObject.CompareTag("Ghost"))
        {
            Debug.Log("You died!");
            // reset the user position to default and use lives
            //gameObject.SetActive(false);
            playerDead();
            transform.position = startPosition;
            currentLives --;
            Debug.Log(currentLives + " lives");
            SetLivesText();
        }
    }


    public void playerDead()
        {
            if(currentLives < 1)
            {
                ResetPlayerState();
                Debug.Log("You died, Game over!");
            }
            else
            {
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 0);
            }
        }

    public void ResetPlayerState()
    {
        currentLives = playerLives;
        SceneManager.LoadScene("SampleScene");
    }
}
