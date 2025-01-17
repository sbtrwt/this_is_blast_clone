using UnityEditor;
using UnityEngine;
using Blaster.Level;
using Blaster.Targets;
using System.Collections.Generic;

[CustomEditor(typeof(LevelSO))]
public class LevelSOEditor : Editor
{
    private LevelSO levelSO;

    private void OnEnable()
    {
        levelSO = (LevelSO)target;

        // Ensure the TargetTypes list is initialized and matches the grid
        if (levelSO.TargetTypes == null)
        {
            levelSO.TargetTypes = new List<TargetData>();
        }

        levelSO.OnValidate();
    }

    public override void OnInspectorGUI()
    {
        // Draw default fields
        DrawDefaultInspector();

        // Draw TargetTypes grid
        DrawTargetTypesGrid();

        // Save changes
        if (GUI.changed)
        {
            EditorUtility.SetDirty(levelSO);
        }
    }

    private void DrawTargetTypesGrid()
    {
        GUILayout.Label("Target Types Grid", EditorStyles.boldLabel);

        // Loop through rows and columns
        for (int row = 0; row < levelSO.Rows; row++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int column = 0; column < levelSO.Columns; column++)
            {
                // Find the TargetData for this position
                int index = levelSO.TargetTypes.FindIndex(t => t.X == column && t.Y == row);
                if (index >= 0)
                {
                    var targetData = levelSO.TargetTypes[index];
                    targetData.TargetType = (TargetType)EditorGUILayout.ObjectField(
                        targetData.TargetType,
                        typeof(TargetType),
                        false,
                        GUILayout.Width(100)
                    );

                    // Update the TargetData entry in the list
                    levelSO.TargetTypes[index] = targetData;
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
