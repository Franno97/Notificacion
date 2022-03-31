using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Mre.Sb.Notificacion.EntityFrameworkCore
{
    public class NotificationModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public NotificationModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}