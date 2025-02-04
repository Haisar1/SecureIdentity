import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '../../config';  
import { usuario } from '../Interfaces/usuario';
import { Observable } from 'rxjs';
import { respuestaControl } from '../Interfaces/respuestaControl';
import { login } from '../Interfaces/login';

@Injectable({
     providedIn: 'root'
})
export class ControlService {

    private url: string = environment.apiURL;
    private api: string = this.url + "Control/";
    constructor(private http: HttpClient) { }


     registrarse(objeto: usuario): Observable<respuestaControl> {
          return this.http.post<respuestaControl>(`${this.api}Registrarse`, objeto)
     }

     login(objeto: login): Observable<respuestaControl> {
          return this.http.post<respuestaControl>(`${this.api}login`, objeto)
     }

     validarToken(token: string): Observable<respuestaControl> {
          return this.http.get<respuestaControl>(`${this.api}ValidarToken?token=${token}`)
     }
}