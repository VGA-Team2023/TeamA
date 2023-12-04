//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using Cysharp.Threading.Tasks;

/// <summary>
/// Inputの情報を提供するクラス
/// </summary>
public class InputProvider
{
    public Vector3 MoveDir => _moveDir;
    public Vector2 EimDir => _eimDir;
    public static InputProvider Instance => _instance;

    [Tooltip("InputSystemで生成したクラス")]
    private GameInputs _inputMap;
    [Tooltip("移動する向き")]
    private Vector3 _moveDir;
    [Tooltip("エイムの入力している向き")]
    private Vector2 _eimDir;
    [Tooltip("入力直後")]
    private Dictionary<InputType, Action> _onEnterInputDic = new Dictionary<InputType, Action>();
    [Tooltip("入力直後(Async)")]
    private Dictionary<InputType, Func<UniTaskVoid>> _onEnterInputAsyncDic = new Dictionary<InputType, Func<UniTaskVoid>>();
    [Tooltip("入力解除")]
    private Dictionary<InputType, Action> _onExitInputDic = new Dictionary<InputType, Action>();
    [Tooltip("入力直後(Async)")]
    private Dictionary<InputType, Func<UniTaskVoid>> _onExitInputAsyncDic = new Dictionary<InputType, Func<UniTaskVoid>>();
    [Tooltip("入力中")]
    private Dictionary<InputType, bool> _isStayInputDic = new Dictionary<InputType, bool>();

    private bool _isInstanced = false;
    private static InputProvider _instance = new InputProvider();

    public InputProvider() 
    {
        Initialize();
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Initialize()
    {
        _inputMap = new GameInputs();
        _inputMap.Enable();
        InirializeInput();
        _inputMap.Player.Eim.performed += context => _eimDir = context.ReadValue<Vector2>(); 
        _inputMap.Player.Eim.canceled += context => _eimDir = Vector2.zero; 
        _inputMap.Player.Move.performed += context => _moveDir = context.ReadValue<Vector2>();
        _inputMap.Player.Move.canceled += context => _moveDir = Vector3.zero;
        _inputMap.Player.Jump.performed += context => ExecuteInput(InputType.Jump, InputMode.Enter);
        _inputMap.Player.Jump.canceled += context => ExecuteInput(InputType.Jump, InputMode.Exit);
        _inputMap.Player.Attack.performed += context => ExecuteInput(InputType.Attack, InputMode.Enter);
        _inputMap.Player.Attack.canceled += context => ExecuteInput(InputType.Attack, InputMode.Exit);
        _inputMap.Player.Dash.performed += context => ExecuteInput(InputType.Dash, InputMode.Enter);
        _inputMap.Player.Dash.canceled += context => ExecuteInput(InputType.Dash, InputMode.Exit);
        _inputMap.Player.Interact.performed += context => ExecuteInput(InputType.Interact, InputMode.Enter);
        _inputMap.Player.Interact.canceled += context => ExecuteInput(InputType.Interact, InputMode.Exit);

        _isInstanced = true;
    }

    /// <summary>
    /// 入力処理の初期化を行う
    /// </summary>
    private void InirializeInput()
    {
        if (_isInstanced)
        {
            for (int i = 0; i < Enum.GetValues(typeof(InputType)).Length; i++)
            {
                _onEnterInputDic[(InputType)i] = null;
                _onEnterInputAsyncDic[(InputType)i] = null;
                _onExitInputDic[(InputType)i] = null;
                _onExitInputAsyncDic[(InputType)i] = null;
                _isStayInputDic[(InputType)i] = false;
            }
            return;
        }
        for (int i = 0; i < Enum.GetValues(typeof(InputType)).Length; i++)
        {
            _onEnterInputDic.Add((InputType)i, null);
            _onEnterInputAsyncDic.Add((InputType)i, null);
            _onExitInputDic.Add((InputType)i, null);
            _onExitInputAsyncDic.Add((InputType)i, null);
            _isStayInputDic.Add((InputType)i, false);
        }
    }

    /// <summary>
    /// 入力開始入力解除したときに呼ばれる関数
    /// </summary>
    /// <param name="input"></param>
    private void ExecuteInput(InputType input, InputMode type)
    {
        switch (type)
        {
            case InputMode.Enter:
                //入力開始処理を実行する
                SetStayInput(input, true);
                _onEnterInputDic[input]?.Invoke();
                _onEnterInputAsyncDic[input]?.Invoke();
                break;
            case InputMode.Exit:
                // 入力解除処理を実行する
                SetStayInput(input, false);
                _onExitInputDic[input]?.Invoke();
                _onExitInputAsyncDic[input]?.Invoke();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// そのInputTypeが入力中かどうかフラグを返す
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public bool GetStayInput(InputType type)
    {
        return _isStayInputDic[type];
    }

    /// <summary>
    ///特定の入力で呼び出すActionを登録する
    /// </summary>
    public void SetEnterInput(InputType type, Action action)
    {
        _onEnterInputDic[type] += action;
    }

    public void SetEnterInputAsync(InputType type, Func<UniTaskVoid> func) 
    {
        _onEnterInputAsyncDic[type] += func;
    }

    private void SetStayInput(InputType type, bool isBool)
    {
        _isStayInputDic[type] = isBool;
    }

    /// <summary>
    ///特定の入力終わった時に呼び出すActionを登録する
    /// </summary>
    public void SetExitInput(InputType type, Action action)
    {
        _onExitInputDic[type] += action;
    }

    public void SetExitInputAsync(InputType type, Func<UniTaskVoid> func)
    {
        _onExitInputAsyncDic[type] += func;
    }

    /// <summary>
    /// 特定の入力で呼び出される登録したActionを削除する
    /// </summary>
    public void LiftEnterInput(InputType type, Action action)
    {
        _onEnterInputDic[type] -= action;
    }

    public void LiftEnterInputAsync(InputType type, Func<UniTaskVoid> func)
    {
        _onEnterInputAsyncDic[type] -= func;
    }

    /// <summary>
    ///特定の入力終わった時に呼び出される登録したActionを削除する
    /// </summary>
    public void LiftExitInput(InputType type, Action action)
    {
        _onExitInputDic[type] -= action;
    }

    public void LiftExitInputAsync(InputType type, Func<UniTaskVoid> func)
    {
        _onExitInputAsyncDic[type] -= func;
    }


    /// <summary>
    /// 入力のタイミング
    /// </summary>
    public enum InputMode
    {
        /// <summary>入力時</summary>
        Enter,
        /// <summary>入力終了時</summary>
        Exit,
    }

    /// <summary>
    /// 入力の種類
    /// </summary>
    public enum InputType
    {
        /// <summary>キャンセルの処理</summary>
        Cancel,
        /// <summary>インタラクト</summary>
        Interact,
        /// <summary>攻撃</summary>
        Attack,
        /// <summary>ジャンプをする</summary>
        Jump,
        /// <summary>ダッシュ</summary>
        Dash,
    }
}
