using UnityEngine;
using FSPlay.GameEngine.Logic;
using FSPlay.GameEngine.Sprite;
using Server.Data;
using FSPlay.GameFramework.Logic;
using FSPlay.KiemVu.Factory;
using FSPlay.KiemVu.Control.Component;
using FSPlay.KiemVu;

namespace FSPlay.GameEngine.Scene
{
	/// <summary>
	/// Quản lý NPC
	/// </summary>
	public partial class GScene
    {
        #region Quản lý NPC
        /// <summary>
        /// Tải NPC vào map
        /// </summary>
        /// <param name="npPosXmlNode"></param>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <returns></returns>
        private bool AddNewNPCEx(NPCRole npcData)
		{
            int roleID = SpriteBaseIds.NpcBaseId + npcData.NPCID;
            string spriteName = string.Format("Role_{0}", roleID);

            GSprite npc = new GSprite();
            npc.BaseID = SpriteBaseIds.NpcBaseId + npcData.NPCID;
			npc.SpriteType = GSpriteTypes.NPC;

            string npcName = npcData.Name;
            string npcTitle = npcData.Title;


            GameObject go = GameObject.Find(spriteName);
            if (go != null && go.GetComponent<Monster>() != null)
            {
                KTObjectPoolManager.Instance.ReturnToPool(go);
            }

            this.LoadSprite(
                npc,
                roleID,
                spriteName,
                null,
                null,
                npcData,
                null,
                null,
                (KiemVu.Entities.Enum.Direction) npcData.Dir,
                npcData.PosX,
                npcData.PosY
            );

            npc.Start();


            /// Tìm đối tượng cũ
            GameObject oldObject = KTObjectPoolManager.Instance.FindSpawn(x => x.name == spriteName);
            /// Nếu tồn tại
            if (oldObject != null)
            {
                /// Trả lại Pool
                KTObjectPoolManager.Instance.ReturnToPool(oldObject);
                //KTGlobal.AddNotification("Duplicate NPC obj => " + npcData.Name);
            }

            //Monster mons = Object2DFactory.MakeNPC();
            Monster mons = KTObjectPoolManager.Instance.Instantiate<Monster>("NPC");
            mons.name = spriteName;

            /// Gắn tham chiếu
            mons.RefObject = npc;

            mons.ShowHPBar = false;
            mons.ShowElemental = false;
            ColorUtility.TryParseHtmlString("#f5ff2e", out Color minimapNameColor);
            mons.MinimapNameColor = minimapNameColor;
            mons.ShowMinimapIcon = npcData.VisibleOnMinimap;
            mons.ShowMinimapName = npcData.VisibleOnMinimap;
            mons.MinimapIconSize = new Vector2(100, 100);
            ColorUtility.TryParseHtmlString("#f5ff2e", out Color nameColor);
            mons.NameColor = nameColor;
            //mons.UIHeaderOffset = new Vector2(0, 200);

            /// Res
            mons.StaticID = npcData.ResID;
            mons.ResID = FSPlay.KiemVu.Loader.Loader.ListMonsters[npcData.ResID].ResID;
            mons.UpdateData();

            GameObject role2D = mons.gameObject;
            npc.Role2D = role2D;
            role2D.transform.localPosition = new Vector2(npcData.PosX, npcData.PosY);

            /// Thực hiện động tác đứng
            npc.DoStand();

            /// Cập nhật trạng thái cho NPC tương ứng nếu có
            NPCTaskState taskState = KTGlobal.FindTaskByNPCResID(npcData.ResID);
            /// Nếu có nhiệm vụ
            if (taskState != null)
            {
                this.UpdateNPCTaskState(npc, (NPCTaskStates) taskState.TaskState);
            }

            //KTDebug.LogError("Load NPC DONE => " + npcData.Name);

			return true;
		}

        /// <summary>
        /// Cập nhật trạng thái nhiệm vụ của NPC
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="state"></param>
        public void UpdateNPCTaskState(GSprite sprite, NPCTaskStates state)
        {
            //KTDebug.LogError("UpdateNPCTaskState -> " + state.ToString());

            switch (state)
            {
                case NPCTaskStates.None:
                {
                    KTGlobal.HideNPCTaskState(sprite);
                    break;
                }
                case NPCTaskStates.ToReceive_MainQuest:
                case NPCTaskStates.ToReceive_SubQuest:
                case NPCTaskStates.ToReceive_DailyQuest:
                {
                    KTGlobal.ShowNPCTaskState(sprite, state);
                    break;
                }
                case NPCTaskStates.ToReturn_MainQuest:
                case NPCTaskStates.ToReturn_SubQuest:
                case NPCTaskStates.ToReturn_DailyQuest:
                {
                    KTGlobal.ShowNPCTaskState(sprite, state);
                    break;
                }
            }

            /// Cập nhật trạng thái ở Minimap
            if (sprite.ComponentMonster != null)
            {
                sprite.ComponentMonster.UpdateMinimapNPCTaskState(state);
            }
        }

        /// <summary>
        /// Thêm NPC vào Map
        /// </summary>
        /// <param name="npPosXmlNode"></param>
        /// <param name="name"></param>
        /// <param name="title"></param>
        /// <param name="gridX"></param>
        /// <param name="gridY"></param>
        /// <param name="mapCode"></param>
        /// <returns></returns>
        private bool AddNewNPC(NPCRole npc)
        {
            //KTDebug.LogError("Add NPC -> Name: " + npc.Name + ", Pos: (" + npc.PosX + "," + npc.PosY + ")");

            /// Tên đối tượng
            int roleID = SpriteBaseIds.NpcBaseId + npc.NPCID;
            string spriteName = string.Format("Role_{0}", roleID);
            /// Tìm đối tượng cũ
            GameObject oldObject = KTObjectPoolManager.Instance.FindSpawn(x => x.name == spriteName);
            /// Nếu tồn tại
            if (oldObject != null)
            {
                //KTGlobal.AddNotification("Duplicate NPC obj => " + npc.Dir);
                /// Bỏ qua không xử lý
                return false;
            }

            return this.AddNewNPCEx(npc);
        }

        /// <summary>
        /// Xóa NPC 
        /// </summary>
        /// <param name="npcID"></param>
        /// <param name="mapCode"></param>
        /// <returns></returns>
        private bool DelNpc(int npcID, int mapCode)
        {
            if (mapCode != Global.Data.RoleData.MapCode)
            {
                return false;
            }

            int roleID = SpriteBaseIds.NpcBaseId + npcID;
            GSprite npc = this.FindSprite(roleID);

            if (null != npc)
            {
                //KTDebug.LogError("Del NPC => " + npc.RoleName);
                KTGlobal.RemoveObject(npc, true);
            }

            return true;
        }

        #endregion
    }
}
