export interface LoginRequest {
  correo: string;
  password: string;
}

export interface RegistroRequest {
  documento: string;
  nombre: string;
  correo: string;
  telefono: string;
  password: string;
}

export interface AuthRespuesta {
  token: string;
  nombre: string;
  rol: string;
  expira: string;
}