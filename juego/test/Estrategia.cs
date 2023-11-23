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
			
			// procesamos cola
			while(!c.esVacia()){
				arbolAux = c.desencolar();
				
				// procesar el dato
				if(arbolAux.getDatoRaiz().EsPlanetaDeLaIA())
					distancia= arbol.nivel(arbolAux.getDatoRaiz());
				
				// encolamos hijos
				foreach(var hijo in arbolAux.getHijos())
					c.encolar(hijo);
								
			}
			return "La distancia del camino que existe entre el planeta del Bot y la raíz es de " + distancia;
			
		}
		
		
		//Retorna un texto con el listado de los planetas ubicados en todos los descendientes 
		//del nodo que contiene al planeta del Bot.
		
		public String Consulta2( ArbolGeneral<Planeta> arbol)
		{
			var c = new Cola<ArbolGeneral<Planeta>>();
			ArbolGeneral<Planeta> arbolAux;
			string desc = "";
			//creo "esDesc" para cambiarlo a true en los proximos planetas procesados luego de 
			//encontrar al del bot
			bool esDesc=false;
			// encolamos raiz
			c.encolar(arbol);					
			
			while(!c.esVacia()){
				arbolAux = c.desencolar();
				
				// si encuentro al planeta del bot...
				if(arbolAux.getDatoRaiz().EsPlanetaDeLaIA()){
					if(arbolAux.esHoja()) return "El planeta del bot no tiene descendientes";
					//cambio "esDesc" a true, ya que los proximos seran hijos del bot
					esDesc=true;
					//reseteo la cola, porque solamente deberá contener los planetas descendientes del bot 
					c=new Cola<ArbolGeneral<Planeta>>();
				}
				//si no es planeta de la ia significa que bien, es hijo del planeta del bot o es descendiente
				//de este, por eso tambien evalúo si esDesc es true. Si no lo es significa que aún no se encontro el bot  				
				if(!arbolAux.getDatoRaiz().EsPlanetaDeLaIA() && esDesc) desc+="\n" + arbolAux.getDatoRaiz();
					
				// encolo a los hijos si es que todavia no agregué ningun descendiente, o si ya encontre al bot
				foreach(var hijo in arbolAux.getHijos())
					c.encolar(hijo);
						
			}				
		
			return "Los planetas descendientes del bot son: " + desc;
		}
		
		//Calcula y retorna en un texto la población total y promedio por cada nivel del árbol.
		public String Consulta3( ArbolGeneral<Planeta> arbol)
		{
			var c = new Cola<ArbolGeneral<Planeta>>();
			ArbolGeneral<Planeta> arbolAux;
			int pobTotal=0;
			int prom=0;
			int nivel=0;
			int cant=0;
			string resultado="";
			// encolamos raiz
			c.encolar(arbol);					
			
			// procesamos cola
			while(!c.esVacia()){
				arbolAux = c.desencolar();
				//si el nivel en el que esta el arbol auxiliar es diferente al que estoy calculando 
				//o no hay ningun arbol mas en la cola..
				if(arbol.nivel(arbolAux.getDatoRaiz())!=nivel) {
					prom=pobTotal/cant;
					resultado+="\n" + "En el nivel " + nivel + " la poblacion total es de " + pobTotal + " y la promedio es de " + prom;
					nivel++;
					pobTotal=0;
					prom=0;
					cant=0;
				}					
				pobTotal+=arbolAux.getDatoRaiz().Poblacion();
				cant++;
			
				foreach(var hijo in arbolAux.getHijos()) c.encolar(hijo);
				//Estom es para el ultimo nivel...
				if(c.esVacia()){
					prom=pobTotal/cant;
					resultado+="\n" + "En el nivel " + nivel + " la poblacion total es de " + pobTotal + " y la promedio es de " + prom;
				}
					
			}
			return resultado;
		}			
		
		
			
			
		//Este método calcula y retorna el movimiento apropiado según el estado del juego. El estado del juego 
		//es recibido en el parámetro arbol de tipo ArbolGeneral<Planeta>. Cada nodo del 
		//árbol contiene como dato un planeta del juego.
		public Movimiento CalcularMovimiento(ArbolGeneral<Planeta> arbol)
		{
			var c= new Cola<ArbolGeneral<Planeta>>();
			ArbolGeneral<Planeta> arbolAux;
			c.encolar(arbol);
			while(!c.esVacia()){
				arbolAux=c.desencolar();
				var planeta=arbolAux.getDatoRaiz();
				
				foreach(var hijo in arbolAux.getHijos()){
					var planeta2=hijo.getDatoRaiz();
					c.encolar(hijo);
					if(planeta.EsPlanetaDeLaIA() && planeta.Poblacion()>=2*planeta2.Poblacion()) 
						return new Movimiento(planeta,planeta2);
					
					if(planeta2.EsPlanetaDeLaIA() && planeta2.Poblacion()>=2*planeta.Poblacion()) 
						return new Movimiento(planeta2,planeta);	
				}
			
			}
			
			return null;
		}
	}
}
