using System;
using UnityEngine;

public class MoneySystem : MonoBehaviour
{
    public static MoneySystem current;
    public event EventHandler OnMoneyChanged;//tells otehr system to update when the money value has changed
    public int Money=100;//players money
    public int UpKeep=10;//players upkeep / money deduction per wave
    public int WaveIncome = 0;//players income on completion of the wave


    bool WaveActive = false;
    public bool HasGoneBankRupt = false;//has the player gone bank rupt once during the match

    public bool hadAnEscape = false;

    public static bool IsCurrentlyBankRupt()//check if the player is bankrupt
    {
        if (current.Money >= 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    //returns true if the player had enough funds to make the purchase;
    public static bool MakePurchase(int InitalCost,int WeeklyUpKeep)
    {
        if ((current.Money - (InitalCost + WeeklyUpKeep)) >= 0 || (WeeklyUpKeep==0&& current.Money - InitalCost>=0))
        {
            if (current.WaveActive)// is currentl in a wave so weekly upkeep must be taken tooo
            {
                current.Money -= InitalCost;
                current.Money -= WeeklyUpKeep;

                current.UpKeep += WeeklyUpKeep;

                current.OnMoneyChanged?.Invoke(current, EventArgs.Empty);
                return true;

            }
            else//isnt in a wave so upkeep must be added but not taken yett
            {
                if(current.Money- (InitalCost +current.UpKeep+ WeeklyUpKeep) >= 0){
                    current.Money -= InitalCost;
                    current.UpKeep += WeeklyUpKeep;

                    current.OnMoneyChanged?.Invoke(current, EventArgs.Empty);
                    return true;
                }
                else
                {
                    return false;
                }

            }

        }
        else
        {
            return false;
        }
    }
    //refunds upkeep of guard
    public static void RefundGuardUpKeep()
    {
        current.UpKeep -= 10;
        current.OnMoneyChanged?.Invoke(current, EventArgs.Empty);
    }
    //partialy refunds the cost spent on a guard
    public static void RefundGuardCost(int cost)
    {
        current.Money += (int)MathF.Round(cost/2);
        current.OnMoneyChanged?.Invoke(current, EventArgs.Empty);
    }

    //deducts an amount from the current waves income
    public void DeductIncome(int Amount)
    {
        hadAnEscape = true;
        WaveIncome -= Amount;
        OnMoneyChanged?.Invoke(this, EventArgs.Empty);
    }
    //sets the current waves income
    public void SetIncome(int Amount)
    {
        WaveIncome = Amount;
        OnMoneyChanged?.Invoke(this, EventArgs.Empty);
    }

    private void Awake()
    {
        current = this;//set static singleton instance
    }
    private void Start()//subscribe to wave start and end events
    {
        WaveManager.Current.OnWaveStarted += WaveStarted;
        WaveManager.Current.OnWaveEnded += WaveEnded;
    }
    public void OnDestroy()//un-subscribe to wave start and end events
    {
        WaveManager.Current.OnWaveStarted -= WaveStarted;
        WaveManager.Current.OnWaveEnded -= WaveEnded;
    }

    private void WaveEnded(object sender, System.EventArgs e)//wave ended
    {
        WaveActive = false;
        Money += WaveIncome;//add wave income
        WaveIncome = 0;

        //invoke money changed event so other systems like UI can update
        OnMoneyChanged?.Invoke(this,EventArgs.Empty);
    }

    private void WaveStarted(object sender, System.EventArgs e)
    {
        WaveActive = true;
        Money -= UpKeep;

        if (Money < 0)//if wave start made the player bankrupt, effects max stars
        {
            HasGoneBankRupt = true;
            Debug.Log("Player Bank Rupt");
        }
        //invoke money changed event so other systems like UI can update
        OnMoneyChanged?.Invoke(this, EventArgs.Empty);

    }


}
