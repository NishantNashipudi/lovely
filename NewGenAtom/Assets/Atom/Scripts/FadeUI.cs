using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Atom
{
	public class FadeUI : MonoBehaviour{
		public enum valueType
		{
			ImageColorAlpha, CanvasAlpha
		};

		public valueType values;

		void m_UpdateValue(float newvalue)
		{
			switch (values) {
			case valueType.ImageColorAlpha:
				this.gameObject.GetComponent<Image>().color = new Color (this.gameObject.GetComponent<Color> ().r, this.gameObject.GetComponent<Color> ().g, this.gameObject.GetComponent<Color> ().b, newvalue);
				break;
			case valueType.CanvasAlpha:
				this.gameObject.GetComponent<CanvasGroup> ().alpha = newvalue;
				break;
			}
		}

	}
}
