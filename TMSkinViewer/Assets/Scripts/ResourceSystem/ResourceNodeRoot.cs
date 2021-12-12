public class ResourceNodeRoot : ResourceNode
{

    #region Public

    public ResourceNodeRoot( string name, ResourceOrigin origin ) : base( name, null, ResourceType.Directory, origin )
    {
    }

    public ResourceNode CreateNode( string path, ResourceType type )
    {
        if ( path == null )
        {
            return null;
        }

        string[] pathParts = path.Split( '/' );

        ResourceNode currentNode = this;

        for ( int i = 0; i < pathParts.Length; i++ )
        {
            string pathPart = pathParts[i];

            ResourceType childType = i == pathParts.Length - 1 ? type : ResourceType.Directory;

            if ( currentNode.HasNode( pathPart, childType ) )
            {
                currentNode = currentNode.GetNode( pathPart, childType );
            }
            else
            {
                currentNode = currentNode.AddChild( pathPart, childType );
            }
        }

        return currentNode;
    }

    public ResourceNode FindNode( string path, ResourceType type )
    {
        if ( path == null )
        {
            return null;
        }

        string[] pathParts = path.Split( '/' );

        ResourceNode currentNode = this;

        for ( int i = 0; i < pathParts.Length; i++ )
        {
            string pathPart = pathParts[i];

            ResourceType childType = i == pathParts.Length - 1 ? type : ResourceType.Directory;
            currentNode = currentNode.GetNode( pathPart, childType );
        }

        return currentNode;
    }

    #endregion

}
