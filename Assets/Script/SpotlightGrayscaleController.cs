using UnityEngine;
using UnityEngine.Rendering.Universal; // URP 네임스페이스 (선택사항)

public class SpotlightGrayscaleController : MonoBehaviour
{
    [Header("연결할 에셋")]
    [Tooltip("화면 비율에 맞춰 크기를 조절할 렌더 텍스처")]
    public RenderTexture maskRenderTexture;
    
    // 이전 프레임의 화면 크기를 저장할 변수
    private int lastScreenWidth = 0;
    private int lastScreenHeight = 0;

    void Start()
    {
        // 게임 시작 시 한 번 크기를 맞춰줌
        ResizeRenderTexture();
    }

    void Update()
    {
        // 현재 화면 크기가 이전 프레임과 다른지 확인
        if (Screen.width != lastScreenWidth || Screen.height != lastScreenHeight)
        {
            ResizeRenderTexture();
        }
    }

    void ResizeRenderTexture()
    {
        if (maskRenderTexture == null)
        {
            Debug.LogError("렌더 텍스처가 할당되지 않았습니다!");
            return;
        }

        // 기존 렌더 텍스처의 메모리를 해제 (중요!)
        if (maskRenderTexture.IsCreated())
        {
            maskRenderTexture.Release();
        }

        // 렌더 텍스처의 너비와 높이를 현재 화면 크기로 설정
        maskRenderTexture.width = Screen.width;
        maskRenderTexture.height = Screen.height;

        // 새로운 크기로 렌더 텍스처를 다시 생성
        maskRenderTexture.Create();
        
        // 현재 화면 크기 정보를 기록
        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;

        Debug.Log($"렌더 텍스처 크기 변경: {Screen.width}x{Screen.height}");
    }

    void OnDestroy()
    {
        // 씬이 파괴될 때 렌더 텍스처 메모리 해제
        if (maskRenderTexture != null && maskRenderTexture.IsCreated())
        {
            maskRenderTexture.Release();
        }
    }
}