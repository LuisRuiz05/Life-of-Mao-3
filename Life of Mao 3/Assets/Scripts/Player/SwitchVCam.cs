using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.UI;

/// <summary>
///     This class allows to alternate within the normal third person camera and the aiming camera.
/// </summary>
public class SwitchVCam : MonoBehaviour
{
    CinemachineVirtualCamera tpCamera;

    [SerializeField]
    private PlayerInput playerInput;
    [SerializeField]
    private int priorityBoostAmount = 10;
    [SerializeField]
    private Image normalReticle;
    [SerializeField]
    private Image aimReticle;

    private CinemachineVirtualCamera virtualCamera;
    private InputAction aimAction;

    private void Awake()
    {
        tpCamera = GameObject.Find("ThirdPersonCamera").GetComponent<CinemachineVirtualCamera>();
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        aimAction = playerInput.actions["Aim"];
        aimReticle.enabled = false;
        normalReticle.enabled = true;
    }

    private void Start()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        tpCamera.LookAt = player;
        tpCamera.Follow = player;

        virtualCamera.LookAt = player;
        virtualCamera.Follow = player;
    }

    /// <summary>
    ///     Detects the aiming button is pressed.
    /// </summary>
    private void OnEnable()
    {
        aimAction.performed += _ => StartAim();
        aimAction.canceled += _ => CancelAim();
    }

    /// <summary>
    ///     Detects when the aiming button is released.
    /// </summary>
    private void OnDisable()
    {
        aimAction.performed -= _ => StartAim();
        aimAction.canceled -= _ => CancelAim();
    }

    /// <summary>
    ///     Maximizes the aiming camera's priority, so it can go on.
    /// </summary>
    private void StartAim()
    {
        virtualCamera.Priority += priorityBoostAmount;
        aimReticle.enabled = true;
        normalReticle.enabled = false;
    }

    /// <summary>
    ///     Minimizes the aiming camera's priority, so it can go off and back to normal.
    /// </summary>
    private void CancelAim()
    {
        virtualCamera.Priority -= priorityBoostAmount;
        aimReticle.enabled = false;
        normalReticle.enabled = true;
    }
}
