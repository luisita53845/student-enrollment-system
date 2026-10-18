import { Component, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../core/auth.service';

@Component({
  selector: 'app-registro',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './registro.html',
  styleUrl: './registro.css'
})
export class Registro {
  error = signal('');
  exito = signal('');
  cargando = signal(false);

  form;

  constructor(private fb: FormBuilder, private auth: AuthService, private router: Router) {
    this.form = this.fb.nonNullable.group({
      documento: ['', [Validators.required, Validators.pattern(/^\d{5,20}$/)]],
      nombre: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
      correo: ['', [Validators.required, Validators.email, Validators.maxLength(150)]],
      telefono: ['', [Validators.required, Validators.pattern(/^\+?\d{7,15}$/)]],
      password: ['', [
        Validators.required,
        Validators.minLength(8),
        Validators.maxLength(100),
        Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$/)
      ]]
    });
  }

  enviar(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.error.set('');
    this.exito.set('');
    this.cargando.set(true);

    this.auth.registro(this.form.getRawValue()).subscribe({
      next: respuesta => {
        this.cargando.set(false);
        this.exito.set(respuesta.mensaje);
        setTimeout(() => this.router.navigate(['/login']), 1800);
      },
      error: err => {
        this.cargando.set(false);
        const errores = err.error?.errors;
        if (errores) {
          this.error.set(Object.values(errores).flat().join(' '));
        } else {
          this.error.set(err.error?.mensaje ?? 'No se pudo conectar con el servidor.');
        }
      }
    });
  }
}