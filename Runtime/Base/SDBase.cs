using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace BilliotGames
{
    public abstract class SDBase : ScriptableObject
    {
        public string ID => id;
        public string DisplayName => displayName;
        public string Description => descripiton;

        [SerializeField] protected string id;
        [SerializeField] protected string displayName;
        [TextArea(1, 10)]
        [SerializeField] protected string descripiton;

        protected void RenameAsset(string newName, string prefix = null, string suffix = null) {
#if UNITY_EDITOR
            if (string.IsNullOrWhiteSpace(newName))
                return;

            string finalName = $"{prefix}{newName}{suffix}";

            // 금지 문자 제거
            foreach (char c in Path.GetInvalidFileNameChars())
                finalName = finalName.Replace(c, '_');

            string path = AssetDatabase.GetAssetPath(this);
            if (string.IsNullOrEmpty(path))
                return;

            string currentName = Path.GetFileNameWithoutExtension(path);

            // ⭐ 같으면 절대 실행하지 마라 (OnValidate 지옥 방지)
            if (currentName == finalName)
                return;

            EditorApplication.delayCall += () =>
            {
                if (this == null) return;

                string assetPath = AssetDatabase.GetAssetPath(this);
                if (string.IsNullOrEmpty(assetPath)) return;

                string error = AssetDatabase.RenameAsset(assetPath, finalName);

                if (!string.IsNullOrEmpty(error))
                    Debug.LogError($"Rename failed: {error}");

                AssetDatabase.SaveAssets();
            };
#endif
        }
    }
}
