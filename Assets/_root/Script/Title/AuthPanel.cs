using _root.Script.Network;
using UnityEngine;
using Void = _root.Script.Network.Void;

namespace _root.Script.Title
{
    public class AuthPanel : MonoBehaviour
    {
        public static AuthPanel instance;
        private SignUi login;
        private SignUi signUp;

        private void Awake()
        {
            var signUis = GetComponentsInChildren<SignUi>();
            login = signUis[0];
            signUp = signUis[1];
            instance = this;
        }

        private void Start()
        {
            Show(false);
            ShowSignUp(false);
        }

        public void Init()
        {
            login.Init();
            signUp.Init();
        }

        public void Show(bool show)
        {
            Init();
            gameObject.SetActive(show);
        }

        public void ShowSignUp(bool show)
        {
            Init();
            login.gameObject.SetActive(!show);
            signUp.gameObject.SetActive(show);
        }

        public Networking.Post<TokenResponse> SignIn()
        {
            var req = login.GetSignInReq();
            if (req.id == "" || req.password == "") return null;
            return API.SignIn(req);
        }

        public Networking.Post<Void> SignUp()
        {
            var req = signUp.GetSignUpReq();
            if (req.id == "" || req.password == "" || req.name == "") return null;
            return API.SignUp(req);
        }
    }
}