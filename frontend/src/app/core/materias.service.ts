import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_URL } from './api.config';
import { Companero, Materia, MateriaResumen } from './models';

@Injectable({ providedIn: 'root' })
export class MateriasService {
  constructor(private http: HttpClient) {}

  listar(): Observable<Materia[]> {
    return this.http.get<Materia[]>(`${API_URL}/materias`);
  }

  companeros(materiaId: number): Observable<Companero[]> {
    return this.http.get<Companero[]>(`${API_URL}/materias/${materiaId}/companeros`);
  }

    resumen(): Observable<MateriaResumen[]> {
    return this.http.get<MateriaResumen[]>(`${API_URL}/materias/resumen`);
  }
}