using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

namespace DontCare.LevelSelect
{
    public class LevelSelectUI : MonoBehaviour
    {
        [SerializeField] private VisualTreeAsset itemXml;

        [SerializeField] private List<string> levelList;

        private void OnEnable()
        {
            var uiDocument = GetComponent<UIDocument>();

            var list = uiDocument.rootVisualElement.Q("LevelList");

            for(int i=0; i<levelList.Count; i++)
            {
                string level = levelList[i];

                var item = new VisualElement();
                itemXml.CloneTree(item);

                var button = item.Q("Button") as Button;
                button.text = $"#{i + 1}";
                button.clicked += () => SceneManager.LoadScene(level);

                list.Add(item);
            }
        }
    }
}
