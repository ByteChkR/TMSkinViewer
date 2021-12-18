using System.Collections.Generic;

public readonly struct AppStartArgs
{

    private readonly Dictionary < string, string > m_Args;
    
    public string this [ string key ] => m_Args [ key ];

    public AppStartArgs ( Dictionary < string, string > args )
    {
        m_Args = args;
    }

}