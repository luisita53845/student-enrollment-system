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

export interface Materia {
  id: number;
  nombre: string;
  creditos: number;
  profesorId: number;
  nombreProfesor: string;
}

export interface Companero {
  nombre: string;
}

export interface Inscripcion {
  materias: Materia[];
  totalCreditos: number;
  estado: string;
}

export interface EstudiantePerfil {
  id: number;
  documento: string;
  nombre: string;
  correo: string;
  telefono: string | null;
  rol: string;
  activo: boolean;
  fechaRegistro: string;
}   