Model Extension:
			Model Binders (EJ: FraseModelBinder en ModelBinders):
				Toman los valores de el Value Provider y crean modelos.
				Demo: Como creo la instancia de Frase y le obtengo el autor. 
				Tambien muestro error si no esta completa corriendo las validaciones

			Value Providers (EJ: CookiesValueProvider en ValueProviders)
				Para leer de distintos origenes de datos.
				Lo usa el ModelBinder.
				Cambiando la ruta al no pasar el dequien se obtiene de la cookier

Views Extension
			Custom View Engines (Ej: CustomListsViewEngine en CustomViewEngine)
				Demo: Mosrar como se pueden agregar Paths de Busqueda y Sobreescribir los metodos para buscar las vistas.
				Actualmente se usa para redireccionar a la lista a la carpeta correspondiente y a la vista correspondiente del controller.
				Url: /host/Frases/List
									
			Html Helpers (Ej:LabelLeft en UIExtensions)
				Demo: Metodo para dibujar un label a la izquierda de las frases en el listado.
									
			Razor Helpers (Ej: @helper RenderFrase en ListaDeFrases)
			Tambien en APP Code
				Demo: Helper en ListaDeFrases. Mostrar como se puede reutilizar poniendolo en AppCode
									
Controllers Extension
			Action Name Selectors (EJ: FraseEspecificaAttribute en ActionNameSelectors)
				Demo: Mostrar como con un Action Name Selector se selecciona una accion para ejecutar aunque no tenga el mismo nombre, 
				y ademas se le puede poner parametros.
				Url: /host/frases/frase-krako-1
			
			Action Method Selectors (Ej: Ajax Only en ActionMethodSelectorAttribute)
				Demo: Mostrar como con un Method Selector se puede frenar la ejecucion de una accion desde un contexto que no es AJAX.
				Url: /host/Frases/ObtenerTodas?autor=krako
				
			Action Filters (Ej: CacheInvalidationFilter en ActionFilters)
				Filtros que se ejecutan antes o despues de una accion o antes o despues de un result. Se pueden configurar globales
				Demo: Mostrar como entra e invalida el cache.
									
			Exception Filters (Ej: PlainTextExceptionFilter en ExceptionFilters)
				Filtros particulares para el manejo de errores. Se puede redireccionar a alguna pagina de error, loguear, etc.
				Demo: Mostrar como los errores se muestran con texto plano.
									
			Custom Results (EJ: XmlResult en CustomActionResults)
				Crear ActionResults para obtener resultados Customs. Se resuelve aca y no en el controller para poder reutilizar y testear
				Demo: Mostrar como se obtiene el resultado en XML.
				Url: /host/frases/frase-krako-1
			