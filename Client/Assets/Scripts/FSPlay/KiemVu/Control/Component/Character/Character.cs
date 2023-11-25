﻿using FSPlay.GameEngine.Logic;
using FSPlay.GameEngine.Sprite;
using FSPlay.KiemVu.Entities.Object;
using FSPlay.KiemVu.Logic.Settings;
using FSPlay.KiemVu.Network;
using FSPlay.KiemVu.Network.Skill;
using FSPlay.KiemVu.UI;
using FSPlay.KiemVu.Utilities.UnityComponent;
using Server.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FSPlay.KiemVu.Entities.Enum;

namespace FSPlay.KiemVu.Control.Component
{
    /// <summary>
    /// Đối tượng nhân vật
    /// </summary>
    public partial class Character : TTMonoBehaviour
    {
        #region Define
        /// <summary>
        /// Đối tượng tham chiếu
        /// </summary>
        public GSprite RefObject { get; set; }

        /// <summary>
        /// Dữ liệu nhân vật
        /// </summary>
        public RoleDataMini Data { get; set; }

        private bool _ShowMPBar;
        /// <summary>
        /// Hiển thị thanh khí
        /// </summary>
        public bool ShowMPBar
        {
            get
            {
                return this._ShowMPBar;
            }
            set
            {
                this._ShowMPBar = value;
                
                if (this.UIHeader != null)
                {
                    this.UIHeader.IsShowMPBar = value;
                }
            }
        }

        private bool _ShowMinimapName;
        /// <summary>
        /// Hiện tên ở bản đồ thu nhỏ
        /// </summary>
        public bool ShowMinimapName
        {
            get
            {
                return this._ShowMinimapName;
            }
            set
            {
                this._ShowMinimapName = value;

                if (this.UIMinimapReference != null)
                {
                    this.UIMinimapReference.ShowName = value;
                }
            }
        }

        private bool _ShowMinimapIcon;
        /// <summary>
        /// Hiển thị Icon ở bản đồ nỏ
        /// </summary>
        public bool ShowMinimapIcon
        {
            get
            {
                return this._ShowMinimapIcon;
            }
            set
            {
                this._ShowMinimapIcon = value;

                if (this.UIMinimapReference != null)
                {
                    this.UIMinimapReference.ShowIcon = value;
                }
            }
        }

        private Color _NameColor;
        /// <summary>
        /// Màu tên đối tượng
        /// </summary>
        public Color NameColor
        {
            get
            {
                return this._NameColor;
            }
            set
            {
                this._NameColor = value;
                if (this.UIHeader != null)
                {
                    this.UIHeader.NameColor = value;
                }
            }
        }

        private Color _HPBarColor;
        /// <summary>
        /// Màu thanh máu đối tượng
        /// </summary>
        public Color HPBarColor
        {
            get
            {
                return this._HPBarColor;
            }
            set
            {
                this._HPBarColor = value;
                if (this.UIHeader != null)
                {
                    this.UIHeader.HPBarColor = value;
                }
            }
        }

        private Color _MinimapNameColor;
        /// <summary>
        /// Màu của tên đối tượng trong bản đồ thu nhỏ
        /// </summary>
        public Color MinimapNameColor
        {
            get
            {
                return this._MinimapNameColor;
            }
            set
            {
                this._MinimapNameColor = value;

                if (this.UIMinimapReference != null)
                {
                    this.UIMinimapReference.NameColor = value;
                }
            }
        }

        private Vector2 _MinimapIconSize;
        /// <summary>
        /// Kích thước ảnh ở bản đồ thu nhỏ
        /// </summary>
        public Vector2 MinimapIconSize
        {
            get
            {
                return this._MinimapIconSize;
            }
            set
            {
                this._MinimapIconSize = value;

                if (this.UIMinimapReference != null)
                {
                    this.UIMinimapReference.IconSize = value;
                }
            }
        }

        private Direction _Direction = Direction.NONE;
        /// <summary>
        /// Hướng quay hiện tại
        /// </summary>
        public Direction Direction
        {
            get
            {
                return this._Direction;
            }

            set
            {
                if (this._Direction == value)
                {
                    return;
                }
                else
                {
                    this._Direction = value;
                    this.ResumeCurrentAction();
                }
            }
        }

        /// <summary>
        /// Khoảng thời gian thực hiện động tác bay
        /// </summary>
        public float JumpDuration { get; set; } = 1f;

        /// <summary>
        /// Sự kiện khi đối tượng bị xóa
        /// </summary>
        public Action Destroyed { get; set; }

        /// <summary>
        /// Độ trong suốt hiện tại của đối tượng
        /// </summary>
        public float CurrentAlpha
        {
            get
            {
                if (this.groupColor != null)
                {
                    return this.groupColor.Alpha;
                }
                else
                {
                    return this.MaxAlpha;
                }
            }
        }

