using Unity.Netcode;

namespace SideQuests.Managers
{
    public class SQNetworkManager : NetworkBehaviour
    {
        public static SQNetworkManager Instance;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void SyncQuotaFulfilledServerRpc(int quotaFulfilled)
        {
            SyncQuotaFulfilledClientRpc(quotaFulfilled);
        }

        [ClientRpc]
        public void SyncQuotaFulfilledClientRpc(int quotaFulfilled)
        {
            TimeOfDay.Instance.quotaFulfilled = quotaFulfilled;
            TimeOfDay.Instance.UpdateProfitQuotaCurrentTime();
        }

        [ServerRpc(RequireOwnership = false)]
        public void SyncGroupCreditsServerRpc(int newGroupCredits)
        {
            SyncGroupCreditsClientRpc(newGroupCredits);
        }

        [ClientRpc]
        public void SyncGroupCreditsClientRpc(int newGroupCredits)
        {
            FindObjectOfType<Terminal>().groupCredits = newGroupCredits;
        }
    }
}
