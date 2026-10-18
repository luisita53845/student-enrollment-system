import { Component, OnInit, inject, signal } from '@angular/core';
import { ProfesoresService } from '../../core/profesores.service';
import { Profesor } from '../../core/models';

@Component({
  selector: 'app-admin-profesores',
  templateUrl: './admin-profesores.html',
  styleUrl: './admin-profesores.css'
})
export class AdminProfesores implements OnInit {
  private service = inject(ProfesoresService);

  profesores = signal<Profesor[]>([]);
  error = signal('');

  ngOnInit(): void {
    this.service.listar().subscribe({
      next: lista => this.profesores.set(lista),
      error: err => this.error.set(err.error?.mensaje ?? 'No se pudo cargar la lista de profesores.')
    });
  }

  iniciales(nombre: string): string {
    return nombre.split(' ').filter(p => p).slice(0, 2).map(p => p[0].toUpperCase()).join('');
  }
}