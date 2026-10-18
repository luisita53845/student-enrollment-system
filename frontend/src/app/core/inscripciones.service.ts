import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_URL } from './api.config';
import { Inscripcion } from './models';

@Injectable({ providedIn: 'root' })
export class InscripcionesService {
  constructor(private http: HttpClient) {}

  obtenerMia(): Observable<Inscripcion> {
    return this.http.get<Inscripcion>(`${API_URL}/inscripciones/me`);
  }

  definirMia(materiaIds: number[]): Observable<Inscripcion> {
    return this.http.put<Inscripcion>(`${API_URL}/inscripciones/me`, { materiaIds });
  }
}