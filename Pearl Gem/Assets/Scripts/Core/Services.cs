using UnityEngine;

public static class Services
{
    public static GameManager GameManager { get; private set; }
    public static UIManager UIManager { get; private set; }
    public static DataManager DataManager { get; private set; }

    public static void Register(GameManager gm) => GameManager = gm;
    public static void Register(UIManager ui) => UIManager = ui;
    public static void Register(DataManager dm) => DataManager = dm;
}
