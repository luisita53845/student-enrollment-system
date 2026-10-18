import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_URL } from './api.config';
import { Profesor } from './models';

@Injectable({ providedIn: 'root' })
export class ProfesoresService {
  constructor(private http: HttpClient) {}

  listar(): Observable<Profesor[]> {
    return this.http.get<Profesor[]>(`${API_URL}/profesores`);
  }
}