using UnityEngine;

public delegate void VoidVoidDelegate();
public delegate void VoidFloatDelegate();
public delegate void VoidIntDelegate();
public delegate void VoidGameObjectDelegate(GameObject _go);

public enum EPublisherType 
{ 
    NONE = -1, 
    GAME_MANAGER,
    LENGTH 
}

public enum EMessageType
{
    NONE = -1,
    PAUSE,
    LENGTH
}
