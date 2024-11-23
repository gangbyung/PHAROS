using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;

public class PlayerMove : MonoBehaviour
{
    public GameObject targetObject;
    public Button LeftrotateButton;
    public Button RightrotateButton;
    public Button ForwardButton;
    public Button BackButton;
    public Button RedPotionActionButton;
    public Button BluePotionActionButton;
    public float rotationSpeed = 90f;
    public float moveSpeed = 5f;
    public float maxDistance = 10f;
    public float rayDistance = 6f;
    public LayerMask wallLayer;
    public LayerMask doorLayer;

    public int KeyIndex = 0;

    private Quaternion targetRotation;
    private bool isMovingForward = false;
    private Vector3 startPosition;
    private bool hasRedPotionEffect = false;
    private bool hasBluePotionEffect = false;
    private float redPotionScaleMultiplier = 2f;
    private float originalMoveSpeed;
    private int bluePotionClickCount = 0;


    public static PlayerMove instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // �̹� �ٸ� �ν��Ͻ��� ������ ����
        }
        else
        {
            instance = this; // �ν��Ͻ��� ���� ��ü�� ����

            LeftrotateButton.onClick.AddListener(() => StartRotation(-90f));
            RightrotateButton.onClick.AddListener(() => StartRotation(90f));
            BackButton.onClick.AddListener(() => StartRotation(180f));
            ForwardButton.onClick.AddListener(OnForwardButtonClick);
            RedPotionActionButton.onClick.AddListener(OnRedPotionActionButtonClick);
            BluePotionActionButton.onClick.AddListener(OnBluePotionActionButtonClick);
        }
        
    }

    
    private void Start()
    {
        if (targetObject != null)
        {
            targetRotation = targetObject.transform.rotation;
        }
        else
        {
            Debug.LogWarning("Ÿ�� ������Ʈ�� null�Դϴ�.");
        }
        originalMoveSpeed = moveSpeed;
        RedPotionActionButton.gameObject.SetActive(false);
        BluePotionActionButton.gameObject.SetActive(false);

    }

    private void Update()
    {
        if (isMovingForward)
        {
            MoveForward();
        }
        UpdateCameraRotation();
    }

    private void MoveForward()
    {
        Vector3 move = transform.forward * moveSpeed * Time.deltaTime;
        transform.Translate(move, Space.World);

        if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
        {
            StopMovingForward();
        }
    }

    private void UpdateCameraRotation()
    {
        if (targetObject != null)
        {
            CameraController.Instance.UpdateCameraRotation(targetObject.transform.rotation);
        }
    }
    private void OnForwardButtonClick()
    {
        if (CanMoveForward() && CanMoveForwardDoor())
        {
            StartMovingForward();
        }
        else
        {
            if (KeyIndex > 0 && !CanMoveForwardDoor())
            {
                OpenClosestDoorWithKey();
            }
            else
            {
                Debug.Log("�տ� ���� �־� ������ �̵��� �� �����ϴ�.");
                HUD.instance.OnWallWarnImage(true); // �� �� ������ ȣ��
            }
        }
    }
    private void OpenClosestDoorWithKey()
    {
        // ��� �� �� ���� ����� �� ã��
        GameObject closestDoor = FindClosestDoor();
        if (closestDoor != null)
        {
            // Animator ��������
            Animator doorAnimator = closestDoor.GetComponent<Animator>();
            if (doorAnimator != null)
            {
                // �ִϸ��̼� ����
                doorAnimator.SetBool("isOpenDoor", true);
                Debug.Log($"�� {closestDoor.name} ����!");

                // ���� �Ҹ�
                KeyIndex--;
                Debug.Log($"���� ����: {KeyIndex}");

                // BoxCollider ����
                BoxCollider boxCollider = closestDoor.GetComponent<BoxCollider>();
                if (boxCollider != null)
                {
                    Destroy(boxCollider); // Collider�� ������ ����
                    Debug.Log($"�� {closestDoor.name}�� BoxCollider ���ŵ�.");
                }
                else
                {
                    Debug.LogWarning($"�� {closestDoor.name}�� BoxCollider�� �����ϴ�.");
                }
            }
            else
            {
                Debug.LogWarning($"Animator�� {closestDoor.name}�� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogWarning("��ó�� ���� �����ϴ�.");
        }
    }

    private GameObject FindClosestDoor()
    {
        // ���� ��� ��(GameObject)�� Ž��
        GameObject[] allDoors = GameObject.FindGameObjectsWithTag("Door"); // �±׸� Door�� �����ؾ� ��
        GameObject closestDoor = null;
        float closestDistance = Mathf.Infinity; // �ʱⰪ�� �ſ� ū ������ ����

        foreach (GameObject door in allDoors)
        {
            // �Ÿ� ���
            float distance = Vector3.Distance(transform.position, door.transform.position);

            // ���� ����� ���� ������Ʈ
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestDoor = door;
            }
        }

        return closestDoor;
    }

    private bool CanMoveForward() //�տ� ���� ���ٸ� Ʈ����
    {
        return !Physics.Raycast(transform.position, transform.forward, rayDistance, wallLayer);
    }
    private bool CanMoveForwardDoor() //�տ� ���� ���ٸ� Ʈ����
    {
        return !Physics.Raycast(transform.position, transform.forward, rayDistance, doorLayer);
    }

    private void StartMovingForward()
    {
        isMovingForward = true;
        startPosition = transform.position;
        SetButtonsInteractable(false);
    }

    private void StopMovingForward()
    {
        isMovingForward = false;
        SetButtonsInteractable(true);
    }

    private void StartRotation(float angle)
    {
        SetButtonsInteractable(false);
        if (targetObject != null)
        {
            targetRotation = Quaternion.Euler(
                targetObject.transform.eulerAngles.x,
                targetObject.transform.eulerAngles.y + angle,
                targetObject.transform.eulerAngles.z
            );

            StartCoroutine(RotateTargetObject());
        }
    }

    private IEnumerator RotateTargetObject()
    {
        while (Quaternion.Angle(targetObject.transform.rotation, targetRotation) > 0.1f)
        {
            targetObject.transform.rotation = Quaternion.RotateTowards(
                targetObject.transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
            yield return null;
        }
        targetObject.transform.rotation = targetRotation;
        SetButtonsInteractable(true);
    }

    private void SetButtonsInteractable(bool interactable)
    {
        LeftrotateButton.interactable = interactable;
        RightrotateButton.interactable = interactable;
        ForwardButton.interactable = interactable;
        BackButton.interactable = interactable;
        RedPotionActionButton.interactable = hasRedPotionEffect && interactable;
        BluePotionActionButton.interactable = hasBluePotionEffect && interactable;
    }

    public void ApplyRedPotionEffect()
    {
        if (!hasRedPotionEffect)
        {
            SoundManager.Instance.PlaySound(0);
            AIMovement.Instance.SetRedPotionEffect(true);
            hasRedPotionEffect = true;
            transform.localScale *= redPotionScaleMultiplier;
            moveSpeed *= redPotionScaleMultiplier;
            CameraController.Instance.ApplyRedPotionEffect();
            RedPotionActionButton.gameObject.SetActive(true);
        }
    }

    public void RemoveRedPotionEffect()
    {
        if (hasRedPotionEffect)
        {
            hasRedPotionEffect = false;
            transform.localScale /= redPotionScaleMultiplier;
            moveSpeed = originalMoveSpeed;
            AIMovement.Instance.SetRedPotionEffect(false);
            CameraController.Instance.ResetCamera();
        }
    }

    public void ApplyBluePotionEffect()
    {
        if (!hasBluePotionEffect)
        {
            SoundManager.Instance.PlaySound(1);
            hasBluePotionEffect = true;
            CameraController.Instance.ApplyBluePotionEffect();
            BluePotionActionButton.gameObject.SetActive(true);
        }
    }

    private void RemoveBluePotionEffect()
    {
        if (hasBluePotionEffect)
        {
            hasBluePotionEffect = false;
            CameraController.Instance.ResetCamera(); // ī�޶�� �̵��� ���� �� ���µ�
            BluePotionActionButton.gameObject.SetActive(false);
        }
    }

    private void OnRedPotionActionButtonClick()
    {
        if (hasRedPotionEffect)
        {
            SetButtonsInteractable(false);
            RedPotionActionButton.gameObject.SetActive(false);
            StartCoroutine(MoveAndReset(40f, RemoveRedPotionEffect));
        }
    }

    private void OnBluePotionActionButtonClick()
    {
        bluePotionClickCount++;
        SetButtonsInteractable(false);
        Debug.Log("��� ���� Ŭ�� Ƚ��: " + bluePotionClickCount);

        if (bluePotionClickCount <= 2)
        {
            StartCoroutine(MoveAndReset(5f, () =>
            {
                if (bluePotionClickCount == 2)
                {
                    RemoveBluePotionEffect(); // �̵��� ���� �� �� ��° Ŭ���� �Ϸ�Ǹ� ��� ���� ȿ�� ����
                }
            }));
        }
    }

    private IEnumerator MoveAndReset(float distanceToMove, System.Action onComplete)
    {
        float startTime = Time.time;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + transform.forward * distanceToMove;

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / distanceToMove;
            transform.position = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);
            yield return null;
        }

        transform.position = targetPosition;
        onComplete?.Invoke();
        SetButtonsInteractable(true);
    }

    public void KeyIndexadd()
    {
        KeyIndex += 1;
        Debug.Log(KeyIndex);
    }
}
