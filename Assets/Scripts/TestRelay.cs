using QFSW.QC;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class TestRelay : MonoBehaviour
{
    public static TestRelay Instance { get; private set; }

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

/*        await UnityServices.InitializeAsync();
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();*/
    }

    [Command]
    public async Task<string> CreateRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log("Relay join code: " + joinCode);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

      //      await Task.Delay(5000);

            NetworkManager.Singleton.StartHost();
            //PlayerSetting.Instance.networkPlayerName.Value = EditPlayerName.Instance.GetPlayerName();

            NetworkManager.Singleton.SceneManager.LoadScene("SampleScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
            
            //Add code
           
            return joinCode;
        }
        catch (RelayServiceException e)
        {
            Debug.LogError("Relay Service Error: " + e);
            return null;
        }
    }

    [Command]
    public async void JoinRelay(string joinCode)
    {
        try
        {
            Debug.Log("Joining Relay with join code: " + joinCode);

            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();
        //    PlayerSetting.Instance.networkPlayerName.Value = EditPlayerName.Instance.GetPlayerName();
            //Add code
        }
        catch (RelayServiceException e)
        {
            Debug.LogError("Relay Service Error: " + e);
        }
    }
}
