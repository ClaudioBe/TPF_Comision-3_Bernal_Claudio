
using System;
using System.Collections.Generic;
namespace DeepSpace
{

	class Estrategia
	{
		
		//Calcula y retorna un texto con la distancia del camino que existe
		//entre el planeta del Bot y la raíz.
		
		public String Consulta1(ArbolGeneral<Planeta> arbol)
		{
			var c = new Cola<ArbolGeneral<Planeta>>();
			ArbolGeneral<Planeta> arbolAux;
			int distancia = 0;
			
			// encolamos raiz
			c.encolar(arbol);
			// encolamos null
			c.encolar(null);						
			
			// procesamos cola
			while(!c.esVacia()){
				arbolAux = c.desencolar();
				
				if(arbolAux == null){
					if(!c.esVacia()){
						c.encolar(null);											
					}
				}
				else{
					// procesar el dato
					if(arbolAux.getDatoRaiz().EsPlanetaDeLaIA())
						distancia= arbol.nivel(arbolAux.getDatoRaiz());
				
					// encolamos hijos
					foreach(var hijo in arbolAux.getHijos())
						c.encolar(hijo);
				}				
			}
			return "La distancia del camino que existe entre el planeta del Bot y la raíz es de " + distancia;
			
		}
		
		//Retorna un texto con el listado de los planetas ubicados en todos los descendientes 
		//del nodo que contiene al planeta del Bot.
		
		public String Consulta2( ArbolGeneral<Planeta> arbol)
		{
			
			return "Implementar";
		}


		public String Consulta3( ArbolGeneral<Planeta> arbol)
		{
			return "Implementar";
		}
		
		public Movimiento CalcularMovimiento(ArbolGeneral<Planeta> arbol)
		{
			//Implementar
			
			return null;
		}
	}
}
