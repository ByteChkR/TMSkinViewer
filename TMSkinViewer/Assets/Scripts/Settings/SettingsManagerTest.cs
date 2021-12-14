using UnityEngine;

public class SettingsManagerTest : MonoBehaviour
{
    private void Start()
    {
        SettingsManager.AddSettingsObject(new TestSettings());
        
        SettingsManager.PrintDebug();
    }
}
