using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FrasesDeTodos.Models;
using FrasesDeTodos.Services;
using System.Text;

namespace FrasesDeTodos.MVCExtension.ModelBinders
{
    public class FraseModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext,
                                ModelBindingContext bindingContext)
        {

            var frase = new Frase();

            frase.Texto = Get<string>(controllerContext, bindingContext, "Texto");

            var nombreAutor = Get<string>(controllerContext, bindingContext, "Autor");

            var autor = FrasesService.ObtenerAutor(nombreAutor);

            frase.Autor = autor;

            return frase;
        }

        private TModel Get<TModel>(ControllerContext controllerContext,
                                    ModelBindingContext bindingContext,
                                    string name)
        {
            // Obtengo el Valor del ValueProvider
            ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(name);

            // Genero el Model State con el valor. Esto me permite devolver el valor para nuevos bindings en caso de que 
            // sea invalido, (Ej: Valores string en propiedades int).
            ModelState modelState = new ModelState { Value = valueProviderResult };
            bindingContext.ModelState.Add(name, modelState);

            // Convierto al tipo especifico y la asigno a la metadata
            var model = (TModel)valueProviderResult.ConvertTo(typeof(TModel));
            
            // Obtengo la metadata de la propiedad, necesaria para la validacion
            ModelMetadata metadata = bindingContext.PropertyMetadata[name];
            metadata.Model = model;

            // Usando la Metadada y el valor corro las validaciones
            IEnumerable<ModelValidator> validators = ModelValidatorProviders.Providers.GetValidators(metadata, controllerContext);
            foreach (var validator in validators)
                foreach (var validatorResult in validator.Validate(bindingContext.Model))
                    modelState.Errors.Add(validatorResult.Message);

            //if (modelState.Errors.Count() > 0)
            //{
            //    var messages = new StringBuilder();

            //    foreach (var e in modelState.Errors)
            //    {
            //        messages.AppendLine(e.ErrorMessage);
            //    }

            //    throw new ApplicationException(messages.ToString());
            //}

            return model;
        }
    }
}