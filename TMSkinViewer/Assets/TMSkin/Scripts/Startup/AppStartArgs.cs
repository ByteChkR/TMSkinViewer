using System.Collections.Generic;

public class AppStartArgs
{

    private static AppStartArgs s_Args;
    public static AppStartArgs Args => s_Args ??= new AppStartArgs(URLParameters.GetSearchParameters());
    private readonly Dictionary < string, string > m_Args;
    
    public string this [ string key ] => m_Args [ key ];
    
    public bool ContainsKey(string key) => m_Args.ContainsKey(key);

    private AppStartArgs ( Dictionary < string, string > args )
    {
        m_Args = args;
    }

}