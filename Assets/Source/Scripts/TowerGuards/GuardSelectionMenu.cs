using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GuardSelectionMenu : MonoBehaviour
{
    [System.Serializable]
    public class UpgradeButton
    {
        [SerializeField]
        public TextMeshProUGUI UpgradeNameText;
        [SerializeField]
        Button Button;
        public void SetVisable(bool visable)
        {
            Button.interactable=visable;
            UpgradeNameText.text = "Reached Max";
        }
    }

    public static GuardSelectionMenu current;
    private void Awake()
    {
        current = this;
    }

    public GameObject Menu;
    public GameObject[] SelectedOcupied;
    public GameObject[] SelectedEmpty;


    public TextMeshProUGUI GuardNameText;
    public UpgradeButton[] UpgradeButtons;

    public void PurchaseTroop()
    {
        if (MoneySystem.MakePurchase(0, 10))
        {
            TowerSelectionSystem.Current.CreateGuard();
        }

        CloseMenu();
    }


    void SetMenu(bool Empty)
    {
        for (int i = 0; i < SelectedOcupied.Length; i++)
        {
            SelectedOcupied[i].SetActive(!Empty);
        }
        for (int i = 0; i < SelectedEmpty.Length; i++)
        {
            SelectedEmpty[i].SetActive(Empty);
        }
    }

    public void CloseMenu()
    {
        Menu.SetActive(false);
        SelectedGuard = null;
    }

    public void SelectEmptyPlot(GameObject plot)
    {
        Menu.SetActive(true);
        SetMenu(true);
    }
    GuardAI SelectedGuard;
    public void SelectExistingGuard(GameObject plot)
    {
        plot.GetComponent<GuardAI>().RangeDisplay(true);
        Menu.SetActive(true);
        SetMenu(false);

        SelectedGuard = plot.GetComponent<GuardAI>();

        var CurrentUpgrades = SelectedGuard.upgradesSystem.current;

        int i = 0;
        while ( i < CurrentUpgrades.Upgrades.Length)
        {
            UpgradeButtons[i].SetVisable(true);
            UpgradeButtons[i].UpgradeNameText.text = CurrentUpgrades.Upgrades[i].GuardName+"\nCost: " + CurrentUpgrades.Upgrades[i].Cost;
            i++;
        }

        while (i < 3)
        {
            UpgradeButtons[i].SetVisable(false);
            i++;
        }

        GuardNameText.text = CurrentUpgrades.GuardName;

    }

    public void UpgradeClicked(int Index)
    {
        if(MoneySystem.MakePurchase(SelectedGuard.upgradesSystem.current.Upgrades[Index].Cost, 0)){
            SelectedGuard.upgradesSystem.current.ChosenUpgrade = Index;
            SelectExistingGuard(SelectedGuard.gameObject);
        }
    }

    public void FireGuardClicked()
    {
        MoneySystem.RefundGuardUpKeep();
        MoneySystem.RefundGuardCost(SelectedGuard.upgradesSystem.UpgradeTree.GetCurrentGuardValue());
        TowerSelectionSystem.Current.DestroyGuard(SelectedGuard.gameObject);
        CloseMenu();
    }
}
