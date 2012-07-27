def ElegirProceso(lista):
	#Selecciona el proceso por la mayor prioridad
	maxPrioridad = lista[0]
	
	for p in lista:
		if p.Prioridad > maxPrioridad.Prioridad:
			maxPrioridad = p
			
	return maxPrioridad
	
