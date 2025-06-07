import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DepartmentsListComponent } from './components/departments-list/departments-list.component';
import { DepartmentFormComponent } from './components/department-form/department-form.component';

const routes: Routes = [
  { path: '', component: DepartmentsListComponent },
  { path: 'new', component: DepartmentFormComponent },
  { path: ':id/edit', component: DepartmentFormComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DepartmentsRoutingModule { }
