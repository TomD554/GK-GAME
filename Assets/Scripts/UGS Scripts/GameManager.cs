using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public GameObject loginUI;
    public int CurrentCoins { get; private set; }
    public event Action<int> OnCoinsChanged;

    public static GameManager Instance;

    private enum LoginMethod { None, Guest, Facebook }
    private LoginMethod currentLoginMethod = LoginMethod.None;

    private async void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Facebook
        if (!FB.IsInitialized)
            FB.Init(OnFacebookInit, OnHideUnity);
        else
            FB.ActivateApp();

        // Unity Services
        await UnityServices.InitializeAsync();

        Debug.Log($"IsSignedIn: {AuthenticationService.Instance.IsSignedIn}");
        Debug.Log($"SessionTokenExists: {AuthenticationService.Instance.SessionTokenExists != false}");

        if (AuthenticationService.Instance.IsSignedIn)
        {
            Debug.Log($"🔁 Auto-login: Player ID = {AuthenticationService.Instance.PlayerId}");
            loginUI.SetActive(false);
            StartCoroutine(InitializeUGSFlow());
        }
        else
        {
            loginUI.SetActive(true);
        }
    }


    private void OnFacebookInit()
    {
        FB.ActivateApp();
    }

    private void OnHideUnity(bool isGameShown)
    {
        Time.timeScale = isGameShown ? 1 : 0;
    }

    public void OnFacebookLoginPressed()
    {
        var perms = new List<string>() { "public_profile", "email" };
        FB.LogInWithReadPermissions(perms, OnFacebookLoginComplete);
        currentLoginMethod = LoginMethod.Facebook;
    }

    private void OnFacebookLoginComplete(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            Debug.Log("🌐 Facebook login success");
            loginUI.SetActive(false);
            StartCoroutine(InitializeUGSFlow());
        }
        else
        {
            Debug.LogError("Facebook login failed: " + result.Error);
        }
    }

    public void OnGuestLoginPressed()
    {
        loginUI.SetActive(false);
        currentLoginMethod = LoginMethod.Guest;
        StartCoroutine(InitializeUGSFlow());
    }

    private IEnumerator InitializeUGSFlow()
    {
        // Only sign in anonymously if no session exists
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            var signInTask = AuthenticationService.Instance.SignInAnonymouslyAsync();
            yield return new WaitUntil(() => signInTask.IsCompleted);

            if (signInTask.Exception != null)
            {
                Debug.LogError("UGS Sign-in failed: " + signInTask.Exception.Message);
                yield break;
            }
        }

        Debug.Log($"🔐 Signed in as: {AuthenticationService.Instance.PlayerId}");

        var syncTask = EconomyService.Instance.Configuration.SyncConfigurationAsync();
        yield return new WaitUntil(() => syncTask.IsCompleted);

        yield return InitializeCoins("COINS", 100);

        Debug.Log("✅ UGS setup complete.");
        SceneManager.LoadScene("Menu");
    }



    private IEnumerator InitializeCoins(string currencyId, int amount)
    {
        var resultTask = EconomyService.Instance.PlayerBalances.GetBalancesAsync();
        yield return new WaitUntil(() => resultTask.IsCompleted);

        var result = resultTask.Result;
        var balance = result.Balances.FirstOrDefault(b => b.CurrencyId == currencyId);

        if (balance == null || balance.Balance == 0)
        {
            var setTask = EconomyService.Instance.PlayerBalances.SetBalanceAsync(currencyId, amount);
            yield return new WaitUntil(() => setTask.IsCompleted);
            Debug.Log($"Initialized {amount} {currencyId} for new player.");

            SetCoins(amount); // ✅ Store and notify
        }
        else
        {
            SetCoins((int)balance.Balance); // ✅ Store and notify
            Debug.Log($"Player already has {balance.Balance} {currencyId}.");
        }
    }

    public void SetCoins(int amount)
    {
        CurrentCoins = amount;
        OnCoinsChanged?.Invoke(CurrentCoins);
    }

}
