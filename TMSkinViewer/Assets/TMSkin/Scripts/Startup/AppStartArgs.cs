using System.Collections.Generic;

public class AppStartArgs
{

    private static AppStartArgs s_Args;
    private readonly Dictionary < string, string > m_Args;

    public static AppStartArgs Args => s_Args ??= new AppStartArgs( URLParameters.GetSearchParameters() );

    public string this[ string key ] => m_Args[key];

    #region Public

    public bool ContainsKey( string key )
    {
        return m_Args.ContainsKey( key );
    }

    #endregion

    #region Private

    private AppStartArgs( Dictionary < string, string > args )
    {
        m_Args = args;
    }

    #endregion

}
