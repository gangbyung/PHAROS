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
            Destroy(gameObject); // 이미 다른 인스턴스가 있으면 삭제
        }
        else
        {
            instance = this; // 인스턴스를 현재 객체로 설정

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
            Debug.LogWarning("타겟 오브젝트가 null입니다.");
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
                Debug.Log("앞에 벽이 있어 앞으로 이동할 수 없습니다.");
                HUD.instance.OnWallWarnImage(true); // 벽 못 지나감 호출
            }
        }
    }
    private void OpenClosestDoorWithKey()
    {
        // 모든 문 중 가장 가까운 문 찾기
        GameObject closestDoor = FindClosestDoor();
        if (closestDoor != null)
        {
            // Animator 가져오기
            Animator doorAnimator = closestDoor.GetComponent<Animator>();
            if (doorAnimator != null)
            {
                // 애니메이션 실행
                doorAnimator.SetBool("isOpenDoor", true);
                Debug.Log($"문 {closestDoor.name} 열림!");

                // 열쇠 소모
                KeyIndex--;
                Debug.Log($"남은 열쇠: {KeyIndex}");

                // BoxCollider 제거
                BoxCollider boxCollider = closestDoor.GetComponent<BoxCollider>();
                if (boxCollider != null)
                {
                    Destroy(boxCollider); // Collider를 완전히 제거
                    Debug.Log($"문 {closestDoor.name}의 BoxCollider 제거됨.");
                }
                else
                {
                    Debug.LogWarning($"문 {closestDoor.name}에 BoxCollider가 없습니다.");
                }
            }
            else
            {
                Debug.LogWarning($"Animator가 {closestDoor.name}에 없습니다.");
            }
        }
        else
        {
            Debug.LogWarning("근처에 문이 없습니다.");
        }
    }

    private GameObject FindClosestDoor()
    {
        // 씬의 모든 문(GameObject)을 탐지
        GameObject[] allDoors = GameObject.FindGameObjectsWithTag("Door"); // 태그를 Door로 설정해야 함
        GameObject closestDoor = null;
        float closestDistance = Mathf.Infinity; // 초기값을 매우 큰 값으로 설정

        foreach (GameObject door in allDoors)
        {
            // 거리 계산
            float distance = Vector3.Distance(transform.position, door.transform.position);

            // 가장 가까운 문을 업데이트
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestDoor = door;
            }
        }

        return closestDoor;
    }

    private bool CanMoveForward() //앞에 벽이 없다면 트루임
    {
        return !Physics.Raycast(transform.position, transform.forward, rayDistance, wallLayer);
    }
    private bool CanMoveForwardDoor() //앞에 문이 없다면 트루임
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
            CameraController.Instance.ResetCamera(); // 카메라는 이동이 끝난 후 리셋됨
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
        Debug.Log("블루 포션 클릭 횟수: " + bluePotionClickCount);

        if (bluePotionClickCount <= 2)
        {
            StartCoroutine(MoveAndReset(5f, () =>
            {
                if (bluePotionClickCount == 2)
                {
                    RemoveBluePotionEffect(); // 이동이 끝난 후 두 번째 클릭이 완료되면 블루 포션 효과 제거
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
