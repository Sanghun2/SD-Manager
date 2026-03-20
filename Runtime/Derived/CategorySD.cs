using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace BilliotGames
{
    [CreateAssetMenu(fileName = "CategorySD", menuName = "Scriptable Objects/CategorySD")]
    public class CategorySD : ImageSDBase
    {
        public IReadOnlyList<CategorySD> Children => childrenCategories;
        [SerializeField] CategorySD[] childrenCategories;

#if UNITY_EDITOR
        [NonSerialized] private CategorySD[] prevChildrenCategories;
        [NonSerialized] private List<CategorySD> prevCategoryList;

        protected virtual void OnValidate() {
            RenameAsset(id, suffix: "_CategorySD");

            bool childrenChanged = !ArrayEquals(prevChildrenCategories, childrenCategories);
            bool categoryListChanged = !ListEquals(prevCategoryList, categoryList);

            if (childrenChanged || categoryListChanged) {
                CheckCategoryValidation();
                prevChildrenCategories = childrenCategories?.ToArray();
                prevCategoryList = categoryList?.ToList();
            }
        }

        private void CheckCategoryValidation() {
            var allCategories = AssetDatabase.FindAssets("t:CategorySD")
                .Select(guid => AssetDatabase.LoadAssetAtPath<CategorySD>(
                    AssetDatabase.GUIDToAssetPath(guid)))
                .Where(c => c != null)
                .ToList();

            // 1. childrenCategories 기준: 자식의 categoryList에 this 등록
            foreach (var child in childrenCategories) {
                if (child == null || child == this) continue;
                if (!child.categoryList.Contains(this)) {
                    child.categoryList.Add(this);
                    EditorUtility.SetDirty(child);
                }
            }

            // 2. childrenCategories에서 빠진 자식의 categoryList에서 this 제거
            foreach (var category in allCategories) {
                if (category == null || category == this) continue;
                if (!childrenCategories.Contains(category) && category.categoryList.Contains(this)) {
                    category.categoryList.Remove(this);
                    EditorUtility.SetDirty(category);
                }
            }

            // 3. categoryList 기준: 부모의 childrenCategories에 this 등록 (자식→부모 방향)
            foreach (var parent in categoryList) {
                if (parent == null || parent == this) continue;
                if (!parent.childrenCategories.Contains(this)) {
                    parent.childrenCategories = parent.childrenCategories.Append(this).ToArray();
                    EditorUtility.SetDirty(parent);
                }
            }

            // 4. categoryList에서 빠진 부모의 childrenCategories에서 this 제거
            foreach (var category in allCategories) {
                if (category == null || category == this) continue;
                if (!categoryList.Contains(category) && category.childrenCategories.Contains(this)) {
                    category.childrenCategories = category.childrenCategories.Where(c => c != this).ToArray();
                    EditorUtility.SetDirty(category);
                }
            }
        }

        private bool ArrayEquals(CategorySD[] a, CategorySD[] b) {
            if (a == null && b == null) return true;
            if (a == null || b == null) return false;
            if (a.Length != b.Length) return false;
            for (int i = 0; i < a.Length; i++)
                if (a[i] != b[i]) return false;
            return true;
        }

        private bool ListEquals(List<CategorySD> a, List<CategorySD> b) {
            if (a == null && b == null) return true;
            if (a == null || b == null) return false;
            if (a.Count != b.Count) return false;
            for (int i = 0; i < a.Count; i++)
                if (a[i] != b[i]) return false;
            return true;
        }
#endif
    }
}