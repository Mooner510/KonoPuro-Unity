using _root.Script.Client;
using _root.Script.Data;
using _root.Script.Network;
using _root.Script.UI;
using UnityEngine;

namespace _root.Script.Title
{
    public class TitleUi : MonoBehaviour
    {
        [SerializeField] private GameObject baseAuths;

        [SerializeField] private GameObject loginRequired;

        private Throbber throbber;

        private void Awake()
        {
            throbber = FindObjectOfType<Throbber>();
        }

        private void Start()
        {
            gameObject.SetActive(false);
            baseAuths.SetActive(false);
            loginRequired.SetActive(false);
            if (Networking.AccessToken == null) Login(true);
        }

        public void Login(bool logout)
        {
            baseAuths.SetActive(false);
            loginRequired.SetActive(!logout);

            if (!logout) return;
            baseAuths.SetActive(true);
            Networking.AccessToken = null;
            UserData.Instance.ActiveDeck = null;
            UserData.Instance.InventoryCards = null;
            UserData.Instance.gold = null;
            var client = FindObjectOfType<NetworkClient>();
            if(client) Destroy(client.gameObject);
        }

        public void SetThrobber(bool active)
        {
            throbber.SetActive(active);
        }
    }
}