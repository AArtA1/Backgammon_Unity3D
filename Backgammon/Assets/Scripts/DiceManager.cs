using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DiceRoller
{
    private int minDiceValue;
    private int maxDiceValue;

    public DiceRoller(int minValue, int maxValue)
    {
        minDiceValue = minValue;
        maxDiceValue = maxValue;
    }

    public int[] RollDices(bool allowDoubles)
    {
        int diceValue1 = Random.Range(minDiceValue, maxDiceValue + 1);
        int diceValue2 = Random.Range(minDiceValue, maxDiceValue + 1);

        if (!allowDoubles)
        {
            while (diceValue1 == diceValue2)
            {
                diceValue1 = Random.Range(minDiceValue, maxDiceValue + 1);
                diceValue2 = Random.Range(minDiceValue, maxDiceValue + 1);
            }
        }

        return new int[] { diceValue1, diceValue2 };
    }
}


/// <summary>
/// Данный класс позволяет управлять генерацией игральных костей.
/// Когда приходит время бросить кость, то мы вызываем функцию throwDice, генерируем два рандомных значения в диапазоне и 
/// вызываем скрипт Dice.cs, который имитирует бросание кости, так как сила приложенная к кости всегда будет давать нужный результат.
/// </summary>
public class DiceManager : MonoBehaviour
{
    // Максимальные значения, которые можно получить бросив кость
    const int minDiceValue = 1;
    const int maxDiceValue = 6;

    // Определяем начальные точки для игральных костей во время броска и в статичном режиме
    [SerializeField] Vector3 positionOfDiceThrowing = new Vector3(2, 2.5f, -11.5f);
    [SerializeField] Vector3 starterPositionOfDice = new Vector3(-0.88f, 1.52f, 0.58f);

    // Ссылки на экземпляры объекта игральной кости
    [SerializeField] Dice firstDice = null;
    [SerializeField] Dice secondDice = null;

    // Задает базовое положение для костей в статичном состоянии 
    [SerializeField] float dice2ZOffset = 2f;
    [SerializeField] float resetRotationY = 90f;

    private DiceRoller diceRoller;

    // Метод Awake вызывается при инициализации объекта
    private void Awake()
    {
        diceRoller = new DiceRoller(minDiceValue, maxDiceValue);
    }

    /// <summary>
    /// Данный метод делает кликабельным игральные кости, меняя состояние шейдера, тем самым дав понять пользователю о возможности разыграть игральную кость.
    /// </summary>
    public void startToThrowDices()
    {
        resetTransformations(starterPositionOfDice);
        this.transform.Rotate(0f, resetRotationY, 0f);
        makeDiceClickable();
    }

    /// <summary>
    /// После вызова данного метода идет генерация рандомных значений и вызов метода для отображения эффекта подбрасывания.
    /// </summary>
    /// <param name="allowDoubles">Флаг, указывающий, можно ли допускать выпадение одинаковых значений.</param>
    /// <returns>Массив из двух значений бросков костей.</returns>
    public int[] throwDice(bool allowDoubles)
    {
        resetTransformations(positionOfDiceThrowing); // Перезадаем скорость броска, чтобы гарантировать, корректность анимации броска игральной кости
        firstDice.changeHighlightDice(false); // Кости не могут двигаться во время броска, поэтому отключаем их кликабельность сменой шейдера
        secondDice.changeHighlightDice(false);

        int[] rolledValues = diceRoller.RollDices(allowDoubles); // Генерация значений бросков

        firstDice.rollDice(rolledValues[0]); // Запускаем метод для имитации рандомной анимации бросания игральной кости
        secondDice.rollDice(rolledValues[1]);

        return rolledValues; // Возвращаем данные значения экземпляру класс, чтобы дать понять, какие возможные ходы пользователь может сделать
    }

    /// <summary>
    /// Перезадаем кость в соответствии с identity, чтобы избежать каких-то странных позиций.
    /// </summary>
    /// <param name="newPos">Новая позиция для установки.</param>
    private void resetTransformations(Vector3 newPos)
    {
        this.transform.position = newPos;
        this.transform.rotation = Quaternion.identity;
        firstDice.transform.localPosition = new Vector3(0f, 0f, 0f);
        secondDice.transform.localPosition = new Vector3(0f, 0f, dice2ZOffset);
        firstDice.transform.rotation = Quaternion.identity;
        secondDice.transform.rotation = Quaternion.identity;
    }

    /// <summary>
    /// Запускаем шейдеры, для того чтобы дать понять игроку, что они кликабельны для разыгрывания.
    /// </summary>
    private void makeDiceClickable()
    {
        firstDice.changeHighlightDice(true);
        secondDice.changeHighlightDice(true);
    }
}