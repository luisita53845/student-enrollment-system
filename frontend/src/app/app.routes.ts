import { Routes } from '@angular/router';
import { Login } from './pages/login/login';
import { Registro } from './pages/registro/registro';
import { Materias } from './pages/materias/materias';
import { Admin } from './pages/admin/admin';
import { authGuard, adminGuard } from './core/guards';

export const routes: Routes = [
  { path: 'login', component: Login },
  { path: 'registro', component: Registro },
  { path: 'materias', component: Materias, canActivate: [authGuard] },
  { path: 'admin', component: Admin, canActivate: [adminGuard] },
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: '**', redirectTo: 'login' }
];