using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace DontFall.Transition
{
    [CustomEditor(typeof(TransitionManager))]
    public class TransitionManagerEditor : Editor
    {
        [SerializeField] private VisualTreeAsset document;

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new();

            document.CloneTree(root);

            var type = root.Q<EnumField>("TransitionType");

            type.RegisterValueChangedCallback((ev) =>
            {
                switch ((TransitionManager.TransitionType)ev.newValue)
                {
                    case TransitionManager.TransitionType.Linear:
                        root.Q("Linear").style.display = DisplayStyle.Flex;
                        root.Q("Circular").style.display = DisplayStyle.None;
                        break;
                    case TransitionManager.TransitionType.Circular:
                        root.Q("Linear").style.display = DisplayStyle.None;
                        root.Q("Circular").style.display = DisplayStyle.Flex;
                        break;
                }
            });

            switch ((TransitionManager.TransitionType)type.value)
            {
                case TransitionManager.TransitionType.Linear:
                    root.Q("Linear").style.display = DisplayStyle.Flex;
                    root.Q("Circular").style.display = DisplayStyle.None;
                    break;
                case TransitionManager.TransitionType.Circular:
                    root.Q("Linear").style.display = DisplayStyle.None;
                    root.Q("Circular").style.display = DisplayStyle.Flex;
                    break;
            }

            return root;
        }
    }
}
