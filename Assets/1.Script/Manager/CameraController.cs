using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    public Transform playerTransform;
    public Vector3 originalCameraOffset;
    public Vector3 redPotionCameraOffset; //0,20,0
    public Vector3 redPotionCameraRotation; // 빨간 포션 효과 시 카메라의 회전 30,0,0
    private float bluePotionYOffset = -1.5f; // 블루 포션 효과 시 카메라의 y 오프셋

    private bool hasRedPotionEffect = false;
    private bool hasBluePotionEffect = false;
    private Vector3 currentCameraOffset;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if (playerTransform != null)
        {
            // 초기 카메라 오프셋 저장
            originalCameraOffset = transform.position - playerTransform.position;
            currentCameraOffset = originalCameraOffset;
        }
    }

    private void LateUpdate()
    {
        if (playerTransform != null)
        {
            // 카메라의 위치 업데이트
            Vector3 targetPosition = playerTransform.position + currentCameraOffset;
            transform.position = targetPosition;
        }
    }

    public void ApplyRedPotionEffect()
    {
        hasRedPotionEffect = true;
        UpdateCameraPositionAndRotation();
    }

    public void ResetCamera()
    {
        hasRedPotionEffect = false;
        hasBluePotionEffect = false;
        UpdateCameraPositionAndRotation();
    }


    public void ApplyBluePotionEffect()
    {
        hasBluePotionEffect = true;
        UpdateCameraPositionAndRotation();
    }

    public void UpdateCameraRotation(Quaternion playerRotation)
    {
        if (playerTransform != null)
        {
            transform.rotation = Quaternion.Euler(
                transform.rotation.eulerAngles.x,
                playerRotation.eulerAngles.y,
                transform.rotation.eulerAngles.z
            );
        }
    }

    private void UpdateCameraPositionAndRotation()
    {
        if (playerTransform != null)
        {
            if (hasRedPotionEffect)
            {
                currentCameraOffset = redPotionCameraOffset;
                transform.rotation = Quaternion.Euler(redPotionCameraRotation);
            }
            else if (hasBluePotionEffect)
            {
                currentCameraOffset = originalCameraOffset + new Vector3(0, bluePotionYOffset, 0);
                transform.rotation = Quaternion.Euler(Vector3.zero);
            }
            else
            {
                currentCameraOffset = originalCameraOffset;
                transform.rotation = Quaternion.Euler(Vector3.zero);
            }

            // 카메라의 위치 업데이트
            Vector3 targetPosition = playerTransform.position + currentCameraOffset;
            transform.position = targetPosition;
        }
    }
}
