using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VirtualKeyboard;
/// <summary>
/// �����, ���������� �� ������ ������ ����� � ���������
/// </summary>
public class KeyboardJoystickInputMode : MonoBehaviour
{
    [SerializeField]
    private KeyboardController keyboardController;
    [SerializeField]
    private SoundController soundController;

    // ̸����� ���� ���������
    [SerializeField]
    private float joystickDeadZone = 0.2f;      
    // �������� ����� ����������� ������� � ������ ������
    private float navigationCooldown = 0.2f;    
    // ����� ���������� ������������ ������
    private float lastNavigationTime;
    // ������ �������� � ���� ���������
    private Vector2 joystickInput;

    [Header("Keyboard layouts")]
    [SerializeField]
    private GameObject rusKeyboard; // ������-��������� ��� ������� ����������
    [SerializeField]
    private GameObject engKeyboard; // ������-��������� ��� ���������� ����������
    [SerializeField]
    private GameObject symbolsKeyboard;// ������-��������� ��� ���������� ����������
    [SerializeField]
    private GameObject setupButtonsObject; //������ - ��������� ��� ��������� ������

    private List<Button[]> currentLayout = new List<Button[]>(); // ������ �������� ��� ����� ����������
    private List<Button[]> rusLayout = new List<Button[]>(); // ������ �������� ��� ����� ����������
    private List<Button[]> engLayout = new List<Button[]>(); // ������ �������� ��� ����� ����������
    private List<Button[]> symbolsLayout = new List<Button[]>(); // ������ �������� ��� ����� ����������
    


    private int previousRow = 1;    // ���������� ���
    private int currentRow = 1;     // ������� ���
    private int currentColumn = 5;  // ������� �������

    //����� ��� ��������� ������
    private Color highlightedButtonColor;
    private Color normalButtonColor;

    private bool isJoystickModeEnabled;

    private void Awake()
    {
        isJoystickModeEnabled = false;
    }

    private void Start()
    {
        FillKeyboardLayout(rusKeyboard, rusLayout);
        FillKeyboardLayout(engKeyboard, engLayout);
        FillKeyboardLayout(symbolsKeyboard, symbolsLayout);

        currentLayout = rusLayout;
        Button currentButton = currentLayout[currentRow][currentColumn];
        highlightedButtonColor = currentButton.colors.highlightedColor;
        normalButtonColor = currentButton.colors.normalColor;
    }
    private void Update()
    {
        if (!isJoystickModeEnabled) { return; }
        // ������� ������ �������������
        if (Input.GetButtonDown("Submit"))
        {
            PressCurrentButton();
        }

        JoystickNavigation();
    }


    /// <summary>
    /// ��������� �� ���������� � ������� ���������
    /// </summary>
    private void JoystickNavigation()
    {       
        //�������� �� �������� ����� ������������ �������� � ��������� ������
        if (Time.time - lastNavigationTime < navigationCooldown)
            return;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");      

        // ����������� �� �����������
        if (Mathf.Abs(horizontal) > joystickDeadZone)
        {
            DeselectButton();
            currentColumn += horizontal > 0 ? 1 : -1;
            ClampColumn();
            lastNavigationTime = Time.time;
            SelectButton();
        }
        // ����������� �� ���������
        else if (Mathf.Abs(vertical) > joystickDeadZone)
        {
            DeselectButton();
            int previousColumn = currentColumn;
            previousRow = currentRow;
            currentRow += vertical > 0 ? -1 : 1;
            ClampRow();
            // ������������ ������� ������ � ����� ���� � ������ ����������� ���������
            currentColumn = CalculateColumnByPercentage(previousColumn, previousRow);
            ClampColumn();
            lastNavigationTime = Time.time;
            SelectButton();
        }     
    }

    /// <summary>
    /// ��������� ������� ������
    /// </summary>
    private void SelectButton()
    {
        Button currentButton = currentLayout[currentRow][currentColumn];
        Image buttonImage = currentButton.GetComponentInChildren<Image>();
        buttonImage.color = highlightedButtonColor;
        soundController.PlayButtonSwapSoundAt(transform.position);
    }
    /// <summary>
    /// ����� ��������� ������
    /// </summary>
    private void DeselectButton()
    {
        Button currentButton = currentLayout[currentRow][currentColumn];
        Image buttonImage = currentButton.GetComponentInChildren<Image>();
        buttonImage.color = normalButtonColor;
    }
    /// <summary>
    /// ���������� �������� ����������� � ������
    /// </summary>
    private void PressCurrentButton()
    {
        Button currentButton = currentLayout[currentRow][currentColumn];
        currentButton.onClick.Invoke();
    }

    private int CalculateColumnByPercentage(int previousColumn, int previousRow)
    {
        Button[] previousRowButtons = currentLayout[previousRow];
        Button[] currentRowButtons = currentLayout[currentRow];

        // ������� �������������� ��������� ������ � ���������� ����
        float percentage = (float)previousColumn / (previousRowButtons.Length - 1);

        //  ������ � ������� ���� � ������ �������������� ���������
        int targetColumnIndex = Mathf.RoundToInt(percentage * (currentRowButtons.Length - 1));

        return targetColumnIndex;
    }
    /// <summary>
    /// ����������� ����������� �� �����
    /// </summary>
    private void ClampRow()
    {
        currentRow = Mathf.Clamp(currentRow, 0, currentLayout.Count - 1);
    }
    /// <summary>
    /// ����������� ����������� �� ��������
    /// </summary>
    private void ClampColumn()
    {
        if (currentRow >= 0 && currentRow < currentLayout.Count)
        {
            currentColumn = Mathf.Clamp(currentColumn, 0, currentLayout[currentRow].Length - 1);
        }
    }
    /// <summary>
    /// ���������� ���������� ����� �������� �� ����
    /// </summary>
    /// <param name="keyboardObject"></param>
    /// <param name="layout"></param>
    private void FillKeyboardLayout(GameObject keyboardObject, List<Button[]> layout)
    {      
        Button[] systemButtons = setupButtonsObject.GetComponentsInChildren<Button>();
        layout.Add(systemButtons);
        // �������� �� ���� ����� ����������
        foreach (Transform rowTransform in keyboardObject.transform)
        {
            // �������� ��� ������ � ������� ����
            Button[] rowButtons = rowTransform.GetComponentsInChildren<Button>(true);
            layout.Add(rowButtons);
        }
    }
    /// <summary>
    /// ������������ ���� ����������
    /// </summary>
    /// <param name="keyboardLayout"></param>
    public void SwitchKeyboardLayout(KeyboardLayout keyboardLayout)
    {
        switch (keyboardLayout) 
        {
            case KeyboardLayout.Rus:
                currentLayout = rusLayout;
                break;
            case KeyboardLayout.Eng:
                currentLayout = engLayout;
                break;
            case KeyboardLayout.Sybmols:
                currentLayout = symbolsLayout;
                break;
        }
        SelectButton();
    }
    /// <summary>
    /// ���� ������ ����� � ���������
    /// </summary>
    public void ToggleJoystickMode()
    {
        isJoystickModeEnabled = !isJoystickModeEnabled;
        if (isJoystickModeEnabled)
        {
            SelectButton();
        }
        else DeselectButton();

    }
}
