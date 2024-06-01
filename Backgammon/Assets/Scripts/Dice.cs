using UnityEngine;

public class Dice : MonoBehaviour
{
    private bool clickable = false;
    private const string HIGHLIGHTSHADERNAME = "Shader Graphs/DiceHighlight";
    private const string DEFAULTSHADERNAME = "Lightweight Render Pipeline/Lit";
    private int minDiceRollNum = 1;
    private int maxDiceRollNum = 6;

    private Rigidbody rb = null;
    private Renderer renderer = null;
    private DiceSettings diceSettings;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        renderer = GetComponent<Renderer>();
        diceSettings = SettingsManager.Instance.Settings.DiceSettings;
    }

    public void rollDice(int numToRoll)
    {
        if (numToRoll < minDiceRollNum || numToRoll > maxDiceRollNum)
        {
            Debug.LogError("Invalid number given to dice roll");
            return;
        }

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(diceSettings.ForceAmount);

        if (numToRoll >= 1 && numToRoll <= 6)
        {
            rb.AddTorque(diceSettings.RollTorques[numToRoll - 1]);
        }
        else
        {
            Debug.LogError("Invalid parameter given to dice roll");
        }
    }

    public void changeHighlightDice(bool toHighlight)
    {
        renderer.material.shader = Shader.Find(toHighlight ? HIGHLIGHTSHADERNAME : DEFAULTSHADERNAME);
        clickable = toHighlight;
    }

    public bool isClickable()
    {
        return clickable;
    }
}