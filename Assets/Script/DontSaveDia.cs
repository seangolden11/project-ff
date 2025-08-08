using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DontSaveDia : MonoBehaviour
{
#if UNITY_EDITOR
    void Update()
    {
        // 씬의 모든 오브젝트 컴포넌트 스캔
        foreach (var comp in FindObjectsOfType<Component>(true))
        {
            if (!comp) continue;
            var so = new SerializedObject(comp);
            var prop = so.GetIterator();
            while (prop.NextVisible(true))
            {
                if (prop.propertyType != SerializedPropertyType.ObjectReference) continue;
                var obj = prop.objectReferenceValue;
                if (!obj) continue;

                var hf = obj.hideFlags;
                // 에디터 저장 금지인데 직렬화 참조에 걸려 있으면 의심
                if ((hf & HideFlags.DontSaveInEditor) != 0)
                {
                    Debug.LogWarning(
                        $"[DontSaveDiagnostic] {comp.name}.{prop.displayName} -> {obj.name} has DontSaveInEditor",
                        comp
                    );
                }
            }
        }
    }
#endif
}
