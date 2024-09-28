using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DontFall.Editor
{
    [CustomEditor(typeof(BoardController))]
    public class BoardControllerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if(GUILayout.Button("Set Board"))
            {
                var board = (BoardController)target;
                board.SetBoard();
            }
        }
    }
}
