using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform mainCameraTransform;

    void Start()
    {
        // 성능을 위해 시작할 때 메인 카메라의 Transform 정보를 한 번만 찾아 저장합니다.
        mainCameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        // LateUpdate는 카메라 이동이 모두 끝난 후에 호출되므로, 떨림 현상 없이 깔끔하게 카메라를 따라갑니다.
        // 그냥 카메라의 방향과 내 방향을 똑같이 만들어버립니다.
        transform.rotation = mainCameraTransform.rotation;
    }
}