using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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

    public int KeyIndex = 0;

    private Quaternion targetRotation;
    private bool isMovingForward = false;
    private Vector3 startPosition;
    private bool hasRedPotionEffect = false;
    private bool hasBluePotionEffect = false;
    private float redPotionScaleMultiplier = 2f;
    private float originalMoveSpeed;
    private int bluePotionClickCount = 0;

    private static PlayerMove instanse;

    public static PlayerMove Instance
    {
        get
        {
            if(instanse == null)
            {
                instanse = new PlayerMove();
            }
            return instanse;
        }
    }
    private void Awake()
    {
        LeftrotateButton.onClick.AddListener(() => StartRotation(-90f));
        RightrotateButton.onClick.AddListener(() => StartRotation(90f));
        BackButton.onClick.AddListener(() => StartRotation(180f));
        ForwardButton.onClick.AddListener(OnForwardButtonClick);
        RedPotionActionButton.onClick.AddListener(OnRedPotionActionButtonClick);
        BluePotionActionButton.onClick.AddListener(OnBluePotionActionButtonClick);
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
        if (CanMoveForward())
        {
            StartMovingForward();
        }
        else
        {
            Debug.Log("앞에 벽이 있어 앞으로 이동할 수 없습니다.");
            if(KeyIndex > 0 )
            {
                StartMovingForward();
                --KeyIndex;
                Debug.Log(KeyIndex);
            }
        }
    }

    private bool CanMoveForward()
    {
        return !Physics.Raycast(transform.position, transform.forward, rayDistance, wallLayer);
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
            RedPotionActionButton.gameObject.SetActive(false);
            StartCoroutine(MoveAndReset(40f, RemoveRedPotionEffect));
        }
    }

    private void OnBluePotionActionButtonClick()
    {
        bluePotionClickCount++;
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
    }

    public void KeyIndexadd(int _index)
    {
        KeyIndex = ++_index;
    }
}
