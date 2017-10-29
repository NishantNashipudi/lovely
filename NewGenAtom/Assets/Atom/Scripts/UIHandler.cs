using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Atom
{
	[System.Serializable]
	public class QuizData{
		public string question;
		public bool istrue;
	}

	public class UIHandler : MonoBehaviour {

		//---- Singleton ---
		private static UIHandler instance;
		public static UIHandler Instance {
			get {
				if (instance == null)
					instance = GameObject.FindObjectOfType<UIHandler> ();

				return instance;
			}
		}

		public enum Infomodes{e_default, e_atom, e_electricity};

		public List<QuizData> g_data;
		public Infomodes g_infomode;

		public GameObject g_RadialMenubutton;
		public GameObject g_RadialMenu;

		public GameObject g_protonpanel;
		public GameObject g_neutronpanel;
		public GameObject g_electornpanel;
		public GameObject g_atomPartsInfo;

		public GameObject g_screenatomparent;
		public GameObject g_imageatomparent;
		public GameObject g_atom;
		public GameObject g_wire;
		public GameObject g_imageAtom;
		public ParticleSystem[] g_wireparticles;
		public GameObject[] g_wireatomanimation;
		public GameObject g_atomanimation;
		public GameObject g_imageAtomanimation;
		public Text g_scrollText;

		public GameObject g_HelpTextpanel;
		public GameObject g_Playbutton;

		public GameObject g_Quizpanel;
		public Text g_QuizQuestion;
		public Button g_quiztrue;
		public Button g_quizfalse;
		public GameObject g_quizresult;
		public GameObject yessprite;
		public GameObject nosprite;
		public GameObject g_feedback;

		public Sprite g_playsprite;
		public Sprite g_pausesprite;
		public Sprite g_Happy;
		public Sprite g_Normal;
		public Sprite g_Sad;
		public Sprite g_tick;
		public Sprite g_cross;

		public GameObject introTxt;

		int g_currentquesttion;
		bool g_radialToggel;
		bool g_play;
		bool g_atomparts;
		int g_score;
		Vector3 g_atominitpos;
		Vector3 g_atominitrot;
		Vector3 g_wireinitpos;
		Vector3 g_wireinitrot;
		Animator g_radialAnim;

		void Start () {
			for (int i = 0; i < g_wireparticles.Length; i++) {
				g_wireparticles [i].Pause ();
			}
			g_atom.SetActive (false);
			g_wire.SetActive (false);
			g_imageAtom.SetActive (false);
			g_atominitpos = g_atom.transform.localPosition;
			g_atominitrot = g_atom.transform.localEulerAngles;
			g_wireinitpos = g_wire.transform.localPosition;
			g_wireinitrot = g_wire.transform.localEulerAngles;
			g_infomode = Infomodes.e_default;
			g_currentquesttion = 0;
			g_play = false;
			g_atomparts = false;
			g_radialToggel = false;
			g_radialAnim = g_RadialMenubutton.GetComponent<Animator> ();
			g_Playbutton.GetComponent<Image> ().sprite = g_playsprite;
			Invoke("m_CloseIntro",5);
		}

		public void m_IntroClose(){
			CancelInvoke ("m_CloseIntro");
			m_CloseIntro ();
		}

		void m_CloseIntro(){
			g_RadialMenu.transform.parent.gameObject.SetActive (true);
			DisableObjects (introTxt, 1);
			iTween.ValueTo (introTxt, iTween.Hash ("from", 1, "to", 0, "time", 1f, "onupdate", "m_UpdateValue"));
		}

		public void m_Home(){
			SceneManager.LoadScene (0);
		}

		public void m_RadialMenuToggel(){
			if (g_radialToggel) {
				g_RadialMenu.transform.GetChild (g_RadialMenu.transform.childCount - 1).gameObject.SetActive (true);
				g_radialAnim.SetTrigger ("Close");
				iTween.ValueTo (g_RadialMenu, iTween.Hash ("from", 1, "to", 0, "time", 1f, "onupdate", "m_UpdateValue"));
				g_radialToggel = false;
			} else {
				if (g_atomparts) {
					m_AtomPartsInfo ();
				}
				g_RadialMenu.transform.GetChild (g_RadialMenu.transform.childCount - 1).gameObject.SetActive (false);
				g_radialAnim.SetTrigger ("Open");
				iTween.ValueTo (g_RadialMenu, iTween.Hash ("from", 0, "to", 1, "time", .5f, "onupdate", "m_UpdateValue"));
				g_radialToggel = true;
			}
		}

		public void m_AtomScene()
		{
			g_infomode = Infomodes.e_atom;
			g_atom.SetActive (true);
			g_wire.SetActive (false);
			g_imageAtom.SetActive (false);
			if (g_play) {
				m_PlayAnimationToggel ();
			} 
			if (g_radialToggel) {
				m_RadialMenuToggel ();
			}
		}

		public void SetImageParent(){
			g_imageAtom.transform.parent = g_imageatomparent.transform;
			g_imageAtom.transform.localPosition = Vector3.zero;
			g_imageAtom.transform.localEulerAngles = Vector3.zero;
			g_imageAtom.transform.localScale = Vector3.one;
		}
		public void SetScreenParent(){
			g_imageAtom.transform.parent = g_screenatomparent.transform;
			g_imageAtom.transform.localPosition = Vector3.zero;
			g_imageAtom.transform.localEulerAngles = Vector3.zero;
			g_imageAtom.transform.localScale = Vector3.one;
		}
		public void m_WireScene()
		{
			g_infomode = Infomodes.e_electricity;
			g_atom.SetActive (false);
			g_wire.SetActive (true);
			g_imageAtom.SetActive (false);
			if (g_play) {
				m_PlayAnimationToggel ();
			} 
			if (g_radialToggel) {
				m_RadialMenuToggel ();
			}
		}

		public void m_ResetProject(){
			g_infomode = Infomodes.e_default;
			g_atom.SetActive (false);
			g_wire.SetActive (false);
			g_imageAtom.SetActive (true);
			if (g_play) {
				m_PlayAnimationToggel ();
			}
			if (g_radialToggel) {
				m_RadialMenuToggel ();
			}

		}


		public void m_OpenHelpTextPanel(){
			if (g_atomparts) {
				m_AtomPartsInfo ();
			}
			g_HelpTextpanel.SetActive (true);
			iTween.ValueTo(g_HelpTextpanel,iTween.Hash("from",0,"to",1,"time",1f,"onupdate", "m_UpdateValue"));
		}
		public void m_CloseHelpTextPanel(){
			DisableObjects (g_HelpTextpanel, 1);
			iTween.ValueTo(g_HelpTextpanel,iTween.Hash("from",1,"to",0,"time",1f,"onupdate", "m_UpdateValue"));
		}



		public void m_AtomPartsInfo(){
			switch (g_infomode) {

				case Infomodes.e_default:
					if (!g_atomparts) {
						if (g_radialToggel) {
							m_RadialMenuToggel ();
						}
						g_atomPartsInfo.GetComponent<Animator> ().SetTrigger ("open");
						g_atomparts = true;
					} else {
						g_atomPartsInfo.GetComponent<Animator> ().SetTrigger ("close");
						g_atomparts = false;
					}
					break;


				case Infomodes.e_atom:
					if (!g_atomparts) {
						if (g_radialToggel) {
							m_RadialMenuToggel ();
						}
						g_atom.transform.localPosition = Vector3.zero;
						g_atom.transform.localEulerAngles = Vector3.zero;
						g_scrollText.text = "Atoms are the building blocks of matter.An atom has protons, neutrons and electrons.\n\n Protons and neutrons are found in the middle of the   atom. Electrons orbit the atom.";
						g_scrollText.gameObject.transform.parent.gameObject.SetActive (true);
						iTween.ValueTo(g_scrollText.gameObject.transform.parent.gameObject,iTween.Hash("from",0,"to",1,"time",1f,"onupdate", "m_UpdateValue"));
						g_atomparts = true;
					} else {
						g_atom.transform.localPosition = g_atominitpos;
						g_atom.transform.localEulerAngles = g_atominitrot;
						DisableObjects(g_scrollText.gameObject.transform.parent.gameObject,1);
						iTween.ValueTo(g_scrollText.gameObject.transform.parent.gameObject,iTween.Hash("from",1,"to",0,"time",1f,"onupdate", "m_UpdateValue"));
						g_atomparts = false;
					}
					break;


				case Infomodes.e_electricity:
					if (!g_atomparts) {
						if (g_radialToggel) {
							m_RadialMenuToggel ();
						}
						g_scrollText.text = "Protons and electrons are attracted to each other.\n\nElectricity is generated when electrons move from one atom to\ranother. The flow of electrons is called electricity.";
						g_scrollText.gameObject.transform.parent.gameObject.SetActive (true);
						iTween.ValueTo(g_scrollText.gameObject.transform.parent.gameObject,iTween.Hash("from",0,"to",1,"time",1f,"onupdate", "m_UpdateValue"));
						g_wire.transform.localPosition = Vector3.zero;
						g_wire.transform.localEulerAngles = Vector3.zero;
						g_atomparts = true;
					} else {
						g_wire.transform.localPosition = g_wireinitpos;
						g_wire.transform.localEulerAngles = g_wireinitrot;
						
						DisableObjects(g_scrollText.gameObject.transform.parent.gameObject,1);
						iTween.ValueTo(g_scrollText.gameObject.transform.parent.gameObject,iTween.Hash("from",1,"to",0,"time",1f,"onupdate", "m_UpdateValue"));
						g_atomparts = false;
					}
					break;
			}

		}

		public void m_AtomPartsDescriptionOpen(int val){
			if (val == 0) 
			{
				g_protonpanel.SetActive (true);
				iTween.ValueTo(g_protonpanel,iTween.Hash("from",0,"to",1,"time",1f,"onupdate", "m_UpdateValue"));
			}
			else if (val ==1)
			{
				g_neutronpanel.SetActive (true);
				iTween.ValueTo(g_neutronpanel,iTween.Hash("from",0,"to",1,"time",1f,"onupdate", "m_UpdateValue"));
			}
			else if(val ==2)
			{
				g_electornpanel.SetActive (true);
				iTween.ValueTo(g_electornpanel,iTween.Hash("from",0,"to",1,"time",1f,"onupdate", "m_UpdateValue"));
			}
		}

		public void m_AtomPartsDescriptionClose(int val){
			if (val == 0) 
			{
				DisableObjects (g_protonpanel, 1);
				iTween.ValueTo(g_protonpanel,iTween.Hash("from",1,"to",0,"time",1f,"onupdate", "m_UpdateValue"));
			}
			else if (val ==1)
			{
				DisableObjects (g_neutronpanel, 1);
				iTween.ValueTo(g_neutronpanel,iTween.Hash("from",1,"to",0,"time",1f,"onupdate", "m_UpdateValue"));
			}
			else if(val ==2)
			{
				DisableObjects (g_electornpanel, 1);
				iTween.ValueTo(g_electornpanel,iTween.Hash("from",1,"to",0,"time",1f,"onupdate", "m_UpdateValue"));
			}
		}



		public void m_PlayAnimationToggel(){
			if (!g_play) {
				g_play = true;
				g_Playbutton.GetComponent<Image> ().sprite = g_pausesprite;
				switch (g_infomode) {
				case Infomodes.e_default:
						g_imageAtomanimation.GetComponent<Animator> ().SetTrigger ("atom");
					break;
					case Infomodes.e_atom:
						g_atomanimation.GetComponent<Animator> ().SetTrigger ("atom");
					break;
				case Infomodes.e_electricity:
					for (int i = 0; i < g_wireparticles.Length; i++) {
						g_wireparticles [i].Play ();
					}
					for (int i = 0; i < g_wireatomanimation.Length; i++) {
						g_wireatomanimation[i].GetComponent<Animator> ().SetTrigger ("atom");
					}
					break;
				}
			} else {
				g_play = false;
				g_Playbutton.GetComponent<Image> ().sprite = g_playsprite;
				switch (g_infomode) {
					case Infomodes.e_default:
						g_imageAtomanimation.GetComponent<Animator> ().SetTrigger ("pause");
					break;
					case Infomodes.e_atom:
						g_atomanimation.GetComponent<Animator> ().SetTrigger ("pause");
					break;
					case Infomodes.e_electricity:
						for (int i = 0; i < g_wireparticles.Length; i++) {
							g_wireparticles [i].Pause ();
						}
						for (int i = 0; i < g_wireatomanimation.Length; i++) {
							g_wireatomanimation[i].GetComponent<Animator> ().SetTrigger ("pause");
						}
					break;
				}
			}
		}




		public void m_StartQuiz()
		{
			g_quizresult.GetComponent<CanvasGroup> ().alpha = 0;
			g_quizresult.SetActive (false);
			g_atom.SetActive (false);
			g_wire.SetActive (false);
			g_imageAtom.SetActive (false);
			if (g_play) {
				m_PlayAnimationToggel ();
			} 
			m_RadialMenuToggel ();
			g_Quizpanel.SetActive (true);
			iTween.ValueTo(g_Quizpanel,iTween.Hash("from",0,"to",1,"time",1f,"onupdate", "m_UpdateValue"));
			g_score = 0;
			g_currentquesttion = 0;
			StartCoroutine(c_WriteQuestion(g_currentquesttion));
			g_quiztrue.GetComponent<Button> ().interactable = false;
			g_quizfalse.GetComponent<Button> ().interactable = false;
		}

		public void m_CloseQuiz(){
			
			DisableObjects (g_Quizpanel, 1);
			iTween.ValueTo(g_Quizpanel,iTween.Hash("from",1,"to",0,"time",1f,"onupdate", "m_UpdateValue"));
			m_ResetProject ();
		}

		IEnumerator c_WriteQuestion(int index){

			if (g_currentquesttion < g_data.Count) {
				if (index > 0) {
					yield return new WaitForSeconds (3);
					g_feedback.SetActive (false);
					yessprite.SetActive (false);
					nosprite.SetActive (false);
				}
				g_QuizQuestion.text = "";
				for (int i = 0; i < g_data [index].question.Length; i++) {
					g_QuizQuestion.text += g_data [index].question [i];
					yield return new WaitForSeconds (0.01f);
				}
				g_currentquesttion++;
				g_quiztrue.GetComponent<Button> ().interactable = true;
				g_quizfalse.GetComponent<Button> ().interactable = true;
			} else {
				g_feedback.SetActive (false);
				g_quizresult.SetActive (true);
				iTween.ValueTo(g_quizresult,iTween.Hash("from",0,"to",1,"time",1f,"onupdate", "m_UpdateValue"));
				if (g_score <= Mathf.Round ((g_data.Count * 30) / 100)) {
					g_quizresult.transform.GetChild (0).GetComponent<Image> ().sprite = g_Sad;
					g_quizresult.transform.GetChild (3).GetComponent<Text> ().text = "Result : 0" + g_score;
				}else if (g_score <= Mathf.Round ((g_data.Count * 70) / 100) && g_score > Mathf.Round ((g_data.Count * 30) / 100)) {
					g_quizresult.transform.GetChild (0).GetComponent<Image> ().sprite = g_Normal;
					g_quizresult.transform.GetChild (3).GetComponent<Text> ().text = "Result : 0" + g_score;
				}else if (g_score > Mathf.Round ((g_data.Count * 70) / 100)){
					g_quizresult.transform.GetChild (0).GetComponent<Image> ().sprite = g_Happy;
					g_quizresult.transform.GetChild (3).GetComponent<Text> ().text = "Result : 0" + g_score;
				}
				//yield return new WaitForSeconds (5);
				yessprite.SetActive (false);
				nosprite.SetActive (false);
				g_QuizQuestion.text = "";
			}
		}

		public void m_QuizAnswer (bool ans){
			g_quizfalse.interactable = false;g_quiztrue.interactable = false;
			if (ans == g_data [g_currentquesttion-1].istrue) {
				g_feedback.GetComponent<Image> ().sprite = g_tick;
				g_feedback.SetActive (true);

				if (g_data [g_currentquesttion - 1].istrue) {
					yessprite.SetActive (true);
					iTween.PunchScale (g_quiztrue.gameObject, new Vector3 (0.5f, 0.2f, 0.2f), 1.5f);
				} else {
					nosprite.SetActive (true);
					iTween.PunchScale (g_quizfalse.gameObject, new Vector3 (0.5f, 0.2f, 0.2f), 1.5f);
				}
				g_score++;

				StartCoroutine(c_WriteQuestion(g_currentquesttion));
				g_quiztrue.GetComponent<Button> ().interactable = false;
				g_quizfalse.GetComponent<Button> ().interactable = false;
			} else {
				Handheld.Vibrate ();
				g_feedback.GetComponent<Image> ().sprite = g_cross;
				g_feedback.SetActive (true);

				if (!g_data [g_currentquesttion - 1].istrue) {
					nosprite.SetActive (true);
					iTween.ShakeRotation (g_quiztrue.gameObject, new Vector3 (0.5f, 0.2f, 3.5f), 1.5f);
				} else {
					yessprite.SetActive (true);
					iTween.ShakeRotation (g_quizfalse.gameObject, new Vector3 (0.5f, 0.2f, 3.5f), 1.5f);
				}
				StartCoroutine(c_WriteQuestion(g_currentquesttion));
				g_quiztrue.GetComponent<Button> ().interactable = false;
				g_quizfalse.GetComponent<Button> ().interactable = false;
			}
		}




		void DisableObjects(GameObject l_obj,float l_time){
			StartCoroutine (e_Disable(l_obj, l_time));
		}
		IEnumerator e_Disable(GameObject l_obj,float l_time){
			yield return new WaitForSeconds (l_time);
			l_obj.SetActive (false);
		}
	}
}
