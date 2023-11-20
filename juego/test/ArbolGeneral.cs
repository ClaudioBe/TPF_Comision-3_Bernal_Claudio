using System;
using System.Collections.Generic;

namespace DeepSpace
{
	public class ArbolGeneral<T>
	{
		
		private T dato;
		private List<ArbolGeneral<T>> hijos = new List<ArbolGeneral<T>>();

		public ArbolGeneral(T dato) {
			this.dato = dato;
		}
	
		public T getDatoRaiz() {
			return this.dato;
		}
	
		public List<ArbolGeneral<T>> getHijos() {
			return hijos;
		}
	
		public void agregarHijo(ArbolGeneral<T> hijo) {
			this.getHijos().Add(hijo);
		}
	
		public void eliminarHijo(ArbolGeneral<T> hijo) {
			this.getHijos().Remove(hijo);
		}
	
		public bool esHoja() {
			return this.getHijos().Count == 0;
		}
	
		public int altura() {
			if(this.esHoja())
				return 0;
			else{
				int hmax=0;
				foreach(var hijo in this.getHijos()){
					int h= hijo.altura();
					if(h>hmax){
						hmax=h;
					}	
				}
			return hmax +1;
			}
		}
		
		public bool includes(T dato){
			if(this.getDatoRaiz().Equals(dato)) return true;
			else{
				foreach(var a in this.getHijos()){
					if(a.includes(dato)) return a.includes(dato);
				}
				return false;
			}
		}
		public int nivel(T dato) {
			if(this.getDatoRaiz().Equals(dato)) return 0;
			else{
				foreach(var a in this.getHijos()){
					if(a.includes(dato)) return 1 + a.nivel(dato);
				}
			}
			return -1;
		}
	
	}
}