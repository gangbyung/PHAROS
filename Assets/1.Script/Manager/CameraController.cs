using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    public Transform playerTransform;
    public Vector3 originalCameraOffset;
    public Vector3 redPotionCameraOffset; // ���� ���� �� ī�޶� ������ 0, 20, 0
    public Vector3 redPotionCameraRotation; // ���� ���� �� ī�޶� ȸ���� 30, 0, 0
    private float bluePotionYOffset = -1.5f; // ��� ���� �� ī�޶� y ������

    private bool hasRedPotionEffect = false;
    private bool hasBluePotionEffect = false;
    private Vector3 currentCameraOffset;
    private Quaternion targetRotation;

    public float smoothSpeed = 5f; // ī�޶� �̵� �� ȸ���� �ε巯�� �ӵ�

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // �̹� �ٸ� �ν��Ͻ��� ������ ����
        }
        else
        {
            Instance = this; // �ν��Ͻ��� ���� ��ü�� ����
            DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� ������Ʈ�� �������� �ʵ��� ����
        }
    }

    private void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        playerTransform = player.transform;
        
        transform.rotation = Quaternion.Euler(0,180,0);

        if (playerTransform != null)
        {
            // �ʱ� ī�޶� ������ ����
            originalCameraOffset = transform.position - playerTransform.position;
            currentCameraOffset = originalCameraOffset;
            targetRotation = transform.rotation; // �ʱ� ȸ���� ����
        }
    }

    private void LateUpdate()
    {
        if (playerTransform != null)
        {
            // ��ǥ ��ġ ���
            Vector3 targetPosition = playerTransform.position + currentCameraOffset;

            // ī�޶� ��ġ �ε巴�� �̵�
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

            // ī�޶� ȸ�� �ε巴�� ����
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
            // �÷��̾��� ȸ������ ������� Y�� ȸ�� ������Ʈ
            targetRotation = Quaternion.Euler(
                targetRotation.eulerAngles.x,  // ���� X�� ȸ���� ����
                playerRotation.eulerAngles.y,  // Y�� ȸ������ �÷��̾� �������� ����
                targetRotation.eulerAngles.z   // ���� Z�� ȸ���� ����
            );
        }
    }

    private void UpdateCameraPositionAndRotation()
    {
        if (playerTransform != null)
        {
            if (hasRedPotionEffect)
            {
                // ���� ���� ȿ��: ī�޶� ������ �� X�� ȸ�� 30��
                currentCameraOffset = redPotionCameraOffset;
                targetRotation = Quaternion.Euler(30f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            }
            else if (hasBluePotionEffect)
            {
                // ��� ���� ȿ��: �������� �����¿��� Y�� ������ ����
                currentCameraOffset = originalCameraOffset + new Vector3(0, bluePotionYOffset, 0);
                targetRotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z); // ȸ�� �ʱ�ȭ
            }
            else
            {
                // ���� ȿ�� ����: �⺻ ������ �� ȸ�� �ʱ�ȭ
                currentCameraOffset = originalCameraOffset;
                targetRotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            }
        }
    }
}
