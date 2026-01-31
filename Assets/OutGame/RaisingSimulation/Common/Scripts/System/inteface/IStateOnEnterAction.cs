using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateOnEnterAction
{
    public UniTask OnEnterAction();
    public void SetActionStateType();
}
