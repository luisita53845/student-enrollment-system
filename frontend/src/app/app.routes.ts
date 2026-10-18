import { Routes } from '@angular/router';
import { Login } from './pages/login/login';
import { Registro } from './pages/registro/registro';
import { Materias } from './pages/materias/materias';
import { AdminLayout } from './pages/admin-layout/admin-layout';
import { Admin } from './pages/admin/admin';
import { AdminProfesores } from './pages/admin-profesores/admin-profesores';
import { AdminMaterias } from './pages/admin-materias/admin-materias';
import { authGuard, adminGuard } from './core/guards';

export const routes: Routes = [
  { path: 'login', component: Login },
  { path: 'registro', component: Registro },
  { path: 'materias', component: Materias, canActivate: [authGuard] },
  {
    path: 'admin',
    component: AdminLayout,
    canActivate: [adminGuard],
    children: [
      { path: 'estudiantes', component: Admin },
      { path: 'profesores', component: AdminProfesores },
      { path: 'materias', component: AdminMaterias },
      { path: '', redirectTo: 'estudiantes', pathMatch: 'full' }
    ]
  },
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: '**', redirectTo: 'login' }
];