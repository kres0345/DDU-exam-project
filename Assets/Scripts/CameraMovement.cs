using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour
{
    public PlayerInput PlayerInput;
    public float MinZoomSize = 5;
    public float MaxZoomSize = 50;
    public GameObject MenuObject;
    public GameObject StorageView;
    public GameObject SpecificContainerView;
    public GameObject RecipeSelect;
    private Camera cam;
    private InputAction inputTurboMode;
    private InputAction inputMove;
    private InputAction inputZoom;

    // Start is called before the first frame update
    void Awake()
    {
        cam = GetComponent<Camera>();

        inputMove = PlayerInput.actions["Move"];
        inputTurboMode = PlayerInput.actions["TurboMode"];
        inputZoom = PlayerInput.actions["ZoomScroll"];
    }

    // Update is called once per frame
    void Update()
    {
        if (MenuObject.activeSelf || StorageView.activeSelf || SpecificContainerView.activeSelf  || RecipeSelect.activeSelf)
            return;

        int cameraSpeed = inputTurboMode.phase == InputActionPhase.Waiting ? 1 : 3;

        Vector2 movement = cameraSpeed * Mathf.Sqrt(cam.orthographicSize) * inputMove.ReadValue<Vector2>() / 10 ;
        transform.position += new Vector3(movement.x, movement.y);
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - Mathf.Clamp(inputZoom.ReadValue<float>(), -1, 1) * cam.orthographicSize / 10, MinZoomSize, MaxZoomSize);
    }
}