        /// <summary>
        /// Độ trong suốt cực đại
        /// </summary>
        public float MaxAlpha { get; set; } = 1f;
        #endregion

        /// <summary>
        /// Đã chạy hàm Start chưa
        /// </summary>
        private bool isStarted = false;

        /// <summary>
        /// Đánh dấu vừa thực hiện hàm Awake
        /// </summary>
        private bool justAwaken = false;

        #region Init
        /// <summary>
        /// Group Transparency
        /// </summary>
        private GroupColor groupColor;

        /// <summary>
        /// Đối tượng phát âm thanh
        /// </summary>
        private AudioPlayer audioPlayer;

        /// <summary>
        /// Hàm này gọi đến khi đối tượng được tạo ra
        /// </summary>
        private void Awake()
        {
            this.groupColor = this.gameObject.GetComponent<GroupColor>();
            this.animation = this.gameObject.GetComponent<CharacterAnimation2D>();
            this.animation.Head = this.Head.GetComponent<SpriteRenderer>();
            this.animation.Hair = this.Hair.GetComponent<SpriteRenderer>();
            this.animation.Armor = this.Body.GetComponent<SpriteRenderer>();
            this.animation.LeftHand = this.LeftArm.GetComponent<SpriteRenderer>();
            this.animation.RightHand = this.RightArm.GetComponent<SpriteRenderer>();
            this.animation.LeftWeapon = this.LeftWeapon.GetComponent<SpriteRenderer>();
            this.animation.RightWeapon = this.RightWeapon.GetComponent<SpriteRenderer>();
            this.animation.Mantle = this.Coat.GetComponent<SpriteRenderer>();
            this.animation.HorseHead = this.HorseHead.GetComponent<SpriteRenderer>();
            this.animation.HorseBody = this.HorseBody.GetComponent<SpriteRenderer>();
            this.animation.HorseTail = this.HorseTail.GetComponent<SpriteRenderer>();
            this.animation.LeftWeaponEnhanceEffects = this.LeftWeaponEnhanceEffects.GetComponent<WeaponEnhanceEffect_Particle>();
            this.animation.RightWeaponEnhanceEffects = this.RightWeaponEnhanceEffects.GetComponent<WeaponEnhanceEffect_Particle>();

            this.Trail_Head = this.Head.GetComponent<SpriteTrailRenderer>();
            this.Trail_Hair = this.Hair.GetComponent<SpriteTrailRenderer>();
            this.Trail_Body = this.Body.GetComponent<SpriteTrailRenderer>();
            this.Trail_LeftArm = this.LeftArm.GetComponent<SpriteTrailRenderer>();
            this.Trail_RightArm = this.RightArm.GetComponent<SpriteTrailRenderer>();
            this.Trail_LeftWeapon = this.LeftWeapon.GetComponent<SpriteTrailRenderer>();
            this.Trail_RightWeapon = this.RightWeapon.GetComponent<SpriteTrailRenderer>();
            this.Trail_Coat = this.Coat.GetComponent<SpriteTrailRenderer>();
            this.Trail_HorseHead = this.HorseHead.GetComponent<SpriteTrailRenderer>();
            this.Trail_HorseBody = this.HorseBody.GetComponent<SpriteTrailRenderer>();
            this.Trail_HorseTail = this.HorseTail.GetComponent<SpriteTrailRenderer>();

            this.audioPlayer = this.gameObject.GetComponent<AudioPlayer>();

            this.justAwaken = true;
        }

        /// <summary>
        /// Hàm này gọi đến ở Frame đầu tiên
        /// </summary>
        private void Start()
        {
            this.InitAction();
            this.isStarted = true;

            this.DisplayUI();
            this.UpdateHP();

            /// Nếu là động tác cưỡi ngựa thì chuyển sang bóng ngựa
            if (this.Data.IsRiding)
            {
                this.Shadow_Horse.gameObject.SetActive(true);
                this.Shadow.gameObject.SetActive(false);
            }
            else
            {
                this.Shadow_Horse.gameObject.SetActive(false);
                this.Shadow.gameObject.SetActive(true);
            }

            this.justAwaken = false;
        }

        /// <summary>
        /// Hàm này gọi đến khi đối tượng được kích hoạt
        /// </summary>
        private void OnEnable()
        {
            if (this.justAwaken)
            {
                this.justAwaken = false;
                return;
            }

            this.StartCoroutine(this.ExecuteBackgroundWork());
            if (this.isStarted)
            {
                this.StartCoroutine(this.ExecuteSkipFrames(1, () => {
                    this.DisplayUI();
                    this.UpdateHP();
                }));
                this.InitAction();
            }
            this.InitEvents();
        }

        /// <summary>
        /// Hàm này gọi đến khi đối tượng bị hủy
        /// </summary>
        private void OnDisable()
        {
            if (this.justAwaken)
            {
                this.justAwaken = false;
                return;
            }
        }
		#endregion
	}
}
