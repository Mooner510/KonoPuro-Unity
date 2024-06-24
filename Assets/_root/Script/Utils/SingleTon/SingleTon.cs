namespace _root.Script.Utils.SingleTon
{
    public class SingleTon<T> where T : class, new()
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;
                return _instance = new T();
            }
        }
    }
}