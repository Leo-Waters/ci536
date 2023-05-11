using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoneyGraph : MonoBehaviour
{
    public TextMeshProUGUI Income_text;
    public TextMeshProUGUI Funds_text;
    public TextMeshProUGUI Upkeep_text;

    public Slider Income_Slider;
    public Slider Funds_slider;
    public Slider UpKeep_Slider;


    void OnEnable()
    {
        MoneySystem.current.OnMoneyChanged+= UpdateMoneyUI;
        UpdateMoneyUI(this, EventArgs.Empty);
    }
    void OnDisable()
    {
        MoneySystem.current.OnMoneyChanged -= UpdateMoneyUI;
    }

    void UpdateMoneyUI(object sender, EventArgs e)
    {
        Funds_text.text = "Money: " + MoneySystem.current.Money;
        Income_text.text = "Income: " + MoneySystem.current.WaveIncome;
        Upkeep_text.text = "Up Keep: " + MoneySystem.current.UpKeep;

        if (MoneySystem.current.Money >= 1)//has the player got money
        {
            Income_Slider.maxValue = Funds_slider.maxValue = UpKeep_Slider.maxValue = MoneySystem.current.Money + MoneySystem.current.UpKeep + MoneySystem.current.WaveIncome;


            if (MoneySystem.current.WaveIncome > 0)//if wave income is not negitive
            {
                Funds_slider.value = MoneySystem.current.Money;
                Income_Slider.value = MoneySystem.current.Money + MoneySystem.current.WaveIncome;
                UpKeep_Slider.value = MoneySystem.current.UpKeep;
            }
            else//negitive wave income
            {
                Funds_slider.value = MoneySystem.current.Money;
                Income_Slider.value = 0;
                UpKeep_Slider.value = MoneySystem.current.UpKeep + MoneySystem.current.WaveIncome;
            }
        }
        else//player is bankrupt
        {
            Income_Slider.maxValue = Funds_slider.maxValue = UpKeep_Slider.maxValue = MoneySystem.current.UpKeep + MathF.Abs(MoneySystem.current.WaveIncome);
            Funds_slider.value = 0;

            if (MoneySystem.current.WaveIncome > 0)//if wave income is not negitive
            {
                Income_Slider.value = MoneySystem.current.WaveIncome;
                UpKeep_Slider.value = MoneySystem.current.UpKeep;
            }
            else//negitive wave income
            {
                UpKeep_Slider.value= UpKeep_Slider.maxValue;
            }


        }



    }
}
