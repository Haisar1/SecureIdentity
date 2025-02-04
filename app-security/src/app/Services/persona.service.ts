import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { environment } from '../../config';  
import { Observable } from 'rxjs';
import { Persona } from '../Interfaces/persona';

@Injectable({
  providedIn: 'root'
})
export class PersonaService {

  private url: string = environment.apiURL;
  private api: string = this.url + "personas/";
  constructor(private http: HttpClient) { }

  getPersonas(): Observable<Persona[]> {
    return this.http.get<Persona[]>(`${this.api}Lista`);
  }

  add(request: Persona): Observable<Persona> {
    return this.http.post<Persona>(`${this.api}Agregar`, request);
  }

  /*update(request: Persona): Observable<Persona> {
    return this.http.put<Persona>(`${this.api}ActualizarPersona`, request);
  }*/

  delete(identificador: number): Observable<void> {
    return this.http.delete<void>(`${this.api}Eliminar/${identificador}`);
  }
}
