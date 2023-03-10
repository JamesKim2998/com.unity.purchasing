using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using static UnityEditor.Purchasing.UnityPurchasingEditor;

namespace UnityEditor.Purchasing
{
    /// <summary>
    /// Customer Editor class for the IAPButton. This class handle how the IAPButton should represent itself in the UnityEditor.
    /// </summary>
    [CustomEditor(typeof(IAPButton))]
    [CanEditMultipleObjects]
    public class IAPButtonEditor : Editor
    {
        private static readonly string[] excludedFields = new string[] { "m_Script", "onTransactionsRestored" };
        private static readonly string[] restoreButtonExcludedFields = new string[] { "m_Script", "consumePurchase", "onPurchaseComplete", "onPurchaseFailed", "titleText", "descriptionText", "priceText" };
        private const string kNoProduct = "<None>";

        private readonly List<string> m_ValidIDs = new List<string>();
        private SerializedProperty m_ProductIDProperty;

        /// <summary>
        /// Event trigger when IAPButton is enabled in the scene.
        /// </summary>
        public void OnEnable()
        {
            m_ProductIDProperty = serializedObject.FindProperty("productId");
        }

        /// <summary>
        /// Event trigger when trying to draw the IAPButton in the inspector.
        /// </summary>
        public override void OnInspectorGUI()
        {
            var button = (IAPButton)target;

            serializedObject.Update();

            if (button.buttonType == IAPButton.ButtonType.Purchase)
            {
                EditorGUILayout.LabelField(new GUIContent("Product ID:", "Select a product from the IAP catalog."));

                var catalog = ProductCatalog.LoadDefaultCatalog();

                m_ValidIDs.Clear();
                m_ValidIDs.Add(kNoProduct);
                foreach (var product in catalog.allProducts)
                {
                    m_ValidIDs.Add(product.id);
                }

                var currentIndex = string.IsNullOrEmpty(button.productId) ? 0 : m_ValidIDs.IndexOf(button.productId);
                var newIndex = EditorGUILayout.Popup(currentIndex, m_ValidIDs.ToArray());
                m_ProductIDProperty.stringValue = newIndex > 0 && newIndex < m_ValidIDs.Count ? m_ValidIDs[newIndex] : string.Empty;

                if (GUILayout.Button("IAP Catalog..."))
                {
                    ProductCatalogEditor.ShowWindow();
                }
            }

            DrawPropertiesExcluding(serializedObject, button.buttonType == IAPButton.ButtonType.Restore ? restoreButtonExcludedFields : excludedFields);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
