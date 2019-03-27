using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TestProjectAppache
{
    public class ButtonStretch : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private Action pressTouchAction;
        private Action upTouchAction;

        private bool isPressing;

        public void Init(Action pressTouchAction, Action upTouchAction)
        {
            this.pressTouchAction = pressTouchAction;
            this.upTouchAction = upTouchAction;
            isPressing = false;
        }

        private void Update()
        {
            if (!isPressing) return;
            if (pressTouchAction != null) pressTouchAction();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isPressing = true;
          
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isPressing = false;
            if (upTouchAction != null) upTouchAction();
        }
    }
}
