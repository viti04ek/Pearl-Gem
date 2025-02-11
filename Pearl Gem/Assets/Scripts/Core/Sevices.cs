using UnityEngine;

public static class Sevices
{
    public static GameManager GameManager;
    public static UIManager UIManager;

    public static void Initialize()
    {
        GameManager = GetComponentFromScene<GameManager>();
        //UIManager = GetComponentFromScene<UIManager>();
    }
    
    private static T GetComponentFromScene<T>() where T : Object 
    {
        var component = GameObject.FindObjectOfType<T>(includeInactive: true);

        if (component == null) 
        {
            UnityEngine.Debug.LogWarning($"Unable to find component {typeof(T)} on the scene");
        }

        return component;
    }
}
