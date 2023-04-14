using UnityEngine;
using VoxelBusters.EssentialKit;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Use { get; private set; }
    public bool HasConnection => NetworkServices.IsInternetActive;

    private void Awake()
    {
        if (Use == null)
            Use = this;
    }
    
    private void OnEnable()
    {
        NetworkServices.OnHostReachabilityChange += OnHostReachabilityChange;
        NetworkServices.OnInternetConnectivityChange += OnInternetConnectivityChange;
    }

    private void OnDisable()
    {
        NetworkServices.OnHostReachabilityChange -= OnHostReachabilityChange;
        NetworkServices.OnInternetConnectivityChange -= OnInternetConnectivityChange;
    }
    
    private void OnInternetConnectivityChange(NetworkServicesInternetConnectivityStatusChangeResult result)
    {
        Debug.Log("Received internet connectivity changed event.");
        Debug.Log("Internet connectivity status: " + result.IsConnected);

        if (Time.time < .4f)
            return;

        if (!result.IsConnected)
            UIManager.Use.ShowConnectionLostPopup(true);
    }

    private void OnHostReachabilityChange(NetworkServicesHostReachabilityStatusChangeResult result)
    {
        Debug.Log("Received host reachability changed event.");
        Debug.Log("Host reachability status: " + result.IsReachable);
    }
}