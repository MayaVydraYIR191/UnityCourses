using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragonController : MonoBehaviour
{
    private CharacterController controller;

    [SerializeField]
    private float rotationSpeed = 10f;

    [SerializeField]
    private float playerSpeed = 5f;

    [SerializeField]
    private Camera followCamera;

    private Vector3 playerVelocity;
    private bool groundedPlayer;
    [SerializeField]
    private float jumpHeight;
    [SerializeField]
    private float gravityValue = -9.81f;

    private float FlightSpeed = 320;
    private float YawAmount = 120;
    private float Yaw;

    private bool dragonIsRunning;

    private Animator anime;

    public int playerHealth;
    public GameObject RestartButton;
    public Image bar;
    public float fill;

    private void Start()
    {
        anime = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        fill = 1f;
        playerHealth = 5;
        controller.enabled = true;
        RestartButton.SetActive(false);
        dragonIsRunning = false;
    }

    private void Update()
    {
        Movement();
        if(Input.GetKeyDown(KeyCode.F) == true)
        {
            gravityValue = -0.5f;
            playerSpeed = playerSpeed + 10f;
            anime.SetBool("isFlying", true);
            Flight();
        }
        if (playerHealth <= 0)
        {
            anime.SetBool("isDead", true);
            controller.enabled = false;
            RestartButton.SetActive(true);
        }
        if(Input.GetKeyDown(KeyCode.R)==true && dragonIsRunning == false)
        {
            dragonIsRunning = true;
            playerSpeed = playerSpeed + 10f;
            anime.SetBool("isRunning", true);
        }
        else if(Input.GetKeyDown(KeyCode.R) == true && dragonIsRunning == true)
        {
            dragonIsRunning = false;
            playerSpeed = 5f;
            anime.SetBool("isRunning", false);
        }
    }

    void Movement()
    {
        jumpHeight = 10f;
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        float horizonInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementInput = Quaternion.Euler(0,followCamera.transform.eulerAngles.y,0) * new Vector3(horizonInput,0, verticalInput);
        Vector3 movementDirection = movementInput.normalized;

        controller.Move(movementDirection * playerSpeed * Time.deltaTime);

        if (movementDirection != Vector3.zero)
        {
            Quaternion desiredRotate = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotate, rotationSpeed * Time.deltaTime);
            anime.SetBool("isWalking", true);
        }
        else
        {
            anime.SetBool("isWalking", false);
        }
             

        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            anime.SetBool("isJumping", true);
        }
        else
        {
            anime.SetBool("isJumping", false);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void Flight()
    {
        gravityValue = -0.5f;
        jumpHeight = 10f;
        transform.position += transform.forward * FlightSpeed * Time.deltaTime;

        float FhorizonInput = Input.GetAxis("Horizontal");
        float FverticalInput = Input.GetAxis("Vertical");
        float FRiseDown = Input.GetAxis("Mouse Y");

        Vector3 movementInput = Quaternion.Euler(0, followCamera.transform.eulerAngles.y, 0) * new Vector3(FhorizonInput, 0, FverticalInput);
        Vector3 movementDirection = movementInput.normalized;

        Yaw += FhorizonInput * YawAmount * Time.deltaTime;
        float pitch = Mathf.Lerp(0,20,Mathf.Abs(FverticalInput))*Mathf.Sign(FverticalInput);
        float roll = Mathf.Lerp(0, 30, Mathf.Abs(FhorizonInput)) * -Mathf.Sign(FhorizonInput);

        transform.localRotation = Quaternion.Euler(Vector3.up * Yaw + Vector3.right * FRiseDown + Vector3.forward * roll); 
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Ground")
        {
            playerSpeed = 5f;
            gravityValue = -9.81f;
            anime.SetBool("isFlying", false);
            Movement();
        }
        if (other.gameObject.tag == "Danger")
        {
            fill = fill - 0.2f;
            bar.fillAmount = fill;
            anime.SetBool("isAttacked", true);
            playerHealth = (playerHealth - 1);
        }
        else if (other.gameObject.tag == "Restoration")
        {
            anime.SetBool("isAttacked", false);
            fill = fill + 0.2f;
            bar.fillAmount = fill;
            playerHealth = (playerHealth + 1);
        }
    }
}
