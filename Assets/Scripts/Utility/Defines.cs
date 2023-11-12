using UnityEngine;

public delegate void VoidVoidDelegate();
public delegate void VoidFloatDelegate(float _value);
public delegate void VoidIntDelegate(int _value);
public delegate void VoidGameObjectDelegate(GameObject _go);
public delegate void VoidBoolDelegate(bool _value);

public enum EPublisherType 
{ 
    NONE = -1, 
    GAME_MANAGER,
    BOSS_CONTROLLER,
    LENGTH 
}

public enum EMessageType
{
    NONE = -1,
    PAUSE,
    PHASE_CHANGE,
    LENGTH
}
