using UnityEngine;

public class Singleton<T>
{
    private static T _instance;

    public static T instance {
        get {
            if (_instance == null) 
            {
                GameObject gameManagerObject = GameObject.FindGameObjectWithTag("GameController");
                if (gameManagerObject != null) {
                    _instance = gameManagerObject.GetComponent<T>();
                }
            }
            return _instance;
        }
    }
}
