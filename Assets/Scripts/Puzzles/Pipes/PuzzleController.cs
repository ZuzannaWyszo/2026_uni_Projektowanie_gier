using UnityEngine;
using Unity.Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;

public class PuzzleController : MonoBehaviour
{
    public CinemachineCamera puzzleCamera;

    public float dystansKlikniecia = 10f;

    private GameObject player;
    private StarterAssetsInputs starterInputs;
    private FirstPersonController fpsController;
    private CharacterController charController;
    private bool isInPuzzle = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            starterInputs = player.GetComponent<StarterAssetsInputs>();
            fpsController = player.GetComponent<FirstPersonController>();
            charController = player.GetComponent<CharacterController>();
        }
    }

    void Update()
    {
        if (isInPuzzle)
        {
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                ExitPuzzle();
            }

            if (Mouse.current.leftButton.wasPressedThisFrame && Camera.main != null)
            {
                Vector2 mousePos = Mouse.current.position.ReadValue();
                Ray ray = Camera.main.ScreenPointToRay(mousePos);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    PipePiece pipe = hit.collider.GetComponentInParent<PipePiece>();

                    if (pipe != null)
                    {
                        pipe.RotatePipe();
                        PipesGenerator generator = GetComponent<PipesGenerator>();
                        if (generator != null)
                        {
                            generator.CheckWinCondition();
                        }
                    }
                }
            }
        }
        else
        {
            if (Mouse.current.leftButton.wasPressedThisFrame && Camera.main != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

                if (Physics.Raycast(ray, out RaycastHit hit, dystansKlikniecia))
                {
                    if (hit.collider.gameObject == this.gameObject || hit.collider.GetComponentInParent<PipePiece>() != null)
                    {
                        EnterPuzzle();
                    }
                }
            }
        }
    }

    void EnterPuzzle()
    {
        isInPuzzle = true;
        puzzleCamera.Priority = 20;

        fpsController.enabled = false;
        starterInputs.cursorInputForLook = false;

        if (charController != null)
        {
            charController.enabled = false; 
            player.transform.position = puzzleCamera.transform.position - (puzzleCamera.transform.forward * 2f);
            charController.enabled = true; 
        }

        starterInputs.cursorLocked = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void ExitPuzzle()
    {
        isInPuzzle = false;
        puzzleCamera.Priority = 5;

        fpsController.enabled = true;
        starterInputs.cursorInputForLook = true;

        starterInputs.cursorLocked = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}