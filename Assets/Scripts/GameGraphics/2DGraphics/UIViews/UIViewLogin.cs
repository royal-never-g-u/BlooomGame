using UnityEngine;

namespace GameGraphics
{
	public class UIViewLogin : UIViewLoginBase
	{
        protected override void OnEnter(object data = null)
        {
            base.OnEnter(data);
        }
        protected override void OnButtonStartClick()
        {
            base.OnButtonStartClick();
            Close();
            GameObject.Find("Person").AddComponent<PersonComponent>();
        }
    }
}
