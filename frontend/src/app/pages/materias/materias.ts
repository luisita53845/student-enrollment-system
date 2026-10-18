import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../core/auth.service';
import { InscripcionesService } from '../../core/inscripciones.service';
import { MateriasService } from '../../core/materias.service';
import { Companero, Inscripcion, Materia } from '../../core/models';

@Component({
  selector: 'app-materias',
  templateUrl: './materias.html',
  styleUrl: './materias.css'
})
export class Materias implements OnInit {
  private auth = inject(AuthService);
  private materiasService = inject(MateriasService);
  private inscripcionesService = inject(InscripcionesService);
  private router = inject(Router);

  materias = signal<Materia[]>([]);
  seleccion = signal<number[]>([]);
  inscripcion = signal<Inscripcion | null>(null);
  editando = signal(false);
  companeros = signal<Companero[]>([]);
  materiaCompaneros = signal<string>('');
  error = signal('');
  exito = signal('');
  cargando = signal(false);

  nombre = this.auth.obtenerSesion()?.nombre ?? '';

  seleccionadas = computed(() =>
    this.materias().filter(m => this.seleccion().includes(m.id))
  );
  totalCreditos = computed(() =>
    this.seleccionadas().reduce((suma, m) => suma + m.creditos, 0)
  );
  puedeGuardar = computed(() => this.seleccion().length === 3);
  tieneInscripcion = computed(() => (this.inscripcion()?.materias.length ?? 0) > 0);

  ngOnInit(): void {
    this.materiasService.listar().subscribe({
      next: lista => this.materias.set(lista),
      error: err => this.error.set(err.error?.mensaje ?? 'No se pudieron cargar las materias.')
    });
    this.inscripcionesService.obtenerMia().subscribe({
      next: insc => {
        this.inscripcion.set(insc);
        this.seleccion.set(insc.materias.map(m => m.id));
      },
      error: err => this.error.set(err.error?.mensaje ?? 'No se pudo cargar tu inscripción.')
    });
  }

  iniciarEdicion(): void {
    this.error.set('');
    this.exito.set('');
    this.companeros.set([]);
    this.materiaCompaneros.set('');
    this.seleccion.set(this.inscripcion()?.materias.map(m => m.id) ?? []);
    this.editando.set(true);
  }

  cancelar(): void {
    this.error.set('');
    this.seleccion.set(this.inscripcion()?.materias.map(m => m.id) ?? []);
    this.editando.set(false);
  }

  estaSeleccionada(id: number): boolean {
    return this.seleccion().includes(id);
  }

  motivoBloqueo(m: Materia): string {
    if (this.estaSeleccionada(m.id)) return '';
    if (this.seleccion().length >= 3) return 'Ya elegiste 3 materias.';
    if (this.seleccionadas().some(s => s.profesorId === m.profesorId)) {
      return 'Ya elegiste otra materia de este profesor.';
    }
    return '';
  }

  alternar(m: Materia): void {
    this.error.set('');
    if (this.estaSeleccionada(m.id)) {
      this.seleccion.update(ids => ids.filter(id => id !== m.id));
    } else if (!this.motivoBloqueo(m)) {
      this.seleccion.update(ids => [...ids, m.id]);
    }
  }

  guardar(): void {
    if (!this.puedeGuardar()) return;
    this.error.set('');
    this.exito.set('');
    this.cargando.set(true);

    this.inscripcionesService.definirMia(this.seleccion()).subscribe({
      next: insc => {
        this.cargando.set(false);
        this.inscripcion.set(insc);
        this.seleccion.set(insc.materias.map(m => m.id));
        this.editando.set(false);
        this.exito.set('Inscripción guardada correctamente.');
      },
      error: err => {
        this.cargando.set(false);
        this.error.set(err.error?.mensaje ?? 'No se pudo guardar la inscripción.');
      }
    });
  }

  verCompaneros(m: Materia): void {
    this.error.set('');
    this.materiasService.companeros(m.id).subscribe({
      next: lista => {
        this.companeros.set(lista);
        this.materiaCompaneros.set(m.nombre);
      },
      error: err => this.error.set(err.error?.mensaje ?? 'No se pudieron cargar los compañeros.')
    });
  }

  cerrarSesion(): void {
    this.auth.cerrarSesion();
    this.router.navigate(['/login']);
  }
}