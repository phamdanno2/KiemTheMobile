using FSPlay.GameEngine.Logic;
using FSPlay.KiemVu;
using FSPlay.KiemVu.Factory;
using FSPlay.KiemVu.UI;
using FSPlay.KiemVu.UI.CoreUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FSPlay.GameEngine.Sprite
{
    /// <summary>
    /// Quản lý UI
    /// </summary>
    public partial class GSprite
    {
        #region Dòng chữ bay trên đầu
        /// <summary>
        /// Thời gian chờ mỗi lần hiển thị sát thương
        /// </summary>
        private const float TextFlyAddTime = 0.1f;

        /// <summary>
        /// Danh sách chữ bay trên đầu đang chờ thực thi
        /// </summary>
        private readonly Queue<Action> queueFlyingText = new Queue<Action>();

        /// <summary>
        /// Thời điểm thực thi biểu diễn chữ bay trước đó
        /// </summary>
        private long lastProcessHeadTextTick = 0;

        /// <summary>
        /// Thực hiện biểu diễn chữ bay phía trên đầu
        /// </summary>
        private void ProcessHeadText()
        {
            /// Nếu danh sách rỗng
            if (this.queueFlyingText.Count <= 0)
            {
                return;
            }

            /// Nếu chưa đến thời gian
            if (KTGlobal.GetCurrentTimeMilis() - this.lastProcessHeadTextTick < GSprite.TextFlyAddTime * 1000)
            {
                return;
            }

            /// Cập nhật thời điểm thực thi biểu diễn chữ bay
            this.lastProcessHeadTextTick = KTGlobal.GetCurrentTimeMilis();

            /// Thực thi biểu diễn đối tượng ở đầu hàng đợi
            Action action = this.queueFlyingText.Dequeue();
            action?.Invoke();
        }

        /// <summary>
        /// Hiển thị chữ bay phía trên đầu
        /// </summary>
        /// <param name="type"></param>
        /// <param name="content"></param>
        public void AddHeadText(FSPlay.KiemVu.Entities.Enum.DamageType type, string content, Color color = default)
        {
            KiemVu.Control.Component.Character character = this.ComponentCharacter;
            KiemVu.Control.Component.Monster monster = this.ComponentMonster;
            CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();

            switch (type)
            {
                case FSPlay.KiemVu.Entities.Enum.DamageType.DAMAGE_TAKEN:
                {
                    if (character != null)
                    {
                        this.queueFlyingText.Enqueue(() => {
                            //UIFlyingText text = canvas.UITextLeaderDamageTakenPool.Spawn();
                            UIFlyingText text = KTUIElementPoolManager.Instance.Instantiate<UIFlyingText>("UITextLeaderDamageTaken");
                            if (text == null)
                            {
                                return;
                            }
                            canvas.AddUnderLayerUI(text);
                            text.ReferenceObject = character.gameObject;
                            text.Offset = new Vector2(0, 160);
                            text.Text = content;
                            text.Duration = 1.5f;
                            text.Play();
                        });
                    }
                    break;
                }
                case FSPlay.KiemVu.Entities.Enum.DamageType.CRIT_DAMAGE_DEALT:
                {
                    if (character != null)
                    {
                        this.queueFlyingText.Enqueue(() => {
                            //UIFlyingText text = canvas.UITextLeaderDamageTakenCritPool.Spawn();
                            UIFlyingText text = KTUIElementPoolManager.Instance.Instantiate<UIFlyingText>("UITextLeaderDamageCrit");
                            if (text == null)
                            {
                                return;
                            }
                            canvas.AddUnderLayerUI(text);
                            text.ReferenceObject = character.gameObject;
                            text.Offset = new Vector2(0, 160);
                            text.Text = content;
                            text.Duration = 1.5f;
                            text.Play();
                        });
                    }
                    else if (monster != null)
                    {
                        this.queueFlyingText.Enqueue(() => {
                            //UIFlyingText text = canvas.UITextLeaderDamageCritPool.Spawn();
                            UIFlyingText text = KTUIElementPoolManager.Instance.Instantiate<UIFlyingText>("UITextLeaderDamageCrit");
                            if (text == null)
                            {
                                return;
                            }
                            canvas.AddUnderLayerUI(text);
                            text.ReferenceObject = monster.gameObject;
                            text.Offset = new Vector2(0, 120);
                            text.Text = content;
                            text.Duration = 1.5f;
                            text.Play();
                        });
                    }
                    break;
                }
                case FSPlay.KiemVu.Entities.Enum.DamageType.DAMAGE_DEALT:
                {
                    if (character != null)
                    {
                        this.queueFlyingText.Enqueue(() => {
                            //UIFlyingText text = canvas.UITextLeaderDamageTakenPool.Spawn();
                            UIFlyingText text = KTUIElementPoolManager.Instance.Instantiate<UIFlyingText>("UITextLeaderDamage");
                            if (text == null)
                            {
                                return;
                            }
                            canvas.AddUnderLayerUI(text);
                            text.ReferenceObject = character.gameObject;
                            text.Offset = new Vector2(0, 160);
                            text.Text = content;
                            text.Duration = 1.5f;
                            text.Play();
                        });
                    }
                    else if (monster != null)
                    {
                        this.queueFlyingText.Enqueue(() => {
                            //UIFlyingText text = canvas.UITextLeaderDamagePool.Spawn();
                            UIFlyingText text = KTUIElementPoolManager.Instance.Instantiate<UIFlyingText>("UITextLeaderDamage");
                            if (text == null)
                            {
                                return;
                            }
                            canvas.AddUnderLayerUI(text);
                            text.ReferenceObject = monster.gameObject;
                            text.Offset = new Vector2(0, 120);
                            text.Text = content;
                            text.Duration = 1.5f;
                            text.Play();
                        });
                    }
                    break;
                }
                case FSPlay.KiemVu.Entities.Enum.DamageType.DODGE:
                {
                    if (character != null)
                    {
                        this.queueFlyingText.Enqueue(() => {
                            //UIFlyingText text = canvas.UITextMissPool.Spawn();
                            UIFlyingText text = KTUIElementPoolManager.Instance.Instantiate<UIFlyingText>("UITextMiss");
                            if (text == null)
                            {
                                return;
                            }
                            canvas.AddUnderLayerUI(text);
                            text.ReferenceObject = character.gameObject;
                            text.Offset = new Vector2(0, 160);
                            text.Text = "Né";
                            text.Duration = 1.5f;
                            text.Play();
                        });
                    }
                    else if (monster != null)
                    {
                        this.queueFlyingText.Enqueue(() => {
                            //UIFlyingText text = canvas.UITextMissPool.Spawn();
                            UIFlyingText text = KTUIElementPoolManager.Instance.Instantiate<UIFlyingText>("UITextMiss");
                            if (text == null)
                            {
                                return;
                            }
                            canvas.AddUnderLayerUI(text);
                            text.ReferenceObject = monster.gameObject;
                            text.Offset = new Vector2(0, 120);
                            text.Text = "Né";
                            text.Duration = 1.5f;
                            text.Play();
                        });
                    }
                    break;
                }
                case FSPlay.KiemVu.Entities.Enum.DamageType.IMMUNE:
                {
                    if (character != null)
                    {
                        this.queueFlyingText.Enqueue(() => {
                            //UIFlyingText text = canvas.UITextMissPool.Spawn();
                            UIFlyingText text = KTUIElementPoolManager.Instance.Instantiate<UIFlyingText>("UITextMiss");
                            if (text == null)
                            {
                                return;
                            }
                            canvas.AddUnderLayerUI(text);
                            text.ReferenceObject = character.gameObject;
                            text.Offset = new Vector2(0, 160);
                            text.Text = "Miễn dịch";
                            text.Duration = 1.5f;
                            text.Play();
                        });
                    }
                    else if (monster != null)
                    {
                        this.queueFlyingText.Enqueue(() => {
                            //UIFlyingText text = canvas.UITextMissPool.Spawn();
                            UIFlyingText text = KTUIElementPoolManager.Instance.Instantiate<UIFlyingText>("UITextMiss");
                            if (text == null)
                            {
                                return;
                            }
                            canvas.AddUnderLayerUI(text);
                            text.ReferenceObject = monster.gameObject;
                            text.Offset = new Vector2(0, 120);
                            text.Text = "Miễn dịch";
                            text.Duration = 1.5f;
                            text.Play();
                        });
                    }
                    break;
                }
                case FSPlay.KiemVu.Entities.Enum.DamageType.ADJUST:
                {
                    if (character != null)
                    {
                        this.queueFlyingText.Enqueue(() => {
                            //UIFlyingText text = canvas.UITextMissPool.Spawn();
                            UIFlyingText text = KTUIElementPoolManager.Instance.Instantiate<UIFlyingText>("UITextMiss");
                            if (text == null)
                            {
                                return;
                            }
                            canvas.AddUnderLayerUI(text);
                            text.ReferenceObject = character.gameObject;
                            text.Offset = new Vector2(0, 160);
                            text.Text = "Hóa giải";
                            text.Duration = 1.5f;
                            text.Play();
                        });
                    }
                    else if (monster != null)
                    {
                        this.queueFlyingText.Enqueue(() => {
                            //UIFlyingText text = canvas.UITextMissPool.Spawn();
                            UIFlyingText text = KTUIElementPoolManager.Instance.Instantiate<UIFlyingText>("UITextMiss");
                            if (text == null)
                            {
                                return;
                            }
                            canvas.AddUnderLayerUI(text);
                            text.ReferenceObject = monster.gameObject;
                            text.Offset = new Vector2(0, 120);
                            text.Text = "Hóa giải";
                            text.Duration = 1.5f;
                            text.Play();
                        });
                    }
                    break;
                }
            }
        }

        #endregion
    }
}
