using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace DontFall.Editor
{
    [CustomEditor(typeof(BoardController))]
    public class BoardControllerEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset xml;

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new();

            xml.CloneTree(root);

            (root.Q("SetBoard") as Button).clicked += () => (target as BoardController).SetBoard();

            return root;
        }
    }
}
