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

public class LoginTests : MonoBehaviour
{
    public GameObject loginUI;
    public int CurrentCoins { get; private set; }
    public event Action<int> OnCoinsChanged;

    public static LoginTests Instance;
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;


        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private async void Start()
{
    await UnityServices.InitializeAsync();

    if (!FB.IsInitialized)
        FB.Init();
    else
        FB.ActivateApp();

    bool signedIn = await SignInCachedUserAsync();

    if (signedIn && AuthenticationService.Instance.IsSignedIn)
    {
        Debug.Log("✅ Auto-login successful");
        await EconomyManager.Instance.InitializePlayerEconomy();

        // ✅ Only load Cloud Save data **after sign-in**
        await CloudSaveManager.Instance.LoadData();

        loginUI.SetActive(false);
        SceneManager.LoadScene("Menu");
    }
    else
    {
        Debug.Log("🔐 No valid session. Showing login UI.");
        loginUI.SetActive(true);
    }
}



    public void LogInAnonymouslyBtn()
    {
        _ = SignUpAnonymouslyAsync();
    }

    public void LogInFBBtn()
    {
        if (!FB.IsInitialized)
        {
            FB.Init(() =>
            {
                if (FB.IsInitialized)
                {
                    FB.ActivateApp();
                    AttemptFacebookLogin();
                }
                else
                {
                    Debug.LogError("Failed to initialize Facebook SDK.");
                }
            });
        }
        else
        {
            FB.ActivateApp();
            AttemptFacebookLogin();
        }
    }

    async Task SignUpAnonymouslyAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign in anonymously succeeded!");
            await EconomyManager.Instance.InitializePlayerEconomy();
            await CloudSaveManager.Instance.LoadData();
            loginUI.SetActive(false);
            SceneManager.LoadScene("Menu");

            // Shows how to get the playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }

    async Task<bool> SignInCachedUserAsync()
    {
        if (!AuthenticationService.Instance.SessionTokenExists)
            return false;

        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log($"✅ Cached user signed in. PlayerID: {AuthenticationService.Instance.PlayerId}");
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogWarning("⚠️ Cached sign-in failed: " + ex.Message);
            return false;
        }
    }

    void AttemptFacebookLogin()
    {
        var perms = new List<string>() { "public_profile", "email" };
        FB.LogInWithReadPermissions(perms, async result =>
        {
            if (FB.IsLoggedIn)
            {
                string token = AccessToken.CurrentAccessToken.TokenString;
                await SignInWithFacebookAsync(token);
                await EconomyManager.Instance.InitializePlayerEconomy();
                await CloudSaveManager.Instance.LoadData();
                loginUI.SetActive(false);
                SceneManager.LoadScene("Menu");
            }
            else
            {
                Debug.Log("Facebook login canceled or failed: " + result.Error);
            }
        });
    }
    async Task SignInWithFacebookAsync(string token)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithFacebookAsync(token);
            Debug.Log("SignIn is successful.");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }
}
