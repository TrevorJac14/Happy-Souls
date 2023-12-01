using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;
    // 1. Find inputs of a joystick or keyboard
    // 2. Move player accordingly
    PlayerControls playerControls;

    [SerializeField] Vector2 movementInput;
    public float horizontalInput;
    public float verticalInput;

    public float moveAmount;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        // WHEN THE SCENE CHANGES, RUN THIS LOGIC
        SceneManager.activeSceneChanged += OnSceneChange;

        instance.enabled = false;
    }

    private void OnSceneChange(Scene oldScene, Scene newScene)
    {
        // IF WE ARE LOADING INTO OUR WORLD SCENE, ENABLE PLAYER CONTROLS
        if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
        {
            instance.enabled = true;
        }
        // OTHERWISE WE MUST BE AT THE MAIN MENU, DISABLE PLAYER CONTROLS
        // SO IF PLAYER IS IN CREATION OR SOMETHING THEIR CHARACTER WONT MOVE
        else
        {
            instance.enabled = false;
        }
    }
    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
        }

        playerControls.Enable();
    }

    private void OnDestroy()
    {
        // IF WE DESTORY THIS OBJECT, UNSUBSCRIBE FROM THIS EVENT
        SceneManager.activeSceneChanged -= OnSceneChange;
    }

    
    private void Update()
    {
        HandleMovementInput();
    }
    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;


        // RETURNS ABSOLUTE NUMBER NO NEGATIVES
        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

        // THIS IS CALLED CLAMPING AND MAKES IT FEEL BETTER
        if (moveAmount <= 0.5 && moveAmount > 0)
        {
            moveAmount = 0.5f;
        }
        else if (moveAmount > 0.5 && moveAmount <= 1)
        {
            moveAmount = 1;
        }
    }
}