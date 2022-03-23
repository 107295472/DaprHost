using FreeSql.DataAnnotations;

namespace Domain.Entities
{
    /// <summary>
    /// 操作日志
    /// </summary>
	[Table(Name = "ad_login_log")]
    public class LoginLogEntity : LogAbstract
    {
    }
}