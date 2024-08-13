using _root.Script.Network;
using TMPro;
using UnityEngine;

namespace _root.Script.Config
{
    public class UserInfoPanel : MonoBehaviour
    {
        private GameObject _userID;
        private GameObject _userName;

        private void Awake()
        {
            _userID = transform.GetChild(0).gameObject;
            _userName = transform.GetChild(1).gameObject;
            Init();
        }

        public void Init()
        {
            API.GetInfo().OnResponse(playerInfo =>
            {
                _userID.GetComponent<TextMeshProUGUI>().text = $"ID : {playerInfo.id}";
                _userName.GetComponent<TextMeshProUGUI>().text = $"Name : {playerInfo.name}";
            }).OnError((_) =>
            {
                _userID.GetComponent<TextMeshProUGUI>().text = "ID : ???";
                _userName.GetComponent<TextMeshProUGUI>().text = "Name : ???";
            }).Build();
        }
    }
}
