using System.IO;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.TextTemplating;
using Volo.Abp.VirtualFileSystem;

namespace Mre.Sb.Notificacion.Application
{

    /// <summary>
    /// Generar definiciones de plantillas desde una base de datos
    /// </summary>
    public class DatabaseTemplate : TemplateDefinitionProvider
    {
        public IPlantillaStore TemplateStore { get; }


        public DatabaseTemplate(IPlantillaStore templateStore)
        {
            TemplateStore = templateStore;
        }

     
        public override void Define(ITemplateDefinitionContext context)
        {
            var templates = TemplateStore.ObtenerListaAsync(null).Result;

            foreach (var template in templates)
            {

                var templateDefinition = new TemplateDefinition(template.Id);

                templateDefinition.WithProperty(
                    DatabaseTemplateContentContributor.DatabasePropertyName,
                    template.Id
                );

                context.Add(templateDefinition);
            }
        }
    }
     
    
    public class DatabaseTemplateContentContributor : ITemplateContentContributor, ITransientDependency
    {

        public IPlantillaStore TemplateStore { get; }

        public const string DatabasePropertyName = "Database";

        public DatabaseTemplateContentContributor(IPlantillaStore templateStore)
        {
            this.TemplateStore = templateStore; 
        }

      
        public virtual async Task<string> GetOrNullAsync(TemplateContentContributorContext context)
        {
            var templateId = context.TemplateDefinition.Name;

            var template =  await TemplateStore.ObtenerAsync(templateId);

            if (template != null) {
                return template.Contenido;
            }

            return null;
        }
    }

    /// <summary>
    /// Recuperar plantillas desde archivos 
    /// </summary>
    public class DirectoryContentTemplate : TemplateDefinitionProvider
    {
        
        private readonly IVirtualFileProvider virtualFileProvider;

        public DirectoryContentTemplate(IVirtualFileProvider virtualFileProvider)
        {
            this.virtualFileProvider = virtualFileProvider;
        }

        public override void Define(ITemplateDefinitionContext context)
        { 
            //Getting all files/directories under a directory
            var pathTemplate = NotificationConstants.CarpetaPlantillas;
            var directoryContents = virtualFileProvider
                .GetDirectoryContents(pathTemplate);

            foreach (var templateFile in directoryContents)
            {
                var code = Path.GetFileNameWithoutExtension(templateFile.Name).ToUpper();
                var virtualPath = Path.Combine(pathTemplate, templateFile.Name);

                context.Add(
                 new TemplateDefinition(code) 
                     .WithVirtualFilePath(
                       virtualPath,
                       isInlineLocalized: true)
                 );
            } 
        }
    }

     

}
