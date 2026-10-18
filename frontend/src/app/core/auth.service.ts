import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { API_URL } from './api.config';
import { AuthRespuesta, LoginRequest, RegistroRequest } from './models';

const CLAVE_SESION = 'sesion';

@Injectable({ providedIn: 'root' })
export class AuthService {
  constructor(private http: HttpClient) {}

  login(datos: LoginRequest): Observable<AuthRespuesta> {
    return this.http
      .post<AuthRespuesta>(`${API_URL}/auth/login`, datos)
      .pipe(tap(respuesta => sessionStorage.setItem(CLAVE_SESION, JSON.stringify(respuesta))));
  }

  registro(datos: RegistroRequest): Observable<{ mensaje: string }> {
    return this.http.post<{ mensaje: string }>(`${API_URL}/auth/registro`, datos);
  }

  cerrarSesion(): void {
    sessionStorage.removeItem(CLAVE_SESION);
  }

  obtenerSesion(): AuthRespuesta | null {
    const texto = sessionStorage.getItem(CLAVE_SESION);
    return texto ? (JSON.parse(texto) as AuthRespuesta) : null;
  }

  get token(): string | null {
    return this.obtenerSesion()?.token ?? null;
  }

  get estaAutenticado(): boolean {
    const sesion = this.obtenerSesion();
    return !!sesion && new Date(sesion.expira) > new Date();
  }

  get esAdmin(): boolean {
    return this.obtenerSesion()?.rol === 'Administrador';
  }
}