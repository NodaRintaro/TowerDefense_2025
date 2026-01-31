using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateOnExitAction
{
    public UniTask OnExitAction();
    public void SetActionStateType();
}
