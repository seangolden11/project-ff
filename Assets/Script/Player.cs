using UnityEngine;
using UnityEngine.InputSystem; // Input System 관련 기능을 사용하기 위해 필요합니다.

public class Player : MonoBehaviour
{
    [Header("이동 설정")]
    [SerializeField] private float moveSpeed = 5f;       // 플레이어 이동 속도
    [SerializeField] private float rotationSpeed = 720f;  // 플레이어 회전 속도

    
    private Vector2 inputVec2;        // Input System의 OnMove에서 받을 2D 입력 벡터
    private Vector3 moveDirection;    // 실제 플레이어 이동에 사용될 3D 벡터 (XZ 평면)

    public Animator animator;


    void Start()
    {
        animator = GetComponent<Animator>();
    }
    // Input System의 'Move' 액션이 트리거될 때 자동으로 호출되는 콜백 함수
    void OnMove(InputValue value)
    {
        inputVec2 = value.Get<Vector2>();
        if (inputVec2 != null)
        {
            
            moveDirection = new Vector3(inputVec2.x, 0, inputVec2.y);
        }
        
    }

    void FixedUpdate() // 물리 업데이트는 FixedUpdate에서 처리하는 것이 좋습니다.
    {





        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
        if (moveDirection.magnitude > 0.1f) // 작은 값으로 임계점을 두어 떨림 방지
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            animator.SetFloat("Speed", 1);
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }
        
        
    }

    // Update() 함수는 이제 비워두거나 다른 비-물리적 처리를 위해 사용합니다.
    // 현재 코드에서는 더 이상 Update()에서 호출하는 것이 없습니다.
    // 하지만 카메라 스크립트 등 다른 시각적 업데이트는 Update에서 호출될 수 있습니다.



}