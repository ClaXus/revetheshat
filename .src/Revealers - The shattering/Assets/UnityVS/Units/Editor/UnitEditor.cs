using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(Unit))]
public class UnitEditor : Editor {

    SerializedProperty transform;
    SerializedProperty characterController;
    SerializedProperty controlScript;
    SerializedProperty networkView;    
    List<SerializedProperty> unitCombat;
    List<SerializedProperty> unitMovement;
    List<SerializedProperty> unitDetails;

    string[] MovementProperties = {"_UnitType","_MoveType","_Speed","_JumpHeight","_FlightSpeed" };
    string[] CombatProperties = {"_IsDead","_MaxHp","_Hp", "_HpRegen", "_Shield", "_Target"};
    string[] DetailsProperties = {"_name", "_clan", "_title"};


    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        transform = serializedObject.FindProperty("_transform");
        characterController = serializedObject.FindProperty("_CharacterController");
        controlScript = serializedObject.FindProperty("_ControlScript");
        networkView = serializedObject.FindProperty("_Networkview");
        unitMovement = new List<SerializedProperty>();
        for(int i = 0; i < MovementProperties.Length; ++i)
        {
            unitMovement.Add(serializedObject.FindProperty(MovementProperties[i]));
        }
        unitCombat = new List<SerializedProperty>();
        for(int i = 0; i < CombatProperties.Length; ++i)
        {
            unitCombat.Add(serializedObject.FindProperty(CombatProperties[i]));
        }
        unitDetails = new List<SerializedProperty>();
        for(int i = 0; i < DetailsProperties.Length; ++i)
        {
            unitDetails.Add(serializedObject.FindProperty(DetailsProperties[i]));
        }
        GUIStyle labelStyle = new GUIStyle();
        labelStyle.fontSize = 16;
        labelStyle.normal.textColor = Color.white;
        EditorGUILayout.Separator(); // ---------------------------------------------

        EditorGUILayout.LabelField("    Controls (mandatory)", labelStyle);
        EditorGUILayout.Separator(); // ---------------------------------------------
        EditorGUILayout.PropertyField(transform, true);
        EditorGUILayout.PropertyField(characterController, true);
        EditorGUILayout.PropertyField(controlScript, true);
        EditorGUILayout.Separator(); // ---------------------------------------------
        EditorGUILayout.LabelField("    Network (facultative)", labelStyle);
        EditorGUILayout.Separator(); // ---------------------------------------------
        EditorGUILayout.PropertyField(networkView, true);
        EditorGUILayout.Separator(); // ---------------------------------------------
        EditorGUILayout.LabelField("    Details", labelStyle);
        EditorGUILayout.Separator(); // ---------------------------------------------
        for (int i = 0; i < unitDetails.Count; ++i)
        {
            EditorGUILayout.PropertyField(unitDetails[i], true);
        }
        EditorGUILayout.Separator(); // ---------------------------------------------
        EditorGUILayout.LabelField("    Movement", labelStyle);
        EditorGUILayout.Separator(); // ---------------------------------------------
        for (int i = 0; i < unitMovement.Count; ++i)
        {
            EditorGUILayout.PropertyField(unitMovement[i], true);
        }
        EditorGUILayout.Separator(); //  --------------------------------------------
        EditorGUILayout.LabelField("    Combat", labelStyle);
        EditorGUILayout.Separator(); //  --------------------------------------------
        for (int i = 0; i < unitCombat.Count; ++i)
        {
            EditorGUILayout.PropertyField(unitCombat[i], true);
        }
        EditorGUILayout.Separator(); //  --------------------------------------------
        serializedObject.ApplyModifiedProperties();
    }
}
