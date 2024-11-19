using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    public Transform playerTransform;
    public Vector3 originalCameraOffset;
    public Vector3 redPotionCameraOffset; // 빨간 포션 시 카메라 오프셋 0, 20, 0
    public Vector3 redPotionCameraRotation; // 빨간 포션 시 카메라 회전값 30, 0, 0
    private float bluePotionYOffset = -1.5f; // 블루 포션 시 카메라 y 오프셋

    private bool hasRedPotionEffect = false;
    private bool hasBluePotionEffect = false;
    private Vector3 currentCameraOffset;
    private Quaternion targetRotation;

    public float smoothSpeed = 5f; // 카메라 이동 및 회전의 부드러운 속도

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 이미 다른 인스턴스가 있으면 삭제
        }
        else
        {
            Instance = this; // 인스턴스를 현재 객체로 설정
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 오브젝트가 삭제되지 않도록 설정
        }
    }

    private void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        playerTransform = player.transform;
        
        transform.rotation = Quaternion.Euler(0,180,0);

        if (playerTransform != null)
        {
            // 초기 카메라 오프셋 저장
            originalCameraOffset = transform.position - playerTransform.position;
            currentCameraOffset = originalCameraOffset;
            targetRotation = transform.rotation; // 초기 회전값 저장
        }
    }

    private void LateUpdate()
    {
        if (playerTransform != null)
        {
            // 목표 위치 계산
            Vector3 targetPosition = playerTransform.position + currentCameraOffset;

            // 카메라 위치 부드럽게 이동
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

            // 카메라 회전 부드럽게 적용
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);
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
            // 플레이어의 회전값을 기반으로 Y축 회전 업데이트
            targetRotation = Quaternion.Euler(
                targetRotation.eulerAngles.x,  // 기존 X축 회전값 유지
                playerRotation.eulerAngles.y,  // Y축 회전값만 플레이어 기준으로 변경
                targetRotation.eulerAngles.z   // 기존 Z축 회전값 유지
            );
        }
    }

    private void UpdateCameraPositionAndRotation()
    {
        if (playerTransform != null)
        {
            if (hasRedPotionEffect)
            {
                // 빨간 포션 효과: 카메라 오프셋 및 X축 회전 30도
                currentCameraOffset = redPotionCameraOffset;
                targetRotation = Quaternion.Euler(30f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            }
            else if (hasBluePotionEffect)
            {
                // 블루 포션 효과: 오리지널 오프셋에서 Y축 오프셋 조정
                currentCameraOffset = originalCameraOffset + new Vector3(0, bluePotionYOffset, 0);
                targetRotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z); // 회전 초기화
            }
            else
            {
                // 포션 효과 없음: 기본 오프셋 및 회전 초기화
                currentCameraOffset = originalCameraOffset;
                targetRotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            }
        }
    }
}
