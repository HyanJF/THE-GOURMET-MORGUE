using UnityEngine;

public abstract class SocialCondition : ScriptableObject
{
    public abstract bool Check(SocialFSM fsm);
}
