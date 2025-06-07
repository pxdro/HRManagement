import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EmployeesRoutingModule } from './employees-routing.module';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { employeeReducer } from './state/employee.reducer';
import { EmployeeEffects } from './state/employee.effects';
import { EmployeesListComponent } from './components/employees-list/employees-list.component';
import { EmployeeFormComponent } from './components/employee-form/employee-form.component';
import { ReactiveFormsModule } from '@angular/forms';
import { DepartmentsModule } from '../departments/departments.module';

@NgModule({
  imports: [
    CommonModule,
    EmployeesRoutingModule,
    DepartmentsModule,
    StoreModule.forFeature('employees', employeeReducer),
    EffectsModule.forFeature([EmployeeEffects]),
    ReactiveFormsModule 
  ],
  declarations: [
    EmployeesListComponent,
    EmployeeFormComponent
  ]
})
export class EmployeesModule { }
