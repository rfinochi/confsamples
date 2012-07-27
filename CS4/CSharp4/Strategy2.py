def ElegirProceso(lista):
	#Selecciona el proceso por el pid mas bajo
	minPid = lista[0]
	
	for p in lista:
		if p.PID < minPid.PID:
			minPid = p
			
	return minPid