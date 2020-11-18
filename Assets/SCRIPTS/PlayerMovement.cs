using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
     

    public bool phoneIsConnected = false;

    [Header("Movement")]
    public float jumpForce = 5;
    public float dashForce = 5;
    public float moveSpeed = 10;

    [Header("Audio Clip")]
    public AudioClip calibrateClip;
    public AudioClip jumpClip;
    public AudioClip coinClip;

    [Header("Score UI")]
    public TextMeshProUGUI scoreText;

    [Header("Other Stuff")]
    [Tooltip("Assign the main camera to this. It will be disabled when we encounter a CustomCam.")]
    public GameObject mainCam;
    //public GameObject secondCam; 
    public Button jumpButton;

    [HideInInspector]
    public Vector3 dir, startPosition, calibratedDir;

    //private variables
    Rigidbody rb;
    AudioSource aud;
    bool isGrounded = true;
    bool canJump = false;

    int score = 0;
    int coinScore = 250;

    Vector3 startingPositon;


    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        startPosition = this.transform.position;
        aud = GameObject.Find("CalibrateSound").GetComponent<AudioSource>();
        CalibrateTilt();
        jumpButton.interactable = canJump;
    }

    void Update()
    {

        if(Input.GetKeyDown(KeyCode.R))
        {
            ResetPlayer();
            Debug.Log("Pressed R");
        } 
        // if player falls to this point he respawns at the start.
        if(this.transform.position.y < -100) ResetPlayer();



        if(!phoneIsConnected && Input.GetKeyDown(KeyCode.Space)) Jump();
        
        if(Input.GetKeyDown(KeyCode.Alpha7)) SaveProgress();
        if(Input.GetKeyDown(KeyCode.Alpha8)) LoadProgress();
        if(Input.GetKeyDown(KeyCode.Alpha9)) ResetProgress();


        
    } //END OF UPDATE!!!

    // Update is called once per frame
    void FixedUpdate()
    {
        dir = Vector3.zero;
        if(phoneIsConnected)
        {
            dir.x = Input.acceleration.x - calibratedDir.x;
            dir.z = Input.acceleration.y - calibratedDir.z;
        } 
        else
        {
            dir.x = Input.GetAxis("Horizontal");
            dir.z = Input.GetAxis("Vertical");

        }
        rb.AddForce(dir * moveSpeed);
    }

    public void ResetPlayer()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        this.transform.position = startPosition;
    }

    public void CalibrateTilt()
    {
        Debug.Log("Calibrating Tilt");
        calibratedDir.x = Input.acceleration.x;
        calibratedDir.z = Input.acceleration.y;
        aud.PlayOneShot(calibrateClip);
    }

    public void Jump()
    {
        if(isGrounded && canJump == true)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            aud.PlayOneShot(jumpClip);
        }
    }

    public void Dash()
    {
        if(true)
        {
            rb.AddForce(dir * dashForce, ForceMode.Impulse);
            aud.PlayOneShot(jumpClip);
        }
    }

    void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            if(canJump) jumpButton.interactable = true;
        }    
    }

    void OnCollisionExit(Collision other) {
        if(other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            jumpButton.interactable = false;    // add to Start()
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Spikes"))
        {
            ResetProgress();
        }

        //Debug.Log("Entering trigger" + other.gameObject.name);
        if(other.gameObject.CompareTag("Coin"))
        {
            score += coinScore;
            scoreText.text = "Score = " + score;
            aud.PlayOneShot(coinClip);
            Destroy(other.gameObject);
        }
        if(other.gameObject.name == "Jump Pickup")
        {
            canJump = true;
            jumpButton.interactable = true;
            Destroy(other.gameObject);
        }

        if(other.gameObject.CompareTag("CustomCam"))
        {
            mainCam.SetActive(false);
            //secondCam.SetActive(true);
            other.transform.GetChild(0).gameObject.SetActive(true);
        }

        if(other.gameObject.CompareTag("Door"))
        {
            if(score == 1000)
            {
                Destroy(other.gameObject);
            }
        }

        if(other.gameObject.CompareTag("Door2"))
        {
            if(score == 1500)
            {
                Destroy(other.gameObject);
            }
        }

        if(other.gameObject.CompareTag("Checkpoint"))
        {
            SaveProgress();
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Exiting Trigger" + other.gameObject.name);
        if(other.gameObject.CompareTag("CustomCam"))
        {
            other.transform.GetChild(0).gameObject.SetActive(false);
            mainCam.SetActive(true);
        }
    }

    public void SaveProgress() 
    {
        Debug.Log("Saving our progress.");
        // save health, mana, xp, hp potions, mp potions, and position;
        //PlayerPrefs.SetFloat("Health", health);
        //PlayerPrefs.SetFloat("Mana", mana);
        //PlayerPrefs.SetFloat("XP", xp);

        //PlayerPrefs.SetInt("HP Potions", totalHPPotion);
        //PlayerPrefs.SetInt("MP Potions", totalMPPotion);

        // Save our Vector 3 position as 3 floats
        PlayerPrefs.SetFloat("X pos", this.transform.position.x); //0
        PlayerPrefs.SetFloat("Y pos", this.transform.position.y); //1
        PlayerPrefs.SetFloat("Z pos", this.transform.position.z); //0
    }

    public void LoadProgress()
    {
        Debug.Log("Loading our saved progress");

        //health = PlayerPrefs.GetFloat("Health");
        //mana = PlayerPrefs.GetFloat("Mana");
        //xp = PlayerPrefs.GetFloat("XP");

        //totalHPPotion = PlayerPrefs.GetInt("HP Potions");
        //totalHPPotion = PlayerPrefs.GetInt("MP Potions");

        Vector3 savedPos;
        savedPos.x = PlayerPrefs.GetFloat("X pos");     // 0
        savedPos.y = PlayerPrefs.GetFloat("Y pos");     // 0 if returned incorrectly
        savedPos.z = PlayerPrefs.GetFloat("Z pos");     // 0

        this.transform.position = savedPos;
        //UpdateUI();
    }

    public void ResetProgress()
    {
        Debug.Log("Resetting Progress");
        //health = 100;
        //mana = 100;
        //xp = 0;
        //totalHPPotion = 0;
        //totalMPPotion = 0;
        this.transform.position = startingPositon;       //TODO: create a "startingPosition" variable.
        SaveProgress();
        //UpdateUI();
    }

}
