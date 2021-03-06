﻿namespace MoleMole
{
    using System;
    using UnityEngine;

    public class SlideWithScrollOneByOne : MonoBehaviour
    {
        private Transform _contentTrans;
        private float _orginPosition;
        private RectTransform _rectTrans;
        private float _step;
        private float _targetPosition;
        private float _timer;
        public MyScrollRect scrollRect;
        public SetAvatar3DModel SetUpAvatar;
        private const float TRANSIT_LINEAR_LERP_TIME = 1f;

        public void Last()
        {
            this._step = 1f / ((float) this._contentTrans.childCount);
            if ((this._orginPosition - this._step) < 0f)
            {
                this._targetPosition = 1f;
            }
            else
            {
                this._targetPosition = this._orginPosition - this._step;
            }
        }

        public void Next()
        {
            this._step = 1f / ((float) this._contentTrans.childCount);
            if ((this._orginPosition + this._step) > 1f)
            {
                this._targetPosition = 0f;
            }
            else
            {
                this._targetPosition = this._orginPosition + this._step;
            }
        }

        private void OnScrollView()
        {
            Vector2 position = this._rectTrans.TransformPoint((Vector3) this._rectTrans.rect.position);
            Vector2 size = this._rectTrans.TransformVector((Vector3) this._rectTrans.rect.size);
            Rect rect = new Rect(position, size);
            Transform transform = base.transform.Find("Content");
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                RectTransform transform3 = child as RectTransform;
                Vector2 vector3 = transform3.TransformPoint((Vector3) transform3.rect.position);
                Vector2 vector4 = transform3.TransformVector((Vector3) transform3.rect.size);
                Rect other = new Rect(vector3, vector4);
                bool flag = rect.Overlaps(other, true);
                child.gameObject.SetActive(flag);
            }
        }

        public void SetTargetPosition(float targetPosition)
        {
            this._orginPosition = this.scrollRect.horizontalNormalizedPosition;
            this._targetPosition = targetPosition;
        }

        private void Start()
        {
            this._timer = 0f;
            this.scrollRect.horizontalNormalizedPosition = 0f;
            this._orginPosition = this.scrollRect.horizontalNormalizedPosition;
            this._targetPosition = this._orginPosition;
            this._rectTrans = base.transform as RectTransform;
            this._contentTrans = base.transform.Find("Content");
            this._step = 1f / ((float) this._contentTrans.childCount);
            this.OnScrollView();
        }

        private void Update()
        {
            if (this._orginPosition != this._targetPosition)
            {
                this._timer += Time.deltaTime;
                if (this._timer < 1f)
                {
                    this.scrollRect.horizontalNormalizedPosition = Mathf.Lerp(this._orginPosition, this._targetPosition, this._timer / 1f);
                }
                else
                {
                    this.scrollRect.horizontalNormalizedPosition = this._targetPosition;
                    this._orginPosition = this._targetPosition;
                    this._timer = 0f;
                    if (this.SetUpAvatar != null)
                    {
                        this._step = 1f / ((float) this._contentTrans.childCount);
                        this.SetUpAvatar(Mathf.FloorToInt(this._targetPosition / this._step));
                    }
                }
            }
            this.OnScrollView();
        }
    }
}

