using UnityEngine;

public class CharacterModelChanger : MonoBehaviour
{
    // 1. 인스펙터 창에서 모델 프리팹들을 연결할 배열
    public WorkerModel wm;

    // 2. 모델이 생성될 위치를 지정할 Transform (옵션이지만 추천)
    // 이 오브젝트의 자식으로 모델이 생성되어 깔끔하게 관리됩니다.
    public Transform modelContainer;

    // 3. 현재 생성되어 있는 모델을 추적하기 위한 변수
    private GameObject currentModelInstance;

    // 외부에서 이 함수를 호출하여 모델을 변경합니다.
    // 예를 들어, 캐릭터 ID (0, 1, 2...)를 데이터로 받습니다.
    public void ChangeCharacterModel(int modelID)
    {
        // --- 유효성 검사 ---
        // 요청된 ID가 배열 범위 안에 있는지 확인하여 오류를 방지합니다.
        if (modelID < 0 || modelID >= wm.models.Count)
        {
            Debug.LogError("잘못된 모델 ID입니다: " + modelID);
            return;
        }

        // --- 기존 모델 삭제 ---
        // 만약 이미 생성된 모델이 있다면 삭제합니다.
        if (currentModelInstance != null)
        {
            Destroy(currentModelInstance);
        }

        // --- 새 모델 생성 ---
        // 1. 배열에서 올바른 프리팹을 가져옵니다.
        GameObject prefabToInstantiate = wm.models[modelID];

        // 2. 프리팹을 실제로 씬에 생성(Instantiate)하고, 그 정보를 변수에 저장합니다.
        // modelContainer가 지정되었다면 그 위치와 방향으로 생성합니다.
        currentModelInstance = Instantiate(prefabToInstantiate, modelContainer.position, modelContainer.rotation);

        // 3. 생성된 모델을 modelContainer의 자식으로 만들어줍니다.
        // 이렇게 하면 플레이어가 움직일 때 모델도 함께 따라 움직입니다.
        currentModelInstance.transform.SetParent(modelContainer);
    }
}