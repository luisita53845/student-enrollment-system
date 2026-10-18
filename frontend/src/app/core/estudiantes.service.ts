import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_URL } from './api.config';
import { EstudiantePerfil } from './models';

@Injectable({ providedIn: 'root' })
export class EstudiantesService {
  constructor(private http: HttpClient) {}

  listar(): Observable<EstudiantePerfil[]> {
    return this.http.get<EstudiantePerfil[]>(`${API_URL}/estudiantes`);
  }

  inactivar(id: number): Observable<void> {
    return this.http.put<void>(`${API_URL}/estudiantes/${id}/inactivar`, {});
  }

  reactivar(id: number): Observable<void> {
    return this.http.put<void>(`${API_URL}/estudiantes/${id}/reactivar`, {});
  }

  eliminar(id: number): Observable<void> {
    return this.http.delete<void>(`${API_URL}/estudiantes/${id}`);
  }
}