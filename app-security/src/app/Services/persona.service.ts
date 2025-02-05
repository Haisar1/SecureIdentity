import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { environment } from '../../config';  
import { Observable } from 'rxjs';
import { Persona } from '../Interfaces/persona';
import { respuestapersona } from '../Interfaces/respuestapersona';

@Injectable({
  providedIn: 'root'
})
export class PersonaService {

  private url: string = environment.apiURL;
  private api: string = this.url + "personas/";
  constructor(private http: HttpClient) { }

  getPersonas(): Observable<respuestapersona> {
    return this.http.get<respuestapersona>(`${this.api}Lista`);
  }

  add(request: Persona): Observable<Persona> {
    return this.http.post<Persona>(`${this.api}Agregar`, request);
  }

  update(id: number, request: Persona): Observable<Persona> {
    return this.http.put<Persona>(`${this.api}Actualizar/${id}`, request);
  }

  delete(identificador: number): Observable<void> {
    return this.http.delete<void>(`${this.api}Eliminar/${identificador}`);
  }
}
