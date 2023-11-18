//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerRoot
{
    public T SeachState<T>() where T : class;
}
