using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VirtualKeyboard;

/// <summary>
/// ����� �������� ����������� ����������
/// </summary>
public class KeyboardController : MonoBehaviour
{
    [SerializeField]
    private InputField attachedInputField;

    private KeyboardLayoutsController keyboardLayoutsController;
    private SoundController soundController;
    private KeyboardJoystickInputMode keyboardJoystickInputMode;
    private KeyboardKey[] keyboardKeys;

    private string currentText;
    private int textLength;
    private int caretPosition;
    private bool caretAtEnd;

    private void Awake()
    {
        keyboardLayoutsController = GetComponent<KeyboardLayoutsController>();
        soundController = GetComponent<SoundController>();
        keyboardJoystickInputMode = GetComponent<KeyboardJoystickInputMode>();
        keyboardKeys = FindObjectsOfType<KeyboardKey>(true);
    }

    private void Start()
    {
        foreach (var key in keyboardKeys)
        {
            key.KeyInitialization();
        }

        keyboardLayoutsController.LayoutStartSetup();
        attachedInputField.Select();
    }

    private void OnEnable()
    {
        KeyboardKey.onAnyKeyDown += PressKey;
    }

    private void OnDisable()
    {
        KeyboardKey.onAnyKeyDown -= PressKey;
    }
    /// <summary>
    /// ���������� ������� �������
    /// </summary>
    /// <param name="key"></param>
    public void PressKey(string key)
    {
        if (attachedInputField != null)
        {
            UpdateInputField(key);
        }
    }
    /// <summary>
    /// ���������� ������ � ���� �����
    /// </summary>
    /// <param name="key"></param>
    private void UpdateInputField(string key)
    {
        string formattedKey = key;

        currentText = attachedInputField.text;
        caretPosition = attachedInputField.caretPosition;
        textLength = currentText.Length;
        caretAtEnd = attachedInputField.isFocused == false || caretPosition == textLength;

        switch (formattedKey)
        {
            case "enter":
                HandleEnter();
                break;
            case "backspace":
                HandleBackspace();
                break;
            case "shift":
                HandleShift();
                break;
            case "space":
                HandleSpace();
                break;
            case "language":
                HandleLanguageLayoutSwap();
                break;
            case "symbols":
                HandleSymbolsLayoutSwap();
                break;
            case "letters":
                HandleLettersLayoutSwap();
                break;  
            default:
                HandleCharacterInput(formattedKey);
                break;
        }

        attachedInputField.text = currentText;
        attachedInputField.caretPosition = caretPosition;
        StartCoroutine(UpdateCaret());

        if (soundController != null)
        {
            soundController.PlayButtonClickSoundAt(transform.position);
        }

        //if (!attachedInputField.isFocused)
        //{
        //    attachedInputField.Select();
        //}
    }
    /// <summary>
    /// ��������� ������� ������ "Enter"
    /// </summary>
    private void HandleEnter()
    {
        HandleCharacterInput("\n");
    }
    /// <summary>
    /// ��������� ������� ������ "Backspace"
    /// </summary>
    private void HandleBackspace()
    {   
        if (caretPosition == 0)
        {
            soundController.PlayButtonClickSoundAt(transform.position);
            return;
        }

        currentText = currentText.Remove(caretPosition - 1, 1);

        if (!caretAtEnd)
        {
            MoveCaretBack();
        }
    }
    /// <summary>
    /// ��������� ������� ������ "Space"
    /// </summary>
    private void HandleSpace()
    {
        HandleCharacterInput(" ");
    }
    /// <summary>
    /// ��������� ������� ������ "Shift"
    /// </summary>
    private void HandleShift()
    {
        foreach (var key in keyboardKeys)
        {
            if (key != null)
            {
                key.ToggleShift();
            }
        }
    }

    /// <summary>
    /// ��������� ������� ������ ����� �����
    /// </summary>
    private void HandleLanguageLayoutSwap()
    {
        keyboardLayoutsController.ToggleLanguageLayout();
        keyboardJoystickInputMode.SwitchKeyboardLayout(keyboardLayoutsController.GetCurrentLayout());
    }
    /// <summary>
    /// ��������� ������� ������ ����� �� ���������� ���������
    /// </summary>
    private void HandleSymbolsLayoutSwap()
    {
        keyboardLayoutsController.SwapToSymbolsLayout();
        keyboardJoystickInputMode.SwitchKeyboardLayout(keyboardLayoutsController.GetCurrentLayout());
    }
    /// <summary>
    /// ��������� ������� ������ ����� �� ��������� ���������
    /// </summary>
    private void HandleLettersLayoutSwap()
    {
        keyboardLayoutsController.SwapToLettersLayout();
        keyboardJoystickInputMode.SwitchKeyboardLayout(keyboardLayoutsController.GetCurrentLayout());
    }

    /// <summary>
    /// ��������� ������� ������ �������� �������
    /// </summary>
    /// <param name="character"></param>
    private void HandleCharacterInput(string character)
    {
        if (caretAtEnd)
        {
            currentText += character;
        }
        else
        {
            string preText = "";
            if (caretPosition > 0)
            {
                preText = currentText.Substring(0, caretPosition);
            }

            string postText = currentText.Substring(caretPosition, textLength - preText.Length);
            currentText = preText + character + postText;          
        }
        MoveCaretUp();
    }
    
    /// <summary>
    /// ����������� ������� �����
    /// </summary>
    public void MoveCaretUp()
    {
        caretPosition += 1;
    }
    /// <summary>
    /// ����������� ������� �����
    /// </summary>
    public void MoveCaretBack()
    {
        caretPosition -= 1;
    }
    IEnumerator UpdateCaret()
    {
        yield return new WaitForEndOfFrame();
        attachedInputField.ForceLabelUpdate();
    }
}
