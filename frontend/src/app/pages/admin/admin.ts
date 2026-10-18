import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../../core/auth.service';
import { EstudiantesService } from '../../core/estudiantes.service';
import { EstudiantePerfil } from '../../core/models';

type Accion = 'inactivar' | 'reactivar' | 'eliminar';
type Filtro = 'todos' | 'activos' | 'inactivos';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.html',
  styleUrl: './admin.css'
})
export class Admin implements OnInit {
  private auth = inject(AuthService);
  private estudiantesService = inject(EstudiantesService);
  private router = inject(Router);
  private temporizador: ReturnType<typeof setTimeout> | null = null;

  estudiantes = signal<EstudiantePerfil[]>([]);
  cargando = signal(false);
  pendiente = signal<{ id: number; accion: Accion } | null>(null);
  aviso = signal<{ texto: string; tipo: 'exito' | 'error' } | null>(null);
  busqueda = signal('');
  filtro = signal<Filtro>('todos');

  nombre = this.auth.obtenerSesion()?.nombre ?? '';

  soloEstudiantes = computed(() => this.estudiantes().filter(e => e.rol === 'Estudiante'));
  total = computed(() => this.soloEstudiantes().length);
  activos = computed(() => this.soloEstudiantes().filter(e => e.activo).length);
  inactivos = computed(() => this.total() - this.activos());
  porcentajeActivos = computed(() =>
    this.total() === 0 ? 0 : Math.round((this.activos() / this.total()) * 100)
  );

  filtrados = computed(() => {
    const texto = this.busqueda().trim().toLowerCase();
    const filtro = this.filtro();
    return this.soloEstudiantes().filter(e => {
      const coincideFiltro =
        filtro === 'todos' || (filtro === 'activos' ? e.activo : !e.activo);
      const coincideTexto =
        !texto ||
        e.nombre.toLowerCase().includes(texto) ||
        e.correo.toLowerCase().includes(texto) ||
        e.documento.includes(texto);
      return coincideFiltro && coincideTexto;
    });
  });

  ngOnInit(): void {
    this.cargar();
  }

  private cargar(): void {
    this.estudiantesService.listar().subscribe({
      next: lista => this.estudiantes.set(lista),
      error: err => this.mostrarAviso(err.error?.mensaje ?? 'No se pudo cargar la lista de estudiantes.', 'error')
    });
  }

  iniciales(nombre: string): string {
    return nombre
      .split(' ')
      .filter(p => p)
      .slice(0, 2)
      .map(p => p[0].toUpperCase())
      .join('');
  }

  cambiarBusqueda(evento: Event): void {
    this.busqueda.set((evento.target as HTMLInputElement).value);
  }

  private mostrarAviso(texto: string, tipo: 'exito' | 'error'): void {
    if (this.temporizador) clearTimeout(this.temporizador);
    this.aviso.set({ texto, tipo });
    this.temporizador = setTimeout(() => this.aviso.set(null), 4000);
  }

  cerrarAviso(): void {
    if (this.temporizador) clearTimeout(this.temporizador);
    this.aviso.set(null);
  }

  esPendiente(id: number, accion: Accion): boolean {
    const p = this.pendiente();
    return !!p && p.id === id && p.accion === accion;
  }

  pedirConfirmacion(id: number, accion: Accion): void {
    this.pendiente.set({ id, accion });
  }

  cancelarConfirmacion(): void {
    this.pendiente.set(null);
  }

  private ejecutar(peticion: Observable<void>, mensaje: string): void {
    this.cargando.set(true);
    peticion.subscribe({
      next: () => {
        this.cargando.set(false);
        this.pendiente.set(null);
        this.mostrarAviso(mensaje, 'exito');
        this.cargar();
      },
      error: err => {
        this.cargando.set(false);
        this.pendiente.set(null);
        this.mostrarAviso(err.error?.mensaje ?? 'No se pudo completar la acción.', 'error');
      }
    });
  }

  inactivar(e: EstudiantePerfil): void {
    this.ejecutar(this.estudiantesService.inactivar(e.id), `${e.nombre} fue inactivado y sus inscripciones se eliminaron.`);
  }

  reactivar(e: EstudiantePerfil): void {
    this.ejecutar(this.estudiantesService.reactivar(e.id), `${e.nombre} fue reactivado. Debe volver a inscribir sus materias.`);
  }

  eliminar(e: EstudiantePerfil): void {
    this.ejecutar(this.estudiantesService.eliminar(e.id), `${e.nombre} fue eliminado definitivamente.`);
  }

  cerrarSesion(): void {
    this.auth.cerrarSesion();
    this.router.navigate(['/login']);
  }
}