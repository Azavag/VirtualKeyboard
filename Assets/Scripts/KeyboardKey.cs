using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace VirtualKeyboard
{
    /// <summary>
    /// ��� ������
    /// </summary>
    public enum KeyType
    { 
        Character,
        Symbol,
        System
    }

    /// <summary>
    /// �����, ���������� �� ��������� ������� ������ ����������
    /// </summary>
    public class KeyboardKey : MonoBehaviour
    {
        [SerializeField]
        private KeyType keyType;
        [Header("Keycodes")]
        [SerializeField]
        private string keycode;             
        private string uppercaseKeycode;    
        private string currentKeycode;      
        private bool isShiftState;          

        private Button keyButton;
        private TextMeshProUGUI keyButtonText;

        public static Action<string> onAnyKeyDown;

        private void OnValidate()
        {
            gameObject.name = $"KeyButton_{keycode}";
        }

        /// <summary>
        /// ���������� ������������� �������
        /// </summary>
        public void KeyInitialization()
        {
            keyButton = GetComponent<Button>();
            keyButtonText = GetComponentInChildren<TextMeshProUGUI>();

            if (keyType == KeyType.Character)
            {
                uppercaseKeycode = keycode.ToUpper();
            }
            currentKeycode = keycode;

            if (keyButton != null)
            {
                keyButton.onClick.AddListener(OnClickedKeyboardKey);
            }
            else
            {
                Debug.LogError($"�� ������� {currentKeycode} �� ��������� ������.");
            }
        }

        private void OnDestroy()
        {
            if (keyButton != null)
            {
                keyButton.onClick.RemoveListener(OnClickedKeyboardKey);
            }          
        }
        /// <summary>
        /// ������������ �������� �������
        /// </summary>
        public virtual void ToggleShift()
        {
            if(keyType == KeyType.Symbol || keyType == KeyType.System)
                return;

            isShiftState = !isShiftState;
            currentKeycode = isShiftState ? uppercaseKeycode : keycode;
            keyButtonText.text = currentKeycode;
        }
        /// <summary>
        /// ���������� ������� �� ������
        /// </summary>
        public void OnClickedKeyboardKey()
        {
            onAnyKeyDown?.Invoke(currentKeycode);
        }
    }
}
