using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using System.Threading.Tasks;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;

public class TestRelay : MonoBehaviour
{
    private async void Start(){
        await UnityServices.InitializeAsync();
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private async Task<string> CreateRelay(){
        try{
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(relayServerData    );

            NetworkManager.Singleton.StartHost();
            return joinCode;
        }catch (RelayServiceException e){
            Debug.Log(e);
            return null;
        }
    }
    private async void JoinRelay(string joinCode){
        try{
        JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
        RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
        
        NetworkManager.Singleton.StartClient();
        }catch(RelayServiceException e){
            Debug.Log(e);
        }
    }
}
