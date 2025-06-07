import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

const routes: Routes = [
  { path: '', redirectTo: 'employees', pathMatch: 'full' },
  {
    path: 'auth',
    loadChildren: () =>
      import('./auth/auth.module').then((m) => m.AuthModule)
  }, // LazyLoading
  {
    path: 'employees',
    loadChildren: () =>
      import('./employees/employees.module').then((m) => m.EmployeesModule),
    canActivate: [authGuard],
  }, // LazyLoading + AuthGuard
  {
    path: 'departments',
    loadChildren: () =>
      import('./departments/departments.module').then((m) => m.DepartmentsModule),
    canActivate: [authGuard],
  }, // LazyLoading + AuthGuard
  { path: '**', redirectTo: 'employees' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
