using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class SpellDatabaseManager : EditorWindow {
    

    [MenuItem("Data/Spell Directed Creator")]
    static void Init()
    {
        SpellDatabaseManager window = (SpellDatabaseManager)EditorWindow.GetWindow(typeof(SpellDatabaseManager));
        // Creation of Spell manager database.
        window.Show();
    }

    string spell_name = "";
    string spell_description = "";
    int spell_value = 0;
    GameObject spell_projectile;
    float spell_castTime = 0.0f;
    float spell_cooldown = 0.0f;
    enum Spell_Type
    {
        SpellDirected,
        SpellAreaOfEffect,
    }
    Spell_Type type = Spell_Type.SpellDirected;
 
    void OnGUI()
    {
        spell_name = EditorGUILayout.TextField("Name :", spell_name);
        spell_description = EditorGUILayout.TextField("Spell description :", spell_description);
        type = (Spell_Type)EditorGUILayout.EnumPopup("Spell type :", type);
        spell_value = EditorGUILayout.IntField("Value :", spell_value);
        spell_projectile = (GameObject)EditorGUILayout.ObjectField("Model :", spell_projectile, typeof(GameObject), true);
        spell_castTime = EditorGUILayout.FloatField("Cast time :", spell_castTime);
        spell_cooldown = EditorGUILayout.FloatField("Cooldown :", spell_cooldown);
        if(GUILayout.Button("Add Spell"))
        {
            if(spell_name == "")
            {
                Debug.LogError("You must enter a valid name.");
            }
            else
            {
                Debug.Log("coucou");
                SpellDirected NewSpell = ScriptableObject.CreateInstance<SpellDirected>() as SpellDirected;
                NewSpell.name = spell_name;
                NewSpell.description = spell_description;
                NewSpell.castTime = spell_castTime;
                NewSpell.cooldown = spell_cooldown;
                NewSpell.projectile = spell_projectile;
                NewSpell.value = spell_value;
                AssetDatabase.CreateAsset(NewSpell, "Assets/Prefabs/Abilities/" + NewSpell.name + ".asset");
                AssetDatabase.SaveAssets();
            }
        }
    }
}
