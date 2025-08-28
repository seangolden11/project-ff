using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace FGUIStarter
{
    public class CustomButton : Button, IPointerDownHandler, IPointerUpHandler
    {
        RectTransform textRect;
        Vector2 originalTextPos;
        public GameObject LockPanel;

        public int id;

        public int mode;

        bool isLocked = false;
        public bool isCustom = false;

        bool isHeld;
        protected override void Awake()
        {
            base.Awake();
            textRect = GetComponentInChildren<TextMeshProUGUI>().rectTransform;
            originalTextPos = textRect.anchoredPosition;
            if (GetComponentsInChildren<TextMeshProUGUI>().Length >= 2)
            {
                isCustom = true;
            }
        }

        public override void OnPointerDown(PointerEventData eventData)
        {

            base.OnPointerDown(eventData);
            isHeld = true;
            ApplyPressedVisual();

        }

        public override void OnPointerUp(PointerEventData eventData)
        {

            base.OnPointerUp(eventData);
            isHeld = false;
            ApplyNormalVisual();

        }

        private void ApplyPressedVisual()
        {
            if (textRect != null && isCustom == false)
            {
                float height = ((RectTransform)transform).rect.height;
                float offset = height - (height * 0.86718f);//calculation for 128x128 sprite
                //use this code below instead of the code in line 40, in case the offset of the text doesn't make sense with respect to the thickness of the button or gameview dimensions:
                //float offset = height - (height * 0.86718f) - insertYourCustomOffset;
                //Example: float offset = height - (height * 0.86718f) - 10f;
                textRect.anchoredPosition = originalTextPos - new Vector2(0, offset);
            }
        }

        private void ApplyNormalVisual()
        {
            if (textRect != null && isCustom == false)
            {
                textRect.anchoredPosition = originalTextPos;
            }
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);
            if (state == SelectionState.Pressed)
            {
                ApplyPressedVisual();
            }

            else
            {
                ApplyNormalVisual();
            }
        }



        public void OnClick()
        {
            if (isLocked)
                return;
            if (GetComponentInParent<EmployeeManager>())
                    GetComponentInParent<EmployeeManager>().ButtonPress(id, mode);
                else if (GetComponentInParent<UpgradePanel>())
                    GetComponentInParent<UpgradePanel>().Upgrade(id);
                else if (GetComponentInParent<JobManager>())
                    GetComponentInParent<JobManager>().ButtonPress(id, mode);
        }

        public void Lock()
        {
            if (LockPanel)
            {
                LockPanel.SetActive(true);
                isLocked = true;
            }
        }
        public void UnLock()
        {
            if (LockPanel)
            {
                LockPanel.SetActive(false);
                isLocked = false;
            }
        }

    }
}
