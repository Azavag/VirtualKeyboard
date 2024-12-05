using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������� ����������
/// </summary>
public enum KeyboardLayout
{
    Rus,
    Eng,
    Sybmols
}
/// <summary>
/// �����, ���������� �� ���������� ����������� ����������
/// </summary>
public class KeyboardLayoutsController : MonoBehaviour
{   
    [SerializeField]
    private GameObject rusKeyboardObject;
    [SerializeField]
    private GameObject engKeyboardObject; 
    [SerializeField]
    private GameObject symbolsKeyboardObject;

    private Dictionary<KeyboardLayout, GameObject> keyboardLayouts = new Dictionary<KeyboardLayout, GameObject>();
    private KeyboardLayout currentLayout;
    private KeyboardLayout choosenLanguageLayout = KeyboardLayout.Rus;

    /// <summary>
    /// ��������� ������������� ���� ����������
    /// </summary>
    public void LayoutStartSetup()
    {
        LayoutsInitialization();

        DeactivateLayout(KeyboardLayout.Eng);
        DeactivateLayout(KeyboardLayout.Sybmols);

        ActivateLayout(choosenLanguageLayout);
    }

    /// <summary>
    /// ������������ �� ��������� ���������
    /// </summary>
    public void SwapToLettersLayout()
    {
        DeactivateLayout(currentLayout);
        ActivateLayout(choosenLanguageLayout);
    }
    /// <summary>
    /// ������������ �� ��������� ��������
    /// </summary>
    public void SwapToSymbolsLayout()
    {
        DeactivateLayout(currentLayout);
        ActivateLayout(KeyboardLayout.Sybmols);      
    }
    /// <summary>
    /// ������������ ����� ����������
    /// </summary>
    public void ToggleLanguageLayout()
    {
        DeactivateLayout(currentLayout);
        choosenLanguageLayout = choosenLanguageLayout == KeyboardLayout.Eng ? KeyboardLayout.Rus : KeyboardLayout.Eng;
        ActivateLayout(choosenLanguageLayout);
    }

    /// <summary>
    /// ��������� ���� ����������
    /// </summary>
    /// <param name="layout"></param>
    private void ActivateLayout(KeyboardLayout layout)
    {        
        currentLayout = layout;
        keyboardLayouts[layout].SetActive(true);
    }
    /// <summary>
    /// ���������� ���� ����������
    /// </summary>
    /// <param name="layout"></param>
    private void DeactivateLayout(KeyboardLayout layout)
    {
        keyboardLayouts[layout].SetActive(false);
    }

    private void LayoutsInitialization()
    {
        keyboardLayouts.Add(KeyboardLayout.Rus, rusKeyboardObject);
        keyboardLayouts.Add(KeyboardLayout.Eng, engKeyboardObject);
        keyboardLayouts.Add(KeyboardLayout.Sybmols, symbolsKeyboardObject);
    }

    public KeyboardLayout GetCurrentLayout()
    {
        return currentLayout;
    }


}
