using Microsoft.Extensions.Localization;
using Mre.Sb.Notificacion.Localization;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Mre.Sb.Notificacion
{
    public class PlantillaManager : DomainService
    {
        private readonly IStringLocalizer<NotificationResource> localizer;

        private readonly IPlantillaRepository repository;


        public PlantillaManager(IPlantillaRepository repository,
            IStringLocalizer<NotificationResource> localizer)
        {
            this.repository = repository;
            this.localizer = localizer;

        }

        public async virtual Task ValidarCrearAsync(string input)
        {

            var exist = await repository.ExisteAsync(input.ToUpper());
            if (exist)
            {
                var msg = string.Format(localizer["Plantilla:Existe"], input);
                throw new UserFriendlyException(msg);
            }
        }

        public  virtual Task<Plantilla> InsertAsync(Plantilla entity, bool autoSave = false, CancellationToken cancellationToken = default) {

            return repository.InsertAsync(entity, autoSave, cancellationToken);
        }
    }

}
