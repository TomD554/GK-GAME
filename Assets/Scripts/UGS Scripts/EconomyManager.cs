using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
using System.Threading.Tasks;
using System.Linq;
using System;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance;

    public int CurrentCoins { get; private set; }
    public event Action<int> OnCoinsChanged;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    public async Task InitializePlayerEconomy()
    {
        await EconomyService.Instance.Configuration.SyncConfigurationAsync();

        var result = await EconomyService.Instance.PlayerBalances.GetBalancesAsync();
        var balance = result.Balances.FirstOrDefault(b => b.CurrencyId == "COINS");

         if (balance == null)
        {
            const int startingCoins = 1000;

            await EconomyService.Instance.PlayerBalances.SetBalanceAsync("COINS", startingCoins);
            CurrentCoins = startingCoins;
            OnCoinsChanged?.Invoke(CurrentCoins);

        }
        else
        {
            CurrentCoins = (int)balance.Balance;
            OnCoinsChanged?.Invoke(CurrentCoins);
        }

    }

    public async Task IncrementPlayerBalance(string currencyId, int amount)
    {
        var result = await EconomyService.Instance.PlayerBalances.IncrementBalanceAsync(currencyId, amount);
        CurrentCoins = (int)result.Balance;

        Debug.Log($"✅ {amount} coins added! New balance: {CurrentCoins}");

        OnCoinsChanged?.Invoke(CurrentCoins);
    }


}
