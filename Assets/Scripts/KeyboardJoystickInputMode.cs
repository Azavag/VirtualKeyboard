using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VirtualKeyboard;
/// <summary>
/// Класс, отвечающий за работу режима ввода с джойстика
/// </summary>
public class KeyboardJoystickInputMode : MonoBehaviour
{
    [SerializeField]
    private KeyboardController keyboardController;
    [SerializeField]
    private SoundController soundController;

    // Мёртвая зона джойстика
    [SerializeField]
    private float joystickDeadZone = 0.2f;      
    // Задержка перед возможность перейти к другой кнопке
    private float navigationCooldown = 0.2f;    
    // Время последнего переключения кнопок
    private float lastNavigationTime;
    // Вектор значений с осей джойстика
    private Vector2 joystickInput;

    [Header("Keyboard layouts")]
    [SerializeField]
    private GameObject rusKeyboard; // Объект-контейнер для русской клавиатуры
    [SerializeField]
    private GameObject engKeyboard; // Объект-контейнер для английской клавиатуры
    [SerializeField]
    private GameObject symbolsKeyboard;// Объект-контейнер для символьной клавиатуры
    [SerializeField]
    private GameObject setupButtonsObject; //объект - контейнер для системных кнопок

    private List<Button[]> currentLayout = new List<Button[]>(); // Список массивов для строк клавиатуры
    private List<Button[]> rusLayout = new List<Button[]>(); // Список массивов для строк клавиатуры
    private List<Button[]> engLayout = new List<Button[]>(); // Список массивов для строк клавиатуры
    private List<Button[]> symbolsLayout = new List<Button[]>(); // Список массивов для строк клавиатуры
    


    private int previousRow = 1;    // Предыдущий ряд
    private int currentRow = 1;     // Текущий ряд
    private int currentColumn = 5;  // Текущий столбец

    //цвета для выделения кнопки
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
        // Нажатие кнопки подтверждения
        if (Input.GetButtonDown("Submit"))
        {
            PressCurrentButton();
        }

        JoystickNavigation();
    }


    /// <summary>
    /// Навигация по клавиатуре с помощью джойстика
    /// </summary>
    private void JoystickNavigation()
    {       
        //Проверка на задержку перед возможностью перехода к следующей кнопке
        if (Time.time - lastNavigationTime < navigationCooldown)
            return;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");      

        // Перемещение по горизонтали
        if (Mathf.Abs(horizontal) > joystickDeadZone)
        {
            DeselectButton();
            currentColumn += horizontal > 0 ? 1 : -1;
            ClampColumn();
            lastNavigationTime = Time.time;
            SelectButton();
        }
        // Перемещение по вертикали
        else if (Mathf.Abs(vertical) > joystickDeadZone)
        {
            DeselectButton();
            int previousColumn = currentColumn;
            previousRow = currentRow;
            currentRow += vertical > 0 ? -1 : 1;
            ClampRow();
            // Рассчитываем целевой индекс в новом ряду с учётом процентного положения
            currentColumn = CalculateColumnByPercentage(previousColumn, previousRow);
            ClampColumn();
            lastNavigationTime = Time.time;
            SelectButton();
        }     
    }

    /// <summary>
    /// Выделение текущей кнопки
    /// </summary>
    private void SelectButton()
    {
        Button currentButton = currentLayout[currentRow][currentColumn];
        Image buttonImage = currentButton.GetComponentInChildren<Image>();
        buttonImage.color = highlightedButtonColor;
        soundController.PlayButtonSwapSoundAt(transform.position);
    }
    /// <summary>
    /// Сброс выделения кнопки
    /// </summary>
    private void DeselectButton()
    {
        Button currentButton = currentLayout[currentRow][currentColumn];
        Image buttonImage = currentButton.GetComponentInChildren<Image>();
        buttonImage.color = normalButtonColor;
    }
    /// <summary>
    /// Выполнение действий привязанной к кнопке
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

        // Рассчёт относительного положения кнопки в предыдущем ряду
        float percentage = (float)previousColumn / (previousRowButtons.Length - 1);

        //  Индекс в текущем ряду с учётом относительного положения
        int targetColumnIndex = Mathf.RoundToInt(percentage * (currentRowButtons.Length - 1));

        return targetColumnIndex;
    }
    /// <summary>
    /// Ограничение перемещения по рядам
    /// </summary>
    private void ClampRow()
    {
        currentRow = Mathf.Clamp(currentRow, 0, currentLayout.Count - 1);
    }
    /// <summary>
    /// Ограничение перемещения по столбцам
    /// </summary>
    private void ClampColumn()
    {
        if (currentRow >= 0 && currentRow < currentLayout.Count)
        {
            currentColumn = Mathf.Clamp(currentColumn, 0, currentLayout[currentRow].Length - 1);
        }
    }
    /// <summary>
    /// Заполнение контейнера всеми кнопками на слое
    /// </summary>
    /// <param name="keyboardObject"></param>
    /// <param name="layout"></param>
    private void FillKeyboardLayout(GameObject keyboardObject, List<Button[]> layout)
    {      
        Button[] systemButtons = setupButtonsObject.GetComponentsInChildren<Button>();
        layout.Add(systemButtons);
        // Проходим по всем рядам клавиатуры
        foreach (Transform rowTransform in keyboardObject.transform)
        {
            // Получаем все кнопки в текущем ряду
            Button[] rowButtons = rowTransform.GetComponentsInChildren<Button>(true);
            layout.Add(rowButtons);
        }
    }
    /// <summary>
    /// Переключение слоя клавиатуры
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
    /// Тогл режима ввода с джойстика
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
