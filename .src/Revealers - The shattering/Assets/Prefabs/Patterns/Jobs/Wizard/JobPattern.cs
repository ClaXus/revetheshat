using UnityEngine;
using System.Collections;

public class JobPattern : ScriptableObject {
	public int m_frag;
	public int m_cc;
	public int m_force;
	public int m_res;
	public int m_vit;
	public int m_level;
	public int m_diffuser;
	public string m_displayName;
	public string m_associateSpell;

	public JobPattern(){
		m_frag = 1;
		m_cc = 1;
		m_force = 1;
		m_res = 1;
		m_vit = 1;
		m_level = 1;
		m_diffuser = -1;
		m_displayName = "Noname";
		m_associateSpell = "Nothing";
	}


}
