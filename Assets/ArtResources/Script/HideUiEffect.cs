using UnityEngine;
using System.Collections;

/// <summary>
/// 用于延时隐藏UI EFFECT
/// Author:Ransongsong
/// Time:2016/03/03
/// </summary>
public class HideUiEffect : MonoBehaviour {

    #region Date Variables

    public float _fExistTime;
    private bool _bStartTimer = false;

    private float _fTime = 0.0f;

    #endregion

    #region Awake,Start,Update,OnEnable

    void OnEnable()
    {
        _bStartTimer = true;
        _fTime = 0f;
    }

    private void Update()
    {
        if (_bStartTimer)
        {
            _fTime += Time.deltaTime;

            if (_fTime >= _fExistTime)
            {
                _bStartTimer = false;
                this.gameObject.SetActive(false);
            }
        }
    }
    #endregion
}
