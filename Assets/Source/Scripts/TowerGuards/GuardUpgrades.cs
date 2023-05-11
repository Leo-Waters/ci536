using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardUpgrades : MonoBehaviour
{
    [System.Serializable]
    public class GuardUpgrade
    {
        [SerializeField]
        public string GuardName;
        [SerializeField]
        public Sprite GuardIcon;
        [SerializeField]
        public int Cost = 10;
        [SerializeField]
        public float Range = 2;
        [SerializeField]
        public float StunTime = 0;
        [SerializeField]
        public float Damage = 10;
        [SerializeField]
        public float CoolDown = 1;
        [SerializeField]
        public string BulletPrefabName= "Bullet";
        //must be 3 upgrades no more no less
        [SerializeField]
        public GuardUpgrade[] Upgrades;
        //-1 means this is the current upgrade 1,2,3 represent the next node in the upgrade tree
        [SerializeField]
        public int ChosenUpgrade =-1;

        //gets the value of all upgrades spent on the guard using recursion
        public int GetCurrentGuardValue()
        {
            if (ChosenUpgrade != -1) {
                return Cost + Upgrades[ChosenUpgrade].GetCurrentGuardValue();
            }
            else{
                return Cost;
            }
        }

        //get the current guard upgrade using recursion
        public GuardUpgrade GetCurrentGuardUpgrade()
        {
            if(Upgrades==null|| Upgrades.Length != 3)
            {
                return this;
            }
            switch (ChosenUpgrade)
            {
                case 0:
                    return Upgrades[0].GetCurrentGuardUpgrade();
                case 1:
                    return Upgrades[1].GetCurrentGuardUpgrade();
                case 2:
                    return Upgrades[2].GetCurrentGuardUpgrade();
                case -1:
                default:
                    return this;
            }
        }

    }
    [SerializeField]
    public GuardUpgrade UpgradeTree;//the base upgrade

    public GuardUpgrade current //getter for current guard upgrade
    {
        get
        {
            return UpgradeTree.GetCurrentGuardUpgrade();
        }
    }
}
