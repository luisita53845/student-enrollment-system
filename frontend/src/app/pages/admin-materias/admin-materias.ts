import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { MateriasService } from '../../core/materias.service';
import { MateriaResumen } from '../../core/models';

@Component({
  selector: 'app-admin-materias',
  templateUrl: './admin-materias.html',
  styleUrl: './admin-materias.css'
})
export class AdminMaterias implements OnInit {
  private service = inject(MateriasService);

  materias = signal<MateriaResumen[]>([]);
  error = signal('');

  totalCreditos = computed(() => this.materias().reduce((s, m) => s + m.creditos, 0));
  totalInscritos = computed(() => this.materias().reduce((s, m) => s + m.inscritosActivos, 0));

  ngOnInit(): void {
    this.service.resumen().subscribe({
      next: lista => this.materias.set(lista),
      error: err => this.error.set(err.error?.mensaje ?? 'No se pudo cargar la lista de materias.')
    });
  }
}