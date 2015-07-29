using UnityEngine;
using UnityEditor;
using System.Collections;


[CustomEditor(typeof(JobPattern)), CanEditMultipleObjects]
public class JobEditor : Editor {
	[MenuItem("Creation/Create new job")]
	static void CreateJob(){
		string path = EditorUtility.SaveFilePanel ("Create new job", 
		        "Assets/Prefabs/Patterns/Jobs/", "default.asset", "asset");

		if (path == "")
			return;

		path = FileUtil.GetProjectRelativePath (path);

		JobPattern jp = CreateInstance<JobPattern> ();
		AssetDatabase.CreateAsset (jp, path);
		AssetDatabase.SaveAssets ();
	}

	public override void OnInspectorGUI(){
	
		if (Event.current.type == EventType.Layout)
			return;
		Rect position = new Rect (0, 50, Screen.width, Screen.height - 50);

		foreach (var item in targets) {
			if(position.height<EditorGUIUtility.singleLineHeight * 2)
				continue;

			JobPattern jobPattern = item as JobPattern;
			position.x +=  InspectorJobPattern(position, jobPattern);    
		}
	}

	public static float InspectorJobPattern(Rect position, JobPattern jp){
		GUI.changed = false;
		int cc = EditorGUI.IntField(new Rect(position.x, 
		                                           position.y, 
		                                           position.width * 0.5f, 
		                                           EditorGUIUtility.singleLineHeight), 
		                                  			"Critical", jp.m_cc);
		
		int frag = EditorGUI.IntField(new Rect(position.x + position.width * 0.5f, 
		                                            position.y, 
		                                            position.width * 0.5f,
		                                            EditorGUIUtility.singleLineHeight), 
		                                   			"Fragmentation", jp.m_frag);
		int force = EditorGUI.IntField(new Rect(position.x, 
		                                     	position.y+25, 
		                                        position.width * 0.5f, 
		                                        EditorGUIUtility.singleLineHeight), 
		                                  		"Force", jp.m_force);
		
		int res = EditorGUI.IntField(new Rect(position.x + position.width * 0.5f, 
		                                            position.y+25, 
		                                            position.width * 0.5f,
		                                            EditorGUIUtility.singleLineHeight), 
		                                   "Resistance", jp.m_res);

		int vit = EditorGUI.IntField(new Rect(position.x, 
		                                        position.y+50, 
		                                        position.width * 0.5f, 
		                                        EditorGUIUtility.singleLineHeight), 
		                               "Vitality", jp.m_vit);
		
		int level = EditorGUI.IntField(new Rect(position.x + position.width * 0.5f, 
		                                      position.y+50, 
		                                      position.width * 0.5f,
		                                      EditorGUIUtility.singleLineHeight), 
		                             "Level", jp.m_level);
		int diffuser = EditorGUI.IntField(new Rect(position.x, 
		                                             position.y+75, 
		                                             position.width * 0.5f, 
		                                             EditorGUIUtility.singleLineHeight), 
		                                    "Diffuser", jp.m_diffuser);

		string displayName = EditorGUI.TextField(new Rect(position.x, 
		                                                  position.y+100, 
		                                                  position.width, 
		                                                  EditorGUIUtility.singleLineHeight),  
		                                         "Display Name", jp.m_displayName);

		position.y += EditorGUIUtility.singleLineHeight;
		float usedHeight = 0.0f;
		usedHeight += EditorGUIUtility.singleLineHeight;

		jp.m_cc = cc;
		jp.m_force = force;
		jp.m_frag = frag;
		jp.m_res = res;
		jp.m_vit = vit;
		jp.m_level = level;
		jp.m_diffuser = diffuser;
		jp.m_displayName = displayName;

		if (GUI.changed)
			EditorUtility.SetDirty(jp);
		
		return usedHeight;
	}
}


