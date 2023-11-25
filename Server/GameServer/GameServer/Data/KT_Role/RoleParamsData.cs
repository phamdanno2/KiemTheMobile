using ProtoBuf;

namespace Server.Data
{
    /// <summary>
    /// 角色参数
    /// </summary>
    [ProtoContract]
    public class RoleParamsData
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        [ProtoMember(1)]
        public string ParamName = "";

        /// <summary>
        /// 今日经验
        /// </summary>
        [ProtoMember(2)]
        public string ParamValue = "";
    }
}