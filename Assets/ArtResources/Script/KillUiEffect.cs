//=========================================================================
// File: KillUiEffect.cs
//
// Summary:临时脚本挂载在特效上 播放一定时间后 自动删除.
//
// Author: YuLiang
// 
// Created Date:   2016-01-23
//
//=========================================================================
// This file is part of Client
//
//
// CopyRight (c)
// 
//
//
//=========================================================================



using UnityEngine;
using UnityEngine.UI;
using Assets.Script.Com.XYGame.Module.Common;

namespace Assets.Script.Com.XYGame.Module.Common
{
    public class KillUiEffect : MonoBehaviour
    {

        #region Date Variables

        public float _fExistTime;
        private bool _bStartTimer = false;

        private float _fTime = 0.0f;

        #endregion



        #region Awake,Start,Update,OnEnable

        private void OnDisable()
        {
            _bStartTimer = false;
            Destroy(this.gameObject);
        }

        private void Start()
        {
            _bStartTimer = true;
        }

        private void Update()
        {
            if (_bStartTimer)
            {
                _fTime += Time.deltaTime;

                if (_fTime >= _fExistTime)
                {
                    Destroy(this.gameObject);
                }
            }
        }

        public void UpdateUi(float fExistTime)
        {
            _fExistTime = fExistTime;

            _bStartTimer = true;
        }

        #endregion
    }
}