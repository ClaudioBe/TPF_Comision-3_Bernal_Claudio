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
			c.encolar(null);
			// procesamos cola
			while(!c.esVacia()){
				arbolAux = c.desencolar();
				
				// procesar el dato
				if(arbolAux==null){
					if(!c.esVacia()) c.encolar(null);
					distancia++;
				}
				else{
					//si encuentra el planeta del bot, sale del bucle while
					if(arbolAux.getDatoRaiz().EsPlanetaDeLaIA()) break;
					// encolamos hijos
					foreach(var hijo in arbolAux.getHijos()) c.encolar(hijo);
				}				
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
				if(!arbolAux.getDatoRaiz().EsPlanetaDeLaIA() && esDesc) desc+=arbolAux.getDatoRaiz().Poblacion()+ " ";
					
				// encolo a los hijos si es que todavia no agregué ningun descendiente, o si ya encontre al bot
				foreach(var hijo in arbolAux.getHijos())
					c.encolar(hijo);
						
			}				
		
			return "Las poblaciones de los planetas descendientes del bot son: " + desc;
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
			c.encolar(null);
			// procesamos cola
			while(!c.esVacia()){
				arbolAux = c.desencolar();
				//si el arbolAux es null...
				if(arbolAux==null){
					//si la cola no esta vacia, encolo null para que se separen los niveles
					if(!c.esVacia()) c.encolar(null);
					//calculo el promedio entre las poblaciones de todos los planetas del nivel 
					prom=pobTotal/cant;
					//guardo el resultado de lo pedido por nivel
					resultado+="\n" + "Nivel " + nivel + ": población total de " + pobTotal + " y población promedio de "+ prom;
					//incremento nivel para el proximo nivel y seteo a 0 las demas variables para el proximo nivel
					nivel++;
					cant=0;
					prom=0;
					pobTotal=0;
				}
				else{
					//incremento la cantidad de planetas en este nivel
					cant++;
					//sumo la poblacion de cada planeta
					pobTotal+=arbolAux.getDatoRaiz().Poblacion();
					//encolo a los hijos
					foreach(var hijo in arbolAux.getHijos()) c.encolar(hijo);
				}
			}
			return resultado;
		}	
		public List<Planeta> caminos(ArbolGeneral<Planeta> arbol,string p,List<Planeta> camino){
			//si el arbol incluye al bot(p="IA") o al jugador(p="J")
			if(includes(arbol,p)){
				//agrego el planeta al camino
				camino.Add(arbol.getDatoRaiz());
				foreach(var hijo in arbol.getHijos())
					//llamada recursiva con los hijos
					caminos(hijo,p,camino);	
			}
			return camino;
		}
				
		public bool includes(ArbolGeneral<Planeta> arbol, string p){
			//si el parametro p es IA y el planeta de la raiz es de la IA retorna true
			if(p=="IA" && arbol.getDatoRaiz().EsPlanetaDeLaIA()) return true;
			//si el parametro p es J(jugador) y el planeta de la raiz es del jugador retorna true 
			if(p=="J" && arbol.getDatoRaiz().EsPlanetaDelJugador()) return true;
			else {
				//retorna true cuando el hijo incluya al planeta
				foreach(var hijo in arbol.getHijos()) 	
					if(includes(hijo,p)) return true;
			
				return false;
			}
		}
		//Este método calcula y retorna el movimiento apropiado según el estado del juego. El estado del juego 
		//es recibido en el parámetro arbol de tipo ArbolGeneral<Planeta>. Cada nodo del 
		//árbol contiene como dato un planeta del juego.
		public Movimiento CalcularMovimiento(ArbolGeneral<Planeta> arbol)
		{	
			//Lista con el camino de la raiz al pllaneta del bot 
			var caminoAbot=caminos(arbol,"IA",new List<Planeta>());
			//Lista con el camino de la raiz al planeta del jugador 
			var caminoAjugador=caminos(arbol,"J",new List<Planeta>());
			//guardo la posicion del bot y la del jugador
			int posicionDelBot=caminoAbot.Count-1;
			int posicionDelJugador=caminoAjugador.Count-1;
			
			for(int i=caminoAjugador.Count-1;i>=0;i--) {
				for(int j=caminoAbot.Count-1;j>=0;j--) {
					//si encuentro el Ancestro comun minimo 
					if(caminoAjugador[i]==caminoAbot[j]){
						//si el ancestro comun minimo es planeta de la IA(raiz)
						if(caminoAbot[j].EsPlanetaDeLaIA()){
							//si el planeta a atacar no podra conquistarse con un movimiento y hay otro planeta de la IA 
							if(caminoAjugador[i+1].Poblacion()>caminoAbot[j].Poblacion()/2 && caminoAbot.Count>1){
								//si el planeta que va a ayudar tiene menos de 30 naves y hay mas de 2 bots
								if(caminoAbot[j+1].Poblacion()<30 && caminoAbot.Count>2)
									return new Movimiento(caminoAbot[j+2],caminoAbot[j+1]);
		
								return new Movimiento(caminoAbot[j+1],caminoAbot[j]);
							}
							
							return new Movimiento(caminoAbot[j],caminoAjugador[i+1]);
						}	
						//si el jugador esta en un hijo de un bot 
						if(posicionDelJugador>0 && caminoAjugador[posicionDelJugador-1].EsPlanetaDeLaIA()){
							
							//si el planeta del bot no puede conquistar al jugador su nodo anterior le envia tropas
							if(caminoAjugador[posicionDelJugador].Poblacion()>caminoAjugador[posicionDelJugador-1].Poblacion()/2)
								return new Movimiento(caminoAjugador[posicionDelJugador-2],caminoAjugador[posicionDelJugador-1]);
							
							return new Movimiento(caminoAjugador[posicionDelJugador-1],caminoAjugador[posicionDelJugador]);
						}
						//si el padre del bot no es bot
						if(!caminoAbot[posicionDelBot-1].EsPlanetaDeLaIA()) 
							return new Movimiento(caminoAbot[posicionDelBot],caminoAbot[posicionDelBot-1]);
							   
						//si el padre del padre del bot es bot
						if(caminoAbot[posicionDelBot-2].EsPlanetaDeLaIA()) {
							//si el planeta no puede seer conquistado lo ayudan
							if(caminoAbot[posicionDelBot-3].Poblacion()>caminoAbot[posicionDelBot-2].Poblacion()/2)
							{
								//envia tropas al bot que ayuda en caso de que tenga menos de 30 naves 
								if(caminoAbot[posicionDelBot-1].Poblacion()<30)
									return new Movimiento(caminoAbot[posicionDelBot],caminoAbot[posicionDelBot-1]);
								
								return new Movimiento(caminoAbot[posicionDelBot-1],caminoAbot[posicionDelBot-2]);
							}
						
							return new Movimiento(caminoAbot[posicionDelBot-2],caminoAbot[posicionDelBot-3]);
						}
					
						else{
							//si el bot no puede conquistar a su padre
						
							if(caminoAbot[posicionDelBot-2].Poblacion()>caminoAbot[posicionDelBot-1].Poblacion()/2)
							{
								
								if(caminoAbot[posicionDelBot-2].Poblacion()<30)
									return new Movimiento(caminoAbot[posicionDelBot-1],caminoAbot[posicionDelBot-2]);
							
								return new Movimiento(caminoAbot[posicionDelBot],caminoAbot[posicionDelBot-1]);
							}
							
							return new Movimiento(caminoAbot[posicionDelBot-1],caminoAbot[posicionDelBot-2]);
						}
					}
				}
			}
			
			return null;
		}
	}
}
