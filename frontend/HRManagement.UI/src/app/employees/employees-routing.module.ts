import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EmployeesListComponent } from './components/employees-list/employees-list.component';
import { EmployeeFormComponent } from './components/employee-form/employee-form.component';

const routes: Routes = [
  { path: '', component: EmployeesListComponent },
  { path: 'new', component: EmployeeFormComponent },
  { path: ':id/edit', component: EmployeeFormComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmployeesRoutingModule { }
